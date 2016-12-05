CREATE SCHEMA [People]
    AUTHORIZATION [dbo];

GO
CREATE SCHEMA [Product]
    AUTHORIZATION [dbo];

GO
CREATE SCHEMA [Order]
    AUTHORIZATION [dbo];

GO
CREATE TABLE [People].[Account] (
    [AccountID]   INT            IDENTITY (1, 1) NOT NULL,
    [Username]    NVARCHAR (64)  NOT NULL,
    [Password]    NVARCHAR (256) NOT NULL,
    [Role]        NVARCHAR (32)  NOT NULL,
    [CreatedDate] DATETIME       NOT NULL,
    [UpdatedDate] DATETIME       NOT NULL,
    [Active]      BIT            NOT NULL,
    PRIMARY KEY CLUSTERED ([AccountID] ASC),
    UNIQUE NONCLUSTERED ([Username] ASC)
);

GO
CREATE TABLE [People].[Customer] (
    [CustomerID]  INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (256) NOT NULL,
    [AccountID]   INT            NOT NULL,
    [AddressID]   INT            NOT NULL,
    [CreatedDate] DATETIME       NOT NULL,
    [UpdatedDate] DATETIME       NOT NULL,
    [Active]      BIT            NOT NULL,
    PRIMARY KEY CLUSTERED ([CustomerID] ASC)
);

GO
CREATE TABLE [Product].[Category] (
    [CategoryID]  INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (256) NOT NULL,
    [CreatedDate] DATETIME       NOT NULL,
    [UpdatedDate] DATETIME       NOT NULL,
    [Active]      BIT            NOT NULL,
    PRIMARY KEY CLUSTERED ([CategoryID] ASC)
);

GO
CREATE TABLE [Product].[Product] (
    [ProductID]   INT             IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (256)  NOT NULL,
    [Description] NVARCHAR (1024) NOT NULL,
    [Price]       MONEY           NOT NULL,
    [ImagePath]   NVARCHAR (64)   NOT NULL,
    [CategoryID]  INT             NOT NULL,
    [CreatedDate] DATETIME        NOT NULL,
    [UpdatedDate] DATETIME        NOT NULL,
    [Active]      BIT             NOT NULL,
    PRIMARY KEY CLUSTERED ([ProductID] ASC)
);

GO
CREATE TABLE [Order].[Address] (
    [AddressID]   INT            IDENTITY (1, 1) NOT NULL,
    [Street]      NVARCHAR (256) NOT NULL,
    [City]        NVARCHAR (64)  NOT NULL,
    [State]       CHAR (2)       NOT NULL,
    [Zip]         CHAR (5)       NOT NULL,
    [CreatedDate] DATETIME       NOT NULL,
    [UpdatedDate] DATETIME       NOT NULL,
    [Active]      BIT            NOT NULL,
    PRIMARY KEY CLUSTERED ([AddressID] ASC)
);

GO
CREATE TABLE [Order].[Order] (
    [OrderID]     INT           IDENTITY (1, 1) NOT NULL,
    [CustomerID]  INT           NULL,
    [AddressID]   INT           NOT NULL,
    [StripeID]    NVARCHAR (64) NOT NULL,
    [CreatedDate] DATETIME      NOT NULL,
    [UpdatedDate] DATETIME      NOT NULL,
    [Active]      BIT           NOT NULL,
    PRIMARY KEY CLUSTERED ([OrderID] ASC)
);

GO
CREATE TABLE [Order].[Cart] (
    [CustomerID]  INT      NOT NULL,
    [CreatedDate] DATETIME NOT NULL,
    [UpdatedDate] DATETIME NOT NULL,
    [Active]      BIT      NOT NULL,
    PRIMARY KEY CLUSTERED ([CustomerID] ASC)
);

GO
CREATE TABLE [Order].[OrderProducts] (
    [OrderID]   INT NOT NULL,
    [ProductID] INT NOT NULL,
    CONSTRAINT [pk_OrderProducts] PRIMARY KEY CLUSTERED ([OrderID] ASC, [ProductID] ASC)
);

GO
CREATE TABLE [Order].[CartProducts] (
    [CartID]    INT NOT NULL,
    [ProductID] INT NOT NULL,
    CONSTRAINT [pk_CartProducts] PRIMARY KEY CLUSTERED ([CartID] ASC, [ProductID] ASC)
);

GO
ALTER TABLE [People].[Customer]
    ADD CONSTRAINT [fk_Customer_AccountID] FOREIGN KEY ([AccountID]) REFERENCES [People].[Account] ([AccountID]);

GO
ALTER TABLE [People].[Customer]
    ADD CONSTRAINT [fk_Customer_AddressID] FOREIGN KEY ([AddressID]) REFERENCES [Order].[Address] ([AddressID]);

GO
ALTER TABLE [Order].[Order]
    ADD CONSTRAINT [fk_Order_CustomerID] FOREIGN KEY ([CustomerID]) REFERENCES [People].[Customer] ([CustomerID]);

GO
ALTER TABLE [Order].[Order]
    ADD CONSTRAINT [fk_Order_AddressID] FOREIGN KEY ([AddressID]) REFERENCES [Order].[Address] ([AddressID]);

GO
ALTER TABLE [Order].[OrderProducts]
    ADD CONSTRAINT [fk_OrderProducts_OrderID] FOREIGN KEY ([OrderID]) REFERENCES [Order].[Order] ([OrderID]);

GO
ALTER TABLE [Order].[OrderProducts]
    ADD CONSTRAINT [fk_OrderProducts_ProductID] FOREIGN KEY ([ProductID]) REFERENCES [Product].[Product] ([ProductID]);

GO
ALTER TABLE [Order].[CartProducts]
    ADD CONSTRAINT [fk_CartProducts_CartID] FOREIGN KEY ([CartID]) REFERENCES [Order].[Cart] ([CustomerID]);

GO
ALTER TABLE [Order].[CartProducts]
    ADD CONSTRAINT [fk_CartProducts_ProductID] FOREIGN KEY ([ProductID]) REFERENCES [Product].[Product] ([ProductID]);

GO
ALTER TABLE [People].[Account]
    ADD CHECK ([Role] IN ('admin', 'customer'));