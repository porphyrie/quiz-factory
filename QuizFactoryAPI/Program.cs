using Microsoft.EntityFrameworkCore;
using QuizFactoryAPI.Authorization;
using QuizFactoryAPI.Data;
using QuizFactoryAPI.Helpers;
using QuizFactoryAPI.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var services = builder.Services;

services.AddControllers();

services.AddDbContext<QuizFactoryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("QuizFactoryDatabase")));

services.AddCors();

// serialize enums as strings in api responses (e.g. Role)
services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// configure DI for application services
services.AddScoped<IJwtUtils, JwtUtils>();
services.AddScoped<IUserService, UserService>();
services.AddScoped<ICourseService, CourseService>();
services.AddScoped<ISubjectService, SubjectService>();
services.AddScoped<ICategoryService, CategoryService>();
services.AddScoped<IQuestionService, QuestionService>();
services.AddScoped<ITestService, TestService>();
services.AddScoped<IResultService, ResultService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

// global cors policy
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

// global error handler
app.UseMiddleware<ErrorHandlerMiddleware>();

// custom jwt auth middleware
app.UseMiddleware<JwtMiddleware>();

//app.UseAuthorization();

app.MapControllers();

app.Run();

//Scaffold-DbContext "Server=.;Database=QuizFactory;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -ContextDir Data -OutputDir Entities -DataAnnotation -f
