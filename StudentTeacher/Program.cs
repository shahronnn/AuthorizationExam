using Microsoft.EntityFrameworkCore;
using NLog;
using StudentTeacher.Extensions;
using StudentTeacher.Repo.Data;
using StudentTeacher.Service.Services;

var builder = WebApplication.CreateBuilder(args);
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
LoggerManager logger = new LoggerManager();

builder.Services.ConfigureResponseCaching();
var connection = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.RegisterDependencies();
builder.Services.ConfigureMapping();
builder.Services.ConfigureLoggerService();

builder.Services.ConfigurePostgresContext(builder.Configuration);
builder.Services.AddDbContext<RepositoryContext>(cont=> cont.UseNpgsql(connection));
builder.Services.ConfigureRepositoryManager();

builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);


builder.Services.ConfigureControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.ConfigureSwagger();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseResponseCaching();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
