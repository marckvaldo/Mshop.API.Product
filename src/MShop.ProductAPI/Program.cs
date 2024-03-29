using Microsoft.EntityFrameworkCore;
using MShop.ProductAPI.Configuration;


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


//var configuracao = builder.Configuration;
//var redisPassword = configuracao["Redis:Password"];
//var redisEndPoint = configuracao["Redis:Endpoint"];

builder.Services.AddAndConfigureController()
    .AddConfigurationModelState()
    .AddConfigurationEvents(builder.Configuration)
    .AddDependencyInjection()
    .AddConfigurationMySql(builder.Configuration)
    .AddConfigurationStorage()
    .AddConfigurationRedis(builder.Configuration)
    .AddConfigurationHealthChecks();
    

var app = builder.Build();
app.UseHttpLogging(); //aqui para ativar logs de acesso
app.AddMigrateDatabase();
app.AddSetUpRabbiMQ();
app.UseDocumentation();
app.AddMapHealthCheck();


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