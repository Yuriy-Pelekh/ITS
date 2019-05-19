CREATE TABLE [Orders](
    [Id] [int] IDENTITY(1, 1) PRIMARY KEY,
	CONSTRAINT User_ID FOREIGN KEY (Id)  REFERENCES [User] (Id),
    [Name] [nvarchar](250) NOT NULL,
    [CreatedDate] [datetime] NOT NULL)
GO