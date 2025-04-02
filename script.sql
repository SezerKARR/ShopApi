CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;
ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `Categories` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Slug` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Categories` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Products` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Description` longtext CHARACTER SET utf8mb4 NOT NULL,
    `CategoryId` int NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Slug` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Products` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Products_Categories_CategoryId` FOREIGN KEY (`CategoryId`) REFERENCES `Categories` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_Products_CategoryId` ON `Products` (`CategoryId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250301164433_Initial', '9.0.1');

ALTER TABLE `Products` MODIFY COLUMN `Description` longtext CHARACTER SET utf8mb4 NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250303061247_product', '9.0.1');

ALTER TABLE `Products` ADD `ImageUrl` longtext CHARACTER SET utf8mb4 NULL;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250305130317_Image', '9.0.1');

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250312122812_Comment', '9.0.1');

ALTER TABLE `Comments` MODIFY COLUMN `Content` varchar(300) CHARACTER SET utf8mb4 NULL;

ALTER TABLE `Categories` ADD `mainCategoryId` int NULL;

CREATE TABLE `MainCategories` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` longtext CHARACTER SET utf8mb4 NULL,
    `Slug` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_MainCategories` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_Categories_mainCategoryId` ON `Categories` (`mainCategoryId`);

ALTER TABLE `Categories` ADD CONSTRAINT `FK_Categories_MainCategories_mainCategoryId` FOREIGN KEY (`mainCategoryId`) REFERENCES `MainCategories` (`Id`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250313103735_MainCategory', '9.0.1');

ALTER TABLE `Products` ADD `BasketId` int NULL;

ALTER TABLE `Products` ADD `Price` decimal(65,30) NOT NULL DEFAULT 0.0;

CREATE TABLE `Baskets` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` longtext CHARACTER SET utf8mb4 NULL,
    `UserIp` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NULL,
    `Slug` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_Baskets` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_Products_BasketId` ON `Products` (`BasketId`);

ALTER TABLE `Products` ADD CONSTRAINT `FK_Products_Baskets_BasketId` FOREIGN KEY (`BasketId`) REFERENCES `Baskets` (`Id`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250319122952_Basket', '9.0.1');

ALTER TABLE `Products` DROP FOREIGN KEY `FK_Products_Baskets_BasketId`;

ALTER TABLE `Products` DROP INDEX `IX_Products_BasketId`;

ALTER TABLE `Products` DROP COLUMN `BasketId`;

CREATE TABLE `BasketItems` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `BasketId` int NOT NULL,
    `Quantity` int NOT NULL,
    `ProductId` int NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NULL,
    `Slug` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_BasketItems` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_BasketItems_Baskets_BasketId` FOREIGN KEY (`BasketId`) REFERENCES `Baskets` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_BasketItems_Products_ProductId` FOREIGN KEY (`ProductId`) REFERENCES `Products` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_BasketItems_BasketId` ON `BasketItems` (`BasketId`);

CREATE INDEX `IX_BasketItems_ProductId` ON `BasketItems` (`ProductId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250319131437_BasketItem', '9.0.1');

ALTER TABLE `Baskets` MODIFY COLUMN `UserIp` varchar(120) CHARACTER SET utf8mb4 NOT NULL;

ALTER TABLE `Baskets` MODIFY COLUMN `UserId` varchar(120) CHARACTER SET utf8mb4 NULL;

CREATE TABLE `Users` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Email` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Role` int NULL,
    CONSTRAINT `PK_Users` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250320080225_User', '9.0.1');

ALTER TABLE `Categories` DROP FOREIGN KEY `FK_Categories_MainCategories_mainCategoryId`;

ALTER TABLE `Categories` RENAME COLUMN `mainCategoryId` TO `MainCategoryId`;

ALTER TABLE `Categories` RENAME INDEX `IX_Categories_mainCategoryId` TO `IX_Categories_MainCategoryId`;

CREATE TABLE `CategoryTrees` (
    `AncestorId` int NOT NULL,
    `DescendantId` int NOT NULL,
    `Depth` int NOT NULL,
    CONSTRAINT `PK_CategoryTrees` PRIMARY KEY (`AncestorId`, `DescendantId`),
    CONSTRAINT `FK_CategoryTrees_Categories_AncestorId` FOREIGN KEY (`AncestorId`) REFERENCES `Categories` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_CategoryTrees_Categories_DescendantId` FOREIGN KEY (`DescendantId`) REFERENCES `Categories` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_CategoryTrees_DescendantId` ON `CategoryTrees` (`DescendantId`);

ALTER TABLE `Categories` ADD CONSTRAINT `FK_Categories_MainCategories_MainCategoryId` FOREIGN KEY (`MainCategoryId`) REFERENCES `MainCategories` (`Id`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250322093545_CategoryTree', '9.0.1');

ALTER TABLE `Categories` ADD `ParentCategoryId` int NULL;

ALTER TABLE `Categories` ADD `ParentId` int NULL;

CREATE INDEX `IX_Categories_ParentCategoryId` ON `Categories` (`ParentCategoryId`);

ALTER TABLE `Categories` ADD CONSTRAINT `FK_Categories_Categories_ParentCategoryId` FOREIGN KEY (`ParentCategoryId`) REFERENCES `Categories` (`Id`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250322114312_Category', '9.0.1');

ALTER TABLE `Categories` DROP FOREIGN KEY `FK_Categories_MainCategories_MainCategoryId`;

DROP TABLE `CategoryTrees`;

DROP TABLE `MainCategories`;

ALTER TABLE `Categories` DROP INDEX `IX_Categories_MainCategoryId`;

ALTER TABLE `Categories` DROP COLUMN `MainCategoryId`;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250324095930_CategoryProduct', '9.0.1');

CREATE TABLE `ProductFilterValue` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `ProductId` int NOT NULL,
    `CategoryFilterId` int NOT NULL,
    `Value` longtext CHARACTER SET utf8mb4 NULL,
    `Name` longtext CHARACTER SET utf8mb4 NULL,
    `Slug` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_ProductFilterValue` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_ProductFilterValue_Products_ProductId` FOREIGN KEY (`ProductId`) REFERENCES `Products` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_ProductFilterValue_ProductId` ON `ProductFilterValue` (`ProductId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250325094028_CategoryUpdate', '9.0.1');

COMMIT;

