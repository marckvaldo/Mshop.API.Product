using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MShop.ProductAPI.Configuration;
using MShop.Repository.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// essas configurações eu migrei para ControllerCOnfiguration
//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
/*builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();*/


//desativa o modelStateInvalid na controller automatizado
/*builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});*/

//configurando a conexao Mysql
//var ConnectionString = builder.Configuration.GetConnectionString("RepositoryMysql");

//builder.Services.AddDbContext<RepositoryDbContext>(options =>
//    options.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString)));





//configurações de cache REDIS
//var configuracao = builder.Configuration;
//var redisPassword = configuracao["Redis:Password"];
//var redisEndPoint = configuracao["Redis:Endpoint"];
/*builder.Services.AddStackExchangeRedisCache(options =>
{
    options.InstanceName = "Redis";
    options.Configuration = redisEndPoint;
    options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions { Password= redisPassword };
    options.ConfigurationOptions.EndPoints.Add(redisPassword);

});*/


var ConnectionString = builder.Configuration.GetConnectionString("RepositoryMysql");
var configuracao = builder.Configuration;
var redisPassword = configuracao["Redis:Password"];
var redisEndPoint = configuracao["Redis:Endpoint"];

builder.Services.AddAndConfigureController()
    .AddConfigurationModelState()
    .AddDependencyInjection()
    .AddConfigurationMySql(ConnectionString)
    .AddConfigurationRedis(redisPassword,redisEndPoint);


var app = builder.Build();
app.UseDocumentation();

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

//isso foi implementado para que o projeto de teste end2end possa enchergar essa classe
public partial class Program
{

} 