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