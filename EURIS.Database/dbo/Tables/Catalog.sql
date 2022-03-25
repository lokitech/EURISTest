CREATE TABLE [dbo].[Catalog] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Code]        NVARCHAR (10) NOT NULL,
    [Description] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Catalog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

