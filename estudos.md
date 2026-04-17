Dentro do Program.cs é onde fazermos todos os comandos rodados no terminal, onde damos as ordens ao terminal.

|------------------------------------------------|
		Criando um livro e um autor

    var author = new Author
    {
        Name = "Lucas Mattos",
        Bio = " Estudante em desenvolvimento",
    };

    var book = new Book
    {
        Title = " Dominando Entity Framework",
        Isbn = "1234567890123",
        PublishedDate = DateTime.Now,
        Author = author
    };

    context.Books.Add(book);
    context.SaveChanges();

|------------------------------------------------|
No código acima, pegamos o nosso objeto Author
e atribuímos um novo conteúdo chamado de author
onde o caracterizamos, dando um nome e uma Bio.

Pegamos o objeto Book e novamente atribuímos um 
novo conteúdo que chamamos de book onde o 
carazterizamos com um Title, Isbn e publisheddDate 
como DateTime.now.
** DateTime.nou define data e hora como a mesma 
do seu computador. 

contexto.Books.Add(book);
Nessa linha, nós adicionamos à base de dados Books
tudo aquilo que atribuímos ao novo conteúdo que chamamos
de book.

Context.Savechanges();
É onde damos o comando para salvar no banco. Como se fosse o 
Ctrl + S para salvar o código no Visual Studio.


|-----------------------------------------------------------------------------------------|
                Lendo os livros da tabela


    var livros = context.Books
        .Include(b => b.Author)
        .ToList();

    Console.WriteLine("\n --- Lista de Livros no Banco ---");

    foreach (var item in livros)
    {
        Console.WriteLine($"Livro: {item.Title} | Autor: {item.Author.Name}");
    }

|----------------------------------------------------------------------------------------|

Explicando a linguagem.
Na primeira linha, temos um " var livros = context.Books ". O que essa linha quer dizer?
R: Estamos criando uma varável, onde apelidamos de livros. Na sequência, temos o context.Books.
Nessa linhao context.Books é onde acessamos ao DbSet o qual criamos no nosso LibraryDataContext.
O Context é como um gerenciador de banco de dados. ele conhece o caminho do servidor. Sem ele o C# não saberia 
onde buscar os dados que estamos pedindo.

Por baixo dos panos acontece da seguinte forma.

Nós digitamos: var livros = Context.Books.
EF "entende": SELECT * FROM Book.

*Nesse momento ele não vai ao banco ainda*
Ele apenas busca os dados quando damos um comando mais específico, como .ToList(); foreach; .FirstOrDefault().

Analogia mais bruta:
Context = Chave que abre as gavetas.
Books é o rótulo da gaveta que eu tô procurando.
var livros é a sua mão pegando a pasta dentro da gaveta.

No final do código, temos a seguinte linha:

    foreach (var item in livros)
        {
            Console.WriteLine($"Livro: {item.Title} | Autor: {item.Author.Name}");
        }
O que ela significa?
foreach -> Para cada
var item -> Variável temporária que representa u livro de cada vez, conforme o laço percorre a lista.
in livros -> Indica em qual coleção estamos mexendo. Se a lista livros tiver 10 registros do banco, esse código rodará 10x.
*O item é um objeto da classe Book*
*Quando escrevemos item.Title, o C# vai buscar exatamente o que está na coluna Title da tabela Book*

Relacionamento:
item.Author -> Como usamos o .Included(x => x.Author) na busca, o C# já sabe quem é o dono do livro. 
.Name -> estamos atravessando a tabela de livros e entrando na tabela de Autores para pegar o Nome.

*Se não tivessemos incluído a linha " .Include(b => b.Author)" antes do foreach, a propriedade item.Author poderia retornar algo vazio (Null)*

Entao o código é basicamente:
"Hei EF eu criei uma variável eu chamei de livros. Use-a para adentrar no meu Dbset.Books para pegar o que vou te pedir a seguir."
"A.. E por gentileza, inclua a classe Author pq eu vou precisar dela depois."
"Traga os dados por gentileza."

"Vamos colocar um texto bonitinho para representar o que estamos fazendo"

"Para cada item exitente dentro do meu DbSet.Books"
"Escreve no meu console Livro: o livro que pedi para você buscar | Autor: Lembra daquele Included que eu falei que ia precisar? Busque, 
por gentileza, na tabela Autor os seus respectivos nomes."



|----------------------------------------------------------------------------------------|
            editando um livro da tabela
            
            var LivroParaEditar = context.Books.FirstOrDefault(b => b.Id == 1);
    
            if (LivroParaEditar != null)
            {
                Console.WriteLine($"Título antigo: {LivroParaEditar.Title}");

                LivroParaEditar.Title = "Entity Framework Core dominado !!";

                context.SaveChanges();

            Console.WriteLine(" !! Título Atualizado com sucesso !! ");
            }
            else
            {
                Console.WriteLine(" livro não encontrado ");
            }

