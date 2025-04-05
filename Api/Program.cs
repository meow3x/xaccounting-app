using Api.Database;
using Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContextPool<ApplicationDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("ApplicationContext"))
        .UseLowerCaseNamingConvention();
       
});
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();

app.UseCors();
// Configure the HTTP request pipeline.
if (true/*app.Environment.IsDevelopment()*/)
{
    app.MapScalarApiReference("/");
    app.MapOpenApi();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
