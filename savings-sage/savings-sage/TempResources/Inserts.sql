INSERT INTO [SavingsSage].[dbo].[User]
([Name],[UserName], [EmailAddress], [Birthday])
VALUES
('Teszt Elek', 'tesztelek', 'telek@teszt.com', '1990.01.01');

INSERT INTO [SavingsSage].[dbo].[Colors]
([Name],[HexadecimalCode], [ClassNameColor])
VALUES
('PinkPanther', '#F39FB1', 'bgColorPinkPanther');

INSERT INTO [SavingsSage].[dbo].[Categories]
([Name],[OwnerId], [ColorId])
VALUES
('Self love', 1, 1);

INSERT INTO [SavingsSage].[dbo].[Accounts]
([Name],[Currency],[OwnerId],[Amount],[ParentAccountId],[GroupSharingOption],[CanGoMinus],[ExpirationDate],[Type])
VALUES
( 'Account', 'HUF', 1, 1000, null, 0, 0, null, 'Cash');