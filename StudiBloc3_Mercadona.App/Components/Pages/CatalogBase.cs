﻿using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using StudiBloc3_Mercadona.Model;
using StudiBloc3_Mercadona.App.Services;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Inputs;

namespace StudiBloc3_Mercadona.App.Components.Pages;

public class CatalogBase : ComponentBase
{
    #region Statements

    // Services
    [Inject] private ApiProductService ApiProductService { get; init; } = default!;
    [Inject] private ApiCategoryService ApiCategoryService { get; init; } = default!;
    [Inject] private ApiPromotionService ApiPromotionService { get; init; } = default!;
    [Inject] private ApiProductPromotionService ApiProductPromotionService { get; init; } = default!;
    [Inject] private AuthenticationService authService { get; init; } = default!;
    
    // Authentication
    protected bool IsUserAuthenticated { get; private set; }
    protected bool IsLoading { get; set; }
    
    // SyncFusion
    protected SfComboBox<string, Category> SfComboBoxNewCategory = null!;
    protected SfComboBox<int, Promotion> SfComboBoxNewPromotion = null!;
    protected bool NewProductPopupIsVisible { get; set; }
    protected bool NewProductPromotionsPopupIsVisible { get; set; }

    // Products
    private List<Product> Products { get; set; } = new();
    protected readonly Product NewProduct = new();
    private Product SelectedProduct = new();
    
    // Products Filter
    private Category[] SelectedCategories { get; set; } = Array.Empty<Category>();
    protected IEnumerable<Product> FilteredProductsByCategories =>
        SelectedCategories.Length > 0
            ? Products.Where(p => SelectedCategories.Any(c => c.Id == p.CategoryId))
            : Products;
    
    
    // Categories
    protected List<Category> Categories { get; private set; } = new();
    private string? NewCategoryName { get; set; }

    // Promotion
    protected List<Promotion> Promotions { get; private set; } = new();
    private readonly Product NewPromotion = new();

