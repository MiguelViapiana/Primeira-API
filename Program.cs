
using API.Models;
using API.PrimeriaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


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
app.MapGet("/produtos/buscar/{nome}",  async ([FromRoute] string nome, [FromServices] AppDataContext ctx) =>
    {
        var produto = await ctx.TabelaProdutos.FirstOrDefaultAsync(p => p.Nome == nome);
        if( produto != null){
            return Results.Ok(produto);
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
app.MapDelete("/produtos/deletar/{nome}", async ([FromRoute] string nome, [FromServices] AppDataContext ctx) =>
{   
    var produto = await ctx.TabelaProdutos.FirstOrDefaultAsync(p => p.Nome == nome);
        if( produto != null){
            ctx.TabelaProdutos.Remove(produto);
            ctx.SaveChanges();
            return Results.Ok("Produto removido com suscesso!!");
        }
    return Results.NotFound("Produto não encontrado");
});

// PATCH http://localhost:5169/produtos/alterar/Batata/3.5
app.MapPatch("/produtos/alterar/{nome}", async ([FromRoute] string nome, [FromBody] Produto novoProduto, [FromServices] AppDataContext ctx) =>
{
    var produto = await ctx.TabelaProdutos.FirstOrDefaultAsync(p => p.Nome == nome);
        if( produto != null){
            produto.Nome = novoProduto.Nome;
            produto.Descricao = novoProduto.Descricao;
            produto.Valor = novoProduto.Valor;

            ctx.SaveChanges();
            return Results.Ok("Produto atualizado com suscesso!!");
        }
    return Results.NotFound("Produto não encontrado");
});

app.Run();

