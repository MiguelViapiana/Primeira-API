namespace API.PrimeriaAPI.Models;

using API.Models;
using Microsoft.EntityFrameworkCore;

//Classes que representa Entity Framework Core : Code First
public class AppDataContext : DbContext
{
   //Representação das classes que vão virar tabelas do banco de dados
   public DbSet<Produto> TabelaProdutos { get; set; }

   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
   {
      //Configuração da conexão com banco de dados
      optionsBuilder.UseSqlite("Data Source=app.db");

   }

}