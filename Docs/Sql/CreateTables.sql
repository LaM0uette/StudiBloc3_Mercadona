CREATE TABLE data."User" (
  "Id" int PRIMARY KEY,
  "RoleId" int,
  "Name" varchar(255),
  "Mail" varchar(255),
  "PasswordHash" varchar(255)
);

CREATE TABLE data."Role" (
  "Id" int PRIMARY KEY,
  "Name" varchar(255)
);

CREATE TABLE data."Product" (
  "Id" int PRIMARY KEY,
  "CategoryId" int,
  "Name" varchar(255),
  "Description" text,
  "Price" float,
  "Image" varchar(255)
);

CREATE TABLE data."Category" (
  "Id" int PRIMARY KEY,
  "Name" varchar(255)
);

CREATE TABLE data."ProductPromotion" (
  "Id" int PRIMARY KEY,
  "PromotionId" int,
  "ProductId" int
);

CREATE TABLE data."Promotion" (
  "Id" int PRIMARY KEY,
  "DiscountPercentage" int
);

ALTER TABLE data."User" ADD FOREIGN KEY ("RoleId") REFERENCES data."Role" ("Id");

ALTER TABLE data."Product" ADD FOREIGN KEY ("CategoryId") REFERENCES data."Category" ("Id");

ALTER TABLE data."ProductPromotion" ADD FOREIGN KEY ("PromotionId") REFERENCES data."Promotion" ("Id");

ALTER TABLE data."ProductPromotion" ADD FOREIGN KEY ("ProductId") REFERENCES data."Product" ("Id");
