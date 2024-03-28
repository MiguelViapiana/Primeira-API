namespace API.Models;


public class Produto
{
    //Construtor
    public Produto(){}

    public Produto(string nome, string descricao, double valor){

        Nome = nome;
        Descricao = descricao;
        Valor = valor;

    }
    //Atributos ou propriedades = Caracteristicas de um objeto
    public string? Nome {get; set;}
    public string? Descricao { get; set; }
    public double Valor { get; set; }

    // private string nome;
    // private string descricao;

    // public void setNome(string nome)
    // {
    //     this.nome = nome;
    // }
    // public string getNome()
    // {
    //     return this.nome;
    // }

}