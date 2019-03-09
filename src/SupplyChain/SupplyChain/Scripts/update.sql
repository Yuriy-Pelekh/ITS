--##1
CREATE TABLE [User](
    [Id] [int] IDENTITY(1, 1) PRIMARY KEY,
    [FirstName] [nvarchar](250) NOT NULL,
    [LastName] [nvarchar](250) NOT NULL,
    [UpdatedDate] [datetime] NOT NULL)
GO

INSERT INTO [User] ([FirstName], [LastName], [UpdatedDate])
     VALUES ('Yuriy', 'Pelekh', GETUTCDATE())
GO

--##2
INSERT INTO [User] ([FirstName], [LastName], [UpdatedDate])
     VALUES ('Iryna', 'Verbenko', GETUTCDATE())
GO