    // ProductPromotion
    protected List<ProductPromotion> ProductPromotions { get; set; } = new();
    protected ProductPromotion NewProductPromotion { get; set; } = new()
    {
        StartDate = DateTime.Today,
        EndDate = DateTime.Today.AddDays(7)
    };
    private int CustomPromotionsDiscount { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        
        try
        {
            await AuthConnection();
        
            Products = await ApiProductService.GetAllProductsAsync();
            Categories = await ApiCategoryService.GetAllCategoriesAsync();
            Promotions = await ApiPromotionService.GetAllPromotionsAsync();
            ProductPromotions = await ApiProductPromotionService.GetAllProductPromotionsAsync();
        
            await DeleteExpiredPromotions();
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task AuthConnection()
    {
        var isAuthenticated = await authService.IsUserAuthenticated();
        IsUserAuthenticated = isAuthenticated;
    }

    #endregion

    #region Functions

    protected static string ImageBytesToImageDataUrl(byte[] imageBytes)
    {
        var base64String = Convert.ToBase64String(imageBytes);
        return $"data:image/jpeg;base64,{base64String}";
    }
    
    protected (float originalPrice, float discountedPrice)? CalculateDiscountedPrice(Product product)
    {
        var productPromotion = ProductPromotions.FirstOrDefault(pp => pp.ProductId == product.Id 
                                                                      && pp.StartDate <= DateTime.UtcNow 
                                                                      && pp.EndDate.AddDays(1) > DateTime.UtcNow);
        if (productPromotion is null) return null;
    
        var promotion = Promotions.FirstOrDefault(p => p.Id == productPromotion.PromotionId);
        if (promotion is null) return null;
    
        var discountMultiplier = (100 - promotion.DiscountPercentage) / 100f;
        var discountedPrice = product.Price * discountMultiplier;
    
        return (product.Price, discountedPrice);
    }
    
    protected static int GetRemainingDaysForPromotion(ProductPromotion? promotion)
    {
        if (promotion is null)
        {
            return 0;
        }

        var endDate = promotion.EndDate.Date.AddDays(1);
        var today = DateTime.UtcNow.Date;
        return (endDate - today).Days;
    }
    
    protected string GetProductCategoryName(Product product)
    {
        var category = Categories.FirstOrDefault(c => c.Id == product.CategoryId);
        return category?.Name ?? "Unknown";
    }

    private async Task DeleteExpiredPromotions()
    {
        //This function should be launched at regular intervals from the backend but for simplicity I added it here.
        
        var expiredPromotions = ProductPromotions.Where(pp => pp.EndDate < DateTime.Today).ToList();

        foreach (var expiredPromotion in expiredPromotions)
        {
            try
            {
                await ApiProductPromotionService.DeleteProductPromotionAsync(expiredPromotion);
                ProductPromotions.Remove(expiredPromotion);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        StateHasChanged();
    }
    
    #endregion

    #region Checks

    private static bool CategoryIsValid(Category category)
    {
        return !string.IsNullOrEmpty(category.Name);
    }
    
    private static bool ProductIsValid(Product product)
    {
        const string pattern = "^[a-zA-Z0-9 ]*$";
        var regex = new Regex(pattern);
        
        if (product.CategoryId <= 0) return false;
        if (string.IsNullOrEmpty(product.Name) || !regex.IsMatch(product.Name)) return false;
        if (product.Price <= 0) return false;
        
        return true;
    }
    
    private static bool PromotionIsValid(Promotion promotion)
    {
        return promotion.DiscountPercentage > 0;
    }
    
    private static bool ProductPromotionIsValid(ProductPromotion productPromotion)
    {
        if (productPromotion.ProductId <= 0) return false;
        if (productPromotion.PromotionId <= 0) return false;
        if (productPromotion.StartDate > productPromotion.EndDate) return false;
        
        return true;
    }

    #endregion

    #region SyncFusion
    
    #region SyncFusion_NewProduct

    protected async Task OnSfUploaderNewProductImageChanged(UploadChangeEventArgs args)
    {
        var file = args.Files.FirstOrDefault();
        
        if (file is not null)
        {
            try
            {
                await using var stream = file.File.OpenReadStream(long.MaxValue);
                
                var ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                
                ms.Position = 0;
                NewProduct.Image = ms.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                NewProduct.Image = null;
            }
        }
    }
    
    protected async Task NewProductSubmit()
    {
        var newProduct = new Product
        {
            CategoryId = NewProduct.CategoryId,
            Name = NewProduct.Name,
            Description = NewProduct.Description,
            Price = NewProduct.Price,
            Image = NewProduct.Image?.ToArray()
        };
        
        if (!ProductIsValid(newProduct)) return;

        var addedProduct = await ApiProductService.AddProductAsync(newProduct);
        if (addedProduct != null)
        {
            Products.Add(addedProduct); 
        }

        NewProduct.Image = null;
        CloseNewProductPopup();
        StateHasChanged();
    }

    protected void OpenNewProductPopup()
    {
        NewProductPopupIsVisible = true;
    }

    private void CloseNewProductPopup()
    {
        NewProductPopupIsVisible = false;
    }

    #endregion

    #region SyncFusion_Category
    
    protected void OnSfComboBoxSelectCategoryChanged(Category[]? categories)
    {
        if (categories is null || categories.Length <= 0)
        {
            SelectedCategories = Array.Empty<Category>();
            return;
        }

        SelectedCategories = categories;
    }

    protected void OnSfComboBoxCategoryChanged(string arg)
    {
        if (string.IsNullOrEmpty(arg)) return;
        
        var categoryId = int.Parse(arg);
        NewProduct.CategoryId = categoryId;
    }
    
    protected Task OnSfComboBoxCategoryFiltering(FilteringEventArgs args)
    {
        NewCategoryName = args.Text;
        args.PreventDefaultAction = true;

        var query = new Query().Where(new WhereFilter{Field = "Name", Operator = "contains", value = args.Text, IgnoreCase = true});
        query = !string.IsNullOrEmpty(args.Text) ? query : new Query();

        return SfComboBoxNewCategory.FilterAsync(Categories, query);
    }
    
    protected async Task CreateNewCategory()
    {
        var newCategory = new Category
        {
            Name = NewCategoryName
        };
        
        if (!CategoryIsValid(newCategory)) return;
        
        var addedCategory = await ApiCategoryService.AddCategoryAsync(newCategory);
        if (addedCategory != null)
        {
            await SfComboBoxNewCategory.AddItemsAsync(new List<Category> {addedCategory});
            Categories.Add(addedCategory);
        }
        
        await SfComboBoxNewCategory.HidePopupAsync();
        StateHasChanged();
    }
    
    #endregion

    #region SyncFusion_Promotion
    
    protected void OnSfComboBoxPromotionChanged(int arg)
    {
        if (arg <= 0) return;
        NewPromotion.Id = arg;
    }
    
    protected Task OnSfComboBoxPromotionFiltering(FilteringEventArgs args)
    {
        if (!int.TryParse(args.Text, out _)) return Task.CompletedTask;
        
        CustomPromotionsDiscount = Convert.ToInt32(args.Text);
        args.PreventDefaultAction = true;

        var query = new Query().Where(new WhereFilter{Field = "DiscountPercentage", Operator = "contains", value = args.Text, IgnoreCase = true});
        query = !string.IsNullOrEmpty(args.Text) ? query : new Query();

        return SfComboBoxNewPromotion.FilterAsync(Promotions, query);
    }
    
    protected async Task CreateNewPromotion()
    {
        var newPromotion = new Promotion
        {
            DiscountPercentage = CustomPromotionsDiscount
        };
        
        if (!PromotionIsValid(newPromotion)) return;
        
        var addedPromotion = await ApiPromotionService.AddPromotionAsync(newPromotion);
        if (addedPromotion != null)
        {
            await SfComboBoxNewPromotion.AddItemsAsync(new List<Promotion> {addedPromotion});
            Promotions.Add(addedPromotion);
        }

        await SfComboBoxNewPromotion.HidePopupAsync();
        StateHasChanged();
    }

    protected void OpenNewProductPromotionsPopup(Product product)
    {
        SelectedProduct = product;
        NewProductPromotionsPopupIsVisible = true;
    }

    private void CloseNewProductPromotionsPopup()
    {
        NewProductPromotionsPopupIsVisible = false;
    }

    #endregion

    #region SyncFusion_ProductPromotion

    protected async Task NewProductPromotionSubmit()
    {
        if (NewPromotion.Id == 0) return;
        
        var existingProductPromotion = ProductPromotions.FirstOrDefault(pp => pp.ProductId == SelectedProduct.Id);
        var utcStartDate = DateTime.SpecifyKind(NewProductPromotion.StartDate, DateTimeKind.Utc);
        var utcEndDate = DateTime.SpecifyKind(NewProductPromotion.EndDate, DateTimeKind.Utc);
        
        if (existingProductPromotion is not null)
        {
            var newProductPromotion = new ProductPromotion
            {
                Id = existingProductPromotion.Id,
                ProductId = existingProductPromotion.ProductId,
                PromotionId = NewPromotion.Id,
                StartDate = utcStartDate,
                EndDate = utcEndDate
            };
            
            if (!ProductPromotionIsValid(newProductPromotion)) return;
            
            var index = ProductPromotions.IndexOf(existingProductPromotion);
            ProductPromotions[index] = newProductPromotion;
            
            await ApiProductPromotionService.UpdateProductPromotionAsync(newProductPromotion);
        }
        else
        {
            var newProductPromotion = new ProductPromotion
            {
                ProductId = SelectedProduct.Id,
                PromotionId = NewPromotion.Id,
                StartDate = utcStartDate,
                EndDate = utcEndDate
            };
            
            if (!ProductPromotionIsValid(newProductPromotion)) return;
            
            var addedPromotion = await ApiProductPromotionService.AddProductPromotionAsync(newProductPromotion);
            if (addedPromotion != null)
            {
                ProductPromotions.Add(addedPromotion);
            }
        }
        
        CloseNewProductPromotionsPopup();
    }

    #endregion
    
    #endregion
}