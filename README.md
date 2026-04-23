Sistema de Gestão de Biblioteca ( Console App )

Este é um sistema de gerenciamneto de biblioteca desenvolvido como parte dos meus estudos em SI.
O projeto utiliza C# com o Entity Framework Core para persistência de dados em um banco SQL Server.

Funcionalidades:

-> CRUD Completo: Gerenciamneto de Livros e Autores
-> vinculos Muitos-Para-Muitos: Associação dinâmica entre livros e categorias.
-> Busca Inteligente: Filtragem de livros por título.
-> Relatório de Auditoria: Identificação de registros "órfãos" ( Autores sem livros ).
-> Tratamento de Erros: Validação de integridade referencial e entrada de usuários.

Tecnologias Utilizadas:

-> .NET 8 / C#
-> Entity Frameqork Core (ORM)
-> SQL Server / LocalDB
-> LINQ ( Language Integrated Query )


Estrutura do Projeto:

├── Data/               # Contexto do Banco de Dados (DbContext)
├── Models/             # Classes de Entidade (Book, Author, Category)
├── Services/           # Lógica de Negócio (LibraryServices)
├── Scripts/            # Scripts SQL para população do banco
└── Program.cs          # Interface de Console e Menus


Como rodar o projeto:

1. Clonar o repositório:

- Colar o Link do GitHub aqui...

2. Configurar o banco de dados:

- Certifique-se de ter o SQL Server (ou LocalDb) instalado.
- O sistema criará o banco automaticamente ao roda pela primeira vez ( Migrations )

3. Popular os Dados:

- Execute o script localizado em /Scripts/PopularBancvo.sql no seu SQL Server Managment Studio para inserir os dados de teste.

4. Execute o projeto:

- dotnet run


Aprendizado:

Durante o desenvlvimento, apliquei conceitos de integridade referencial, lidando com erros e Chjaves Estrangeiras e garantindo que a 
exclusão de autores com livros vinculados fosse tratada de forma amigável ao usuário. Também explorei o uso do Include do EF Core
para otimizar consultas em relacionamentos complexos.


DESENVOLVIDO POR: Lucas Mattos