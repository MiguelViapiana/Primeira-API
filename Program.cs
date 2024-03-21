var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


//Funcionalidades da aplicação - EndPoints
app.MapGet("/produtos/listar", () => "Listagem de produtos");

app.MapGet("/produtos/cadastrar", () => "Cadastro de produtos");

app.Run();
