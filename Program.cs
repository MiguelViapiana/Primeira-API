
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);
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
app.MapGet("/produtos/listar", () => produtos);

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
app.MapPost("/produtos/cadastrar/", ([FromBody] Produto produto) =>
{
    //Adicionar o objeto dentro da lista
    produtos.Add(produto);
    return Results.Created("", produto);

});

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
//Pela URL
//app.MapPost("/produtos/cadastrar/{nome}/{descricao}/{valor}", ([FromRoute]////string nome, [FromRoute]string descricao, [FromRoute]double valor) => {
//Preencher o objeto pelo o construtor
//Produto produto = new Produto(nome, descricao, valor);

//Preencher objeto pelos os atributos
//produto.Nome = nome;
//produto.Descricao = descricao;
//produto.Valor = valor;
//Adicionar o objeto dentro da lista
//produtos.Add(produto);
//return Results.Created("", produto);
//});



//Exercicio 
//1)Cadastar um produto
//A) Pelo URL
//B) Pelo corpo
//2) Remeção do produto
//3) Alteração do produto
app.Run();

