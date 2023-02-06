using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MShop.ProductAPI.Configuration;
using MShop.Repository.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//desativa o modelStateInvalid na controller automatizado
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

//configurando a conexao Mysql
var ConnectionString = builder.Configuration.GetConnectionString("Repository");

builder.Services.AddDbContext<RepositoryDbContext>(options =>
    options.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString)));


//carregando as configurações de DependencyInjection
builder.Services.ResolveDepencies();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

//isso foi implementado para que o projeto de teste end2end possa enchergar essa classe
public partial class Program
{

} 