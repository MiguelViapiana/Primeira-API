
using API.Models;
using API.PrimeriaAPI.Models;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);
//Registrar o sereviço de banco de dados
builder.Services.AddDbContext<AppDataContext>();
var app = builder.Build();


//Produto produto = new Produto();
//produto.Nome = "Bolacha";
//Console.WriteLine(produto.Nome);
// produto.setNome("Bolacha");
// Console.WriteLine(produto.getNome());

List<Produto> produtos =
[
    new Produto("Celular", "IOS", 345),
    new Produto("Celular", "Android", 256.8),
    new Produto("Televisão", "LG", 240),
    new Produto("Placa de vídeo", "NVIDIA", 1200)
];


//Funcionalidades da aplicação - EndPoints
// GET: http://localhost:5169/
app.MapGet("/", () => "API de produtos");


// GET: http://localhost:5169/produtos/listar
app.MapGet("/produtos/listar", ([FromServices] AppDataContext ctx) => 
{
    if(ctx.TabelaProdutos.Any()){
        return Results.Ok(ctx.TabelaProdutos.ToList());
    }
    return Results.NotFound("Não existe produtos na tabela");
});


// GET: http://localhost:5169/produtos/buscar/nomedoproduto
app.MapGet("/produtos/buscar/{nome}", ([FromRoute] string nome) =>
    {
        for (int i = 0; i < produtos.Count; i++)
        {
            if (produtos[i].Nome == nome)
            {
                //retronar o produto encontrado
                return Results.Ok(produtos[i]);
            }
        }
        return Results.NotFound("Produto não encontrado");
    }
);


//Pelo Body
// GET: http://localhost:5169/produtos/cadastrar
app.MapPost("/produtos/cadastrar/", ([FromBody] Produto produto,
[FromServices] AppDataContext ctx) =>
{
    //Adicionar o objeto dentro da tabela no bando de dados
    ctx.TabelaProdutos.Add(produto);
    ctx.SaveChanges(); //Aletrar ou remover, precisa do SaveChanges()
    return Results.Created("", produto);

});

// DELETE http://localhost:5169/produtos/deletar/Batata
app.MapDelete("/produtos/deletar/{nome}", ([FromRoute] string nome) =>
{
    for (int i = 0; i < produtos.Count; i++)
    {
        if (produtos[i].Nome == nome)
        {
            produtos.RemoveAt(i);
            return Results.Ok("Produto removido com suscesso");
        }
    }
    return Results.NotFound("Produto não encontrado");
});

// PATCH http://localhost:5169/produtos/alterar/Batata/3.5
app.MapPatch("/produtos/alterar/{nome}/{preco}", ([FromRoute] string nome, [FromRoute]double preco) =>
{
    for (int i = 0; i < produtos.Count; i++)
    {
        if (produtos[i].Nome == nome)
        {
            produtos[i].Valor = preco;
            return Results.Ok("Produto alterado  com sucesso");
        }
    }
    return Results.NotFound("Produto não encontrado");
});

//Exercicio 
//1)Cadastar um produto
//A) Pelo URL
//B) Pelo corpo
//2) Remeção do produto
//3) Alteração do produto
app.Run();

