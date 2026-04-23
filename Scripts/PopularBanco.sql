USE [LibraryDb];
GO

-- 1. CADASTRO DE LIVROS (Agora usando 'Book' e 'Author' em tudo)
-- Note que usei TOP 1 para garantir que ele pegue apenas um ID por vez
INSERT INTO dbo.Book (Title, Isbn, AuthorId, PublishedDate) VALUES 
('O Hobbit', '9788595084741', (SELECT TOP 1 Id FROM dbo.Author WHERE Name = 'J.R.R. Tolkien'), GETDATE()),
('A Sociedade do Anel', '9788595084772', (SELECT TOP 1 Id FROM dbo.Author WHERE Name = 'J.R.R. Tolkien'), GETDATE()),
('Eu, Robô', '9788576572007', (SELECT TOP 1 Id FROM dbo.Author WHERE Name = 'Isaac Asimov'), GETDATE()),
('Fundação', '9788576571062', (SELECT TOP 1 Id FROM dbo.Author WHERE Name = 'Isaac Asimov'), GETDATE()),
('1984', '9788535914849', (SELECT TOP 1 Id FROM dbo.Author WHERE Name = 'George Orwell'), GETDATE()),
('A Revolução dos Bichos', '9788535909555', (SELECT TOP 1 Id FROM dbo.Author WHERE Name = 'George Orwell'), GETDATE()),
('Frankenstein', '9788582850381', (SELECT TOP 1 Id FROM dbo.Author WHERE Name = 'Mary Shelley'), GETDATE()),
('Assim Falou Zaratustra', '9788535920192', (SELECT TOP 1 Id FROM dbo.Author WHERE Name = 'Friedrich Nietzsche'), GETDATE()),
('E Não Sobrou Nenhum', '9788525057044', (SELECT TOP 1 Id FROM dbo.Author WHERE Name = 'Agatha Christie'), GETDATE()),
('Morte no Nilo', '9788525433169', (SELECT TOP 1 Id FROM dbo.Author WHERE Name = 'Agatha Christie'), GETDATE()),
('It: A Coisa', '9788535924770', (SELECT TOP 1 Id FROM dbo.Author WHERE Name = 'Stephen King'), GETDATE()),
('O Iluminado', '9788535924787', (SELECT TOP 1 Id FROM dbo.Author WHERE Name = 'Stephen King'), GETDATE()),
('Dom Casmurro', '9788535914856', (SELECT TOP 1 Id FROM dbo.Author WHERE Name = 'Machado de Assis'), GETDATE()),
('A Hora da Estrela', '9788532527561', (SELECT TOP 1 Id FROM dbo.Author WHERE Name = 'Clarice Lispector'), GETDATE()),
('Arquitetura Limpa', '9788550804606', (SELECT TOP 1 Id FROM dbo.Author WHERE Name = 'Robert Martin'), GETDATE());