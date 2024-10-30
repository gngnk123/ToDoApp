CREATE TABLE [dbo].[ToDo]
(
	[Id] INT NOT NULL PRIMARY KEY identity, 
    [Title] NCHAR(20) NOT NULL, 
    [Description] NCHAR(100) NULL, 
    [DateTime] DATETIME NOT NULL
)
