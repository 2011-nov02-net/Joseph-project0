CREATE DATABASE P0;

DROP TABLE Item;
DROP TABLE "Order";
DROP TABLE StoreCustomer;
DROP TABLE Store;
DROP TABLE Product;
CREATE TABLE Product(
	Id INT NOT NULL IDENTITY PRIMARY KEY,
	Name NVARCHAR(150) UNIQUE NOT NULL,
	Price INT not NULL,
);

CREATE TABLE StoreCustomer(
	Id INT NOT NULL IDENTITY PRIMARY KEY,
	Name NVARCHAR(150) UNIQUE NOT NULL,
);

CREATE TABLE Store(
	Id INT NOT NULL IDENTITY PRIMARY KEY,
	Name NVARCHAR(150) NOT NULL,
	Location NVARCHAR(150) UNIQUE NOT NULL,
);

CREATE TABLE "Order"(
	Id INT NOT NULL IDENTITY PRIMARY KEY,
	StoreId INT FOREIGN KEY REFERENCES Store (Id) NOT NULL,
	CustomerId INT FOREIGN KEY REFERENCES StoreCustomer (Id) NULL,
	Time DateTime NOT NULL
);

--Items may be part of an inventory or an Order
CREATE TABLE Item(
	Id INT NOT NULL IDENTITY PRIMARY KEY,
	OrderID INT FOREIGN KEY REFERENCES "Order" (Id) NULL,
	StoreID INT FOREIGN KEY REFERENCES Store (Id) NULL,
	ProductId INT FOREIGN KEY REFERENCES Product (Id) NOT NULL, 
	Quantity INT NULL
);

--some starting values 

Insert INTO Product VALUES
	('Banana',3), ('Bread',4), ('Plunger',10);

Insert INTO StoreCustomer VALUES
	('Sam Alito'), ('Ruth Ginsburg'), ('Antonin Scalia');

Insert INTO Store VALUES
	('StoreA', 'Texas'), ('StoreB', 'Arizona'), ('StoreC', 'California');

Insert INTO "Order" VALUES
	(1, 1, GETDATE()), (1, 2, GETDATE()), (2, 3, GETDATE()),(3, 2, GETDATE());

--insert some Order items
INSERT INTO Item (OrderId,ProductId,Quantity) VALUES
	(1,1,10),(1,2,5);
--insert some inventory items
INSERT INTO Item (StoreId,ProductId,Quantity) VALUES
	(2,1,10),(3,3,5),(2,2,15),(3,2,20);
SELECT * FROM Item;
SELECT * FROM "Store";
SELECT * FROM "Order";