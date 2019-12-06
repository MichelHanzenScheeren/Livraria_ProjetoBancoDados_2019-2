	USE LivrariaMHS;

	SET IDENTITY_INSERT Bairros ON;
	INSERT INTO Bairros (ID, Nome) VALUES (2, N'Da Lapa');
	INSERT INTO Bairros (ID, Nome) VALUES (3, N'São Leopoldo');
	SET IDENTITY_INSERT Bairros OFF;

	SET IDENTITY_INSERT Cidades ON;
	INSERT INTO Cidades (ID, Nome, Estado) VALUES (2, N'Curitiba', N'PR');
	INSERT INTO Cidades (ID, Nome, Estado) VALUES (3, N'Porto Alegre', N'SC');
	SET IDENTITY_INSERT Cidades OFF;

	SET IDENTITY_INSERT Ruas ON;
	INSERT INTO Ruas (ID, Nome) VALUES (2, N'Padre Feijó');
	INSERT INTO Ruas (ID, Nome) VALUES (3, N'Arthur Magalhães');
	SET IDENTITY_INSERT Ruas OFF;

	SET IDENTITY_INSERT Clientes ON;
	INSERT INTO Clientes (ID, Nome, Sexo, Email, CPF, DataNascimento, Telefone, Numero, RuaID, BairroID, CidadeID) VALUES (1, N'Marcos Arnaldo', 1, N'marcosA@gmail.com', N'112.165.161-61', N'2000-03-09 00:00:00', N'(45)99969-9760', N'2242', 3, 3, 2);
	INSERT INTO Clientes (ID, Nome, Sexo, Email, CPF, DataNascimento, Telefone, Numero, RuaID, BairroID, CidadeID) VALUES (2, N'Júlia Pires', 0, N'julia_p@gmail.com', N'121.458.976-21', N'1999-10-02 00:00:00', N'(45)98832-2014', N'1147', 2, 2, 3);
	SET IDENTITY_INSERT Clientes OFF;

	SET IDENTITY_INSERT Categorias ON;
	INSERT INTO Categorias (ID, Nome) VALUES (1, N'Inteligência Artificial');
	INSERT INTO Categorias (ID, Nome) VALUES (2, N'Banco de Dados');
	INSERT INTO Categorias (ID, Nome) VALUES (3, N'Internet das Coisas');
	INSERT INTO Categorias (ID, Nome) VALUES (4, N'Realidade Virtual');
	INSERT INTO Categorias (ID, Nome) VALUES (5, N'Sistemas');
	SET IDENTITY_INSERT Categorias OFF;

	SET IDENTITY_INSERT Autores ON;
	INSERT INTO Autores (ID, Nome) VALUES (2, N'Samuel Greengard');
	INSERT INTO Autores (ID, Nome) VALUES (4, N'Pedro Domingos');
	INSERT INTO Autores (ID, Nome) VALUES (5, N'Alessandro Candeas');
	INSERT INTO Autores (ID, Nome) VALUES (6, N'Christopher J. Date');
	INSERT INTO Autores (ID, Nome) VALUES (7, N'Seth Stephens-Davidowitz');
	INSERT INTO Autores (ID, Nome) VALUES (9, N'eu mesmo');
	INSERT INTO Autores (ID, Nome) VALUES (10, N'Everton Coimbra de Araújo');
	SET IDENTITY_INSERT Autores OFF;

	SET IDENTITY_INSERT Livros ON;
	INSERT INTO Livros (ID, Titulo, Paginas, Preco, Edicao, Ano, AutorID, ContentType, Nome) VALUES (1, N'Todo Mundo Mente', 352, CAST(64.00 AS Decimal(18, 2)), 1, 2018, 7, N'image/jpeg', N'51IZ2Kg27dL._SX353_BO1,204,203,200_.jpg');
	INSERT INTO Livros (ID, Titulo, Paginas, Preco, Edicao, Ano, AutorID, ContentType, Nome) VALUES (2, N'The Internet of Things', 230, CAST(45.50 AS Decimal(18, 2)), 2, 2015, 2, N'image/jpeg', N'31EMjDwaUrL.jpg');
	INSERT INTO Livros (ID, Titulo, Paginas, Preco, Edicao, Ano, AutorID, ContentType, Nome) VALUES (3, N'O Algoritmo Mestre', 344, CAST(75.32 AS Decimal(18, 2)), 1, 2017, 4, N'image/jpeg', N'51ptaEgBpqL.jpg');
	INSERT INTO Livros (ID, Titulo, Paginas, Preco, Edicao, Ano, AutorID, ContentType, Nome) VALUES (4, N'Hybris: IA e a revanche do inconsciente', 187, CAST(68.90 AS Decimal(18, 2)), 1, 2018, 5, N'image/jpeg', N'51WozkXHy+L.jpg');
	INSERT INTO Livros (ID, Titulo, Paginas, Preco, Edicao, Ano, AutorID, ContentType, Nome) VALUES (5, N'SQL e Teoria Relacional', 536, CAST(75.00 AS Decimal(18, 2)), 4, 2015, 6, N'image/jpeg', N'51+ohszN-RL._SX358_BO1,204,203,200_.jpg');
	INSERT INTO Livros (ID, Titulo, Paginas, Preco, Edicao, Ano, AutorID, ContentType, Nome) VALUES (7, N'ASP .NET Core MVC', 248, CAST(52.00 AS Decimal(18, 2)), 1, 2018, 10, N'image/jpeg', N'8ROjv5OVfks54j7FvDehRuYHax0-et96hKHyplURGe4_large.jpg');
	SET IDENTITY_INSERT Livros OFF;

	SET IDENTITY_INSERT LivroCategoria ON;
	INSERT INTO LivroCategoria (ID, LivroID, CategoriaID) VALUES (2, 2, 3);
	INSERT INTO LivroCategoria (ID, LivroID, CategoriaID) VALUES (3, 1, 2);
	INSERT INTO LivroCategoria (ID, LivroID, CategoriaID) VALUES (4, 3, 1);
	INSERT INTO LivroCategoria (ID, LivroID, CategoriaID) VALUES (5, 4, 1);
	INSERT INTO LivroCategoria (ID, LivroID, CategoriaID) VALUES (6, 4, 4);
	INSERT INTO LivroCategoria (ID, LivroID, CategoriaID) VALUES (7, 5, 2);
	INSERT INTO LivroCategoria (ID, LivroID, CategoriaID) VALUES (9, 7, 5);
	SET IDENTITY_INSERT LivroCategoria OFF;

	SET IDENTITY_INSERT Vendas ON;
	INSERT INTO Vendas (ID, Data, Quantidade, ValorUnitario, ClienteID, LivroID) VALUES (1, N'2019-11-03 16:48:00', 1, CAST(52.00 AS Decimal(18, 2)), 1, 7);
	INSERT INTO Vendas (ID, Data, Quantidade, ValorUnitario, ClienteID, LivroID) VALUES (3, N'2019-11-03 16:49:00', 2, CAST(68.90 AS Decimal(18, 2)), 2, 4);
	INSERT INTO Vendas (ID, Data, Quantidade, ValorUnitario, ClienteID, LivroID) VALUES (4, N'2019-11-03 16:49:00', 1, CAST(75.32 AS Decimal(18, 2)), 2, 3);
	INSERT INTO Vendas (ID, Data, Quantidade, ValorUnitario, ClienteID, LivroID) VALUES (5, N'2019-11-03 16:50:00', 1, CAST(45.50 AS Decimal(18, 2)), 1, 2);
	SET IDENTITY_INSERT Vendas OFF;

	INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20191205021336_CiacaoBanco', N'2.2.6-servicing-10079')

