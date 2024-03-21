var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<Produto> produtos = new List<Produto>();

//Funcionalidades da aplicação - EndPoints
app.MapGet("/produtos/listar", () => "Listagem de produtos");

//EXERCÍCIO - Cadastar produtos dentro da lista
app.MapPost("/produtos/cadastrar", () => "Cadastro de produtos");

app.Run();

record Produto(string nome, string descricao);
