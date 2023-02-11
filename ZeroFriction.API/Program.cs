using ZeroFriction.API.Middlewares;
using ZeroFriction.Domain;

var builder = WebApplication.CreateBuilder(args);

var apiKeyConfigValue = builder.Configuration.GetValue<string>("ApiKey");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton(s => new ApplicationConfigurationInfo { ApiKey = apiKeyConfigValue });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
