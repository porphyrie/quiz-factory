using Microsoft.EntityFrameworkCore;

namespace QuizFactory_Backend.Models
{
    //we'll be creating the db and the tables corresponding to these model classes using migration
    //we need to have particular references to those classes here
    //in order to use this quizdbcontext by the framework we have to inject this dbcontext instance into the framework -> open program.cs, where we do the dependency injection
    public class QuizDbContext:DbContext
    {
        public QuizDbContext(DbContextOptions<QuizDbContext> options):base(options)
        {

        }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Participant> Participants { get; set; }

    }
}
