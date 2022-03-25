CREATE TABLE [dbo].[CatalogProduct] (
    [Id]        INT IDENTITY (1, 1) NOT NULL,
    [ProductId] INT NOT NULL,
    [CatalogId] INT NOT NULL,
    CONSTRAINT [PK_CatalogProducts] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CatalogProducts_Catalog] FOREIGN KEY ([CatalogId]) REFERENCES [dbo].[Catalog] ([Id]),
    CONSTRAINT [FK_CatalogProducts_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id])
);

