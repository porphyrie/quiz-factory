using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using QuizFactory_Backend.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(
//        policy =>
//        {
//            policy.WithOrigins("*");
//        });
//});

builder.Services.AddDbContext<QuizDbContext>(options => //lambda expression, we have to provide the provider for this db, which database are we going to use for this application
options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection"))); //we pass the connection string saved in appsettings.json, we have done the dependency injection
// after build, we are going with migration which is the process of creating the actual physical db with these model classes provided here
// Package Manager Console Window-> Add Migration "initial create" ->the actual sql server script will be created according to the model that we have inside the project, which is saved inside the folder migrations
// Update-Database - in order to execute the sql script for creating the db 
//afte that, create api controllers for those model classes

var app = builder.Build();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Images")),
    RequestPath="/Images"
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//the web api will block requests from another application which is hosted on a different domain or a different port number
//enable cors for this port number here
app.UseCors(options => options.WithOrigins("https://localhost:3000").AllowAnyMethod().AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

app.Run();
