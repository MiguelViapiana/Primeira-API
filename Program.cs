
using System.ComponentModel.DataAnnotations;
using API.Models;
using API.PrimeriaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
//Registrar o sereviço de banco de dados
builder.Services.AddDbContext<AppDataContext>();

//Configurar a politica de CORS para liberar o acesso total
builder.Services.AddCors(
    options => options.AddPolicy("Acesso Total", configs => configs.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod())
);

var app = builder.Build();


//Funcionalidades da aplicação - EndPoints
// GET: http://localhost:5169/
app.MapGet("/", () => "API de produtos");


// GET: http://localhost:5169/produtos/listar
app.MapGet("/produtos/listar", ([FromServices] AppDataContext ctx) =>
{
    if (ctx.TabelaProdutos.Any())
    {
        return Results.Ok(ctx.TabelaProdutos.ToList());
    }
    return Results.NotFound("Não existe produtos na tabela");
});


// GET: http://localhost:5169/produtos/buscar/iddoproduto
app.MapGet("/produtos/buscar/{id}", ([FromRoute] string id,
[FromServices] AppDataContext ctx) =>
    {
        Produto? produto = ctx.TabelaProdutos.Find(id);
        if (produto == null)
        {
            return Results.NotFound("Produto não encontrado");
        }
        return Results.Ok(produto);
    }
);


//Pelo Body
// GET: http://localhost:5169/produtos/cadastrar
app.MapPost("/produtos/cadastrar/", ([FromBody] Produto produto,
[FromServices] AppDataContext ctx) =>

{

    List<ValidationResult> erros = new List<ValidationResult>();
    if (!Validator.TryValidateObject(produto, new ValidationContext(produto), erros, true))
    {
        return Results.BadRequest(erros);
    }


    Produto? produtoBuscado = ctx.TabelaProdutos.FirstOrDefault(x => x.Nome == produto.Nome);
    if (produtoBuscado is null)
    {
        //Adicionar o objeto dentro da tabela no bando de dados
        ctx.TabelaProdutos.Add(produto);
        ctx.SaveChanges(); //Aletrar ou remover, precisa do SaveChanges()
        return Results.Created("", produto);
    }
    return Results.BadRequest("Já existe um produto com o mesmo nome");


});

// DELETE http://localhost:5169/produtos/deletar/id
app.MapDelete("/produtos/deletar/{id}", ([FromRoute] string id, [FromServices] AppDataContext ctx) =>
{
    Produto? produto = ctx.TabelaProdutos.FirstOrDefault(x => x.Id == id);

    if (produto is null)
    {

        return Results.NotFound("Produto não encontrado");
    }
    ctx.TabelaProdutos.Remove(produto);
    ctx.SaveChanges();
    return Results.Ok("");
});

// PATCH http://localhost:5169/produtos/alterar/id
app.MapPatch("/produtos/alterar/{id}", ([FromRoute] string id, [FromBody] Produto novoProduto,
[FromServices] AppDataContext ctx) =>
{
    var produto = ctx.TabelaProdutos.Find(id);
    if (produto is null)
    {
        return Results.NotFound("Produto não encontrado");

    }
    produto.Nome = novoProduto.Nome;
    produto.Descricao = novoProduto.Descricao;
    produto.Valor = novoProduto.Valor;

    ctx.SaveChanges();
    return Results.Ok("Produto atualizado com suscesso!!");
});
app.UseCors("Acesso Total");
app.Run();