|----------------------------------------------------------------------------------------|

na primeira linha " var livroParaEditar ... " é onde iniciamos o comando. 
Delimitamos a variável e pedimos ao EF ir na tabela Books e pegue o primeiro conteúdo (nesse caso 
livro) que satisfaça a condição exigida. 
Mas qual condição?
 Entre parenteses há o comando "( b => b.Id == 1 )" Ele está dizendo que quer o primeiro livro cujo Id seja igual a 1.

 O que o EF Lê? SELECT TOP(1) * FROM Books WHERE Id = 1

 Na próxima linha temos o seguinte código:
            
            if (LivroParaEditar != null)
Isso é uma linha para prevenção de falhas.
Se, antes de tentarmos realizar a alteração em um item cujo Id é igual a 1, alguém vem e deleta tal item.
Teríamos uma variável vazia. 
Tentar mudar algo vazio poderia vir a causar o mais famoso erro na programação:
        
        NullReferenceException. 
Esse if previne que o nosso código venha a crashar.

Na linha seguinte:

    LivroParaEditar.Title = "..."

Aqui é onde, após já termos definido qual livro iremos modificar, iremos delimitar o que iremos modificar naquele item.
Pela extensão .Title, podemos compreender que estamos querendo alterar o Título do livro em questão.

No final, temos a linha:
    
       Context.SaveChanges()

Onde dizemos ao EF. Já pegamos o livro, escolher o que iremos modificar, já delimitei oq ue quero que você altere.
Agora salve essas mudanças e mande ao banco.

Fechando o if temos:

    else
    {
        Console.WriteLine(" livro não encontrado ");
    }

Nessa linha, caso tudo o que definirmos no if daquilo que ele deve fazer, não aconteça (pois o livro não foi encontrado).
O Console deve me retornar uma mensgaem personalizada: " Livro não encontrado "


|----------------------------------------------------------------------------------------|
                            Deletando um livro do BD
                        

    var LivroParaDeletar = context.Books.FirstOrDefault(b => b.Id == 1);

    if (LivroParaDeletar != null)
    {
        context.Books.Remove(LivroParaDeletar);

            context.SaveChanges();

        Console.WriteLine(" !! Livro deletado com sucesso !!");
    }

|----------------------------------------------------------------------------------------|

Nesse comando, temos as seguintes especificações:

" var LivroParaDeletar = context.Books.FirstOrDefault(b => b.Id == 1); "
Aqui é onde o que queremos. 
Criamos a variável " LivroParaDeletar ". Pedimos para que o EF vá no DBSet buscar o que queremos.
FirsOrDefault queremos o primeiro item que se encaixe na especificação de Id == 1.

Novamente um if. Caso o LivroParaDeletar receba uma variável diferente de nulo ( De fato receba algo e não seja vazia )
faça o seguinte:

" context.Books.Remove(LivroParaDeletar); "
Delete tudo o que estiver contido no que foi puxado para o LivrosParaDeletar.

Context.SaveChanges();
Salve as mudanças realizadas

E imprima uma mensagem personalizada de que o item foi removido com sucesso.


|----------------------------------------------------------------------------------------|
                    Consulta pelo SELECT


    Console.WriteLine("--- Resultado da Busca Rápida ---");

    var termoBusca = "Entity"; // Imagine que o usuário digitou isso

    var buscaOtimizada = context.Books
        .Where(b => b.Title.Contains(termoBusca)) // Filtro parcial
        .Select(b => new {
            b.Title,
            b.Isbn
        }) // Traz apenas essas duas colunas do SQL
        .ToList();

    foreach (var resumo in buscaOtimizada)
    {
        Console.WriteLine($"Livro: {resumo.Title} | ISBN: {resumo.Isbn}");

    }
|----------------------------------------------------------------------------------------|
Nesse código, estamos realizando uma busca pelo nosso banco de dados utilizando a função Select.

Nela, temos algo mais próximo de um banco profissional, onde de fato estamos buscando apenas aquilo que queremos.

Imagine que tenhamos um banco com 50 atribuições (Nome, Bio, Sobrenome, Contato, E-mail, CEP...) mas queremos buscar apenas o nome e email da pessoa.
Não há por que buscarmos todos os dados, quando não precismoas de tudo.
Daí utilizamos o select. Ele funciona como um filtro de busca que irá nos retornar exatamente o que queremos ver.
No caso no código acima, estamos querendo que ele busque apenas o titulo e o Isbn.