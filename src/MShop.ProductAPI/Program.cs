using Microsoft.EntityFrameworkCore;
using MShop.ProductAPI.Configuration;
using MShop.ProductAPI.Extension;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAndConfigureController()
    .AddConfigurationModelState()
    .AddConfigurationEvents(builder.Configuration)
    .AddDependencyInjection()
    .AddConfigurationMySql(builder.Configuration)
    .AddConfigurationStorage()
    //.AddConfigurationRedis(builder.Configuration)
    .AddConfigurationSeriLog(builder.Configuration)
    .AddConfigurationHealthChecks();

    builder.Host.UseSerilog();//ativar o log do serilog

var app = builder.Build();

//app.UseHttpLogging(); //aqui para ativar logs de acesso
app.AddMigrateDatabase();
app.AddSetUpRabbiMQ();
app.UseDocumentation();
app.AddMapHealthCheck();



app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();


//isso foi implementado para que o projeto de teste end2end possa enchergar essa classe
public partial class Program
{

} 