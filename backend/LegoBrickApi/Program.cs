using Microsoft.EntityFrameworkCore;
using LegoBrickApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Database Context (SQLite)
builder.Services.AddDbContext<BrickContext>(options =>
    options.UseSqlite("Data Source=bricks.db"));

// Add CORS to allow Vue app to connect
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:8080")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowVueApp");

app.UseAuthorization();
app.MapControllers();

app.Run();