CREATE TABLE data."User" (
  "Id" SERIAL PRIMARY KEY,
  "RoleId" int,
  "Name" varchar(255),
  "Mail" varchar(255),
  "PasswordHash" varchar(255)
);

CREATE TABLE data."Role" (
  "Id" SERIAL PRIMARY KEY,
  "Name" varchar(255)
);

CREATE TABLE data."Product" (
  "Id" SERIAL PRIMARY KEY,
  "CategoryId" int,
  "Name" varchar(255),
  "Description" text,
  "Price" float,
  "Image" varchar(255)
);

CREATE TABLE data."Category" (
  "Id" SERIAL PRIMARY KEY,
  "Name" varchar(255)
);

CREATE TABLE data."ProductPromotion" (
  "Id" SERIAL PRIMARY KEY,
  "PromotionId" int,
  "ProductId" int
);

CREATE TABLE data."Promotion" (
  "Id" SERIAL PRIMARY KEY,
  "DiscountPercentage" int
);

ALTER TABLE data."User" ADD FOREIGN KEY ("RoleId") REFERENCES data."Role" ("Id");

ALTER TABLE data."Product" ADD FOREIGN KEY ("CategoryId") REFERENCES data."Category" ("Id");

ALTER TABLE data."ProductPromotion" ADD FOREIGN KEY ("PromotionId") REFERENCES data."Promotion" ("Id");

ALTER TABLE data."ProductPromotion" ADD FOREIGN KEY ("ProductId") REFERENCES data."Product" ("Id");
