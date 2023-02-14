using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ZeroFriction.API.Middlewares;
using ZeroFriction.DB.Domain.Contracts;
using ZeroFriction.DB.Domain.Dtos;
using ZeroFriction.DB.Services;
using ZeroFriction.Domain;
using ZeroFriction.Domain.Contracts;
using ZeroFriction.Services;

var builder = WebApplication.CreateBuilder(args);

var apiKeyConfigValue = builder.Configuration.GetValue<string>("ApiKey");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton(s => new ApplicationConfigurationInfo { ApiKey = apiKeyConfigValue });
builder.Services.AddSingleton(s => {
    DbInfo dbInfo = new DbInfo
    {
        DatabaseName = builder.Configuration.GetValue<string>("DBConnection:DatabaseName"),
        DatabaseKey = builder.Configuration.GetValue<string>("DBConnection:DatabaseKey"),
        DatabaseURI = builder.Configuration.GetValue<string>("DBConnection:DatabaseURI"),
        CollectionId = builder.Configuration.GetValue<string>("DBConnection:CollectionId")
    };
    return dbInfo;
});
builder.Services.AddSingleton<IDocumentDbService, CosmosDbService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();

// Configuring Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseMiddleware<CommonExceptionHandlerMiddleware>();
app.UseMiddleware<AuthenticationMiddleware>();

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
