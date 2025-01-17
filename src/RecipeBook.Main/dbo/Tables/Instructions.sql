﻿CREATE TABLE [dbo].[Instructions]
(
	[RecipeId] UNIQUEIDENTIFIER NOT NULL , 
    [OrdinalPosition] INT NOT NULL, 
    [Instruction] NVARCHAR(250) NOT NULL, 
    PRIMARY KEY ([RecipeId], [OrdinalPosition]), 
    CONSTRAINT [FK_Instructions_Recipes_Id] FOREIGN KEY ([RecipeId]) REFERENCES [Recipes]([Id]) 
)
