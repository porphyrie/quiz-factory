using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using QuizFactoryAPI.Entities;

namespace QuizFactoryAPI.Data
{
    public partial class QuizFactoryContext : DbContext
    {
        private readonly IConfiguration _config;
        public QuizFactoryContext()
        {
        }

        public QuizFactoryContext(DbContextOptions<QuizFactoryContext> options, IConfiguration config)
            : base(options)
        {
            _config = config;
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<EnrolledStudent> EnrolledStudents { get; set; } = null!;
        public virtual DbSet<QuestionType> QuestionTypes { get; set; } = null!;
        public virtual DbSet<Result> Results { get; set; } = null!;
        public virtual DbSet<Subject> Subjects { get; set; } = null!;
        public virtual DbSet<Test> Tests { get; set; } = null!;
        public virtual DbSet<TestQuestionType> TestQuestionTypes { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_config["QuizFactoryDatabase"]);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Categories)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Categories_Subjects");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasOne(d => d.ProfessorUsernameNavigation)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.ProfessorUsername)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Courses_Users");
            });

            modelBuilder.Entity<EnrolledStudent>(entity =>
            {
                entity.HasOne(d => d.Course)
                    .WithMany()
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EnrolledStudents_Courses");

                entity.HasOne(d => d.StudentUsernameNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.StudentUsername)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EnrolledStudents_Users");
            });

            modelBuilder.Entity<QuestionType>(entity =>
            {
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.QuestionTypes)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuestionTypes_Categories");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.QuestionTypes)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuestionTypes_Subjects");
            });

            modelBuilder.Entity<Result>(entity =>
            {
                entity.HasOne(d => d.QuestionType)
                    .WithMany(p => p.Results)
                    .HasForeignKey(d => d.QuestionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Results_QuestionTypes");

                entity.HasOne(d => d.StudentUsernameNavigation)
                    .WithMany(p => p.Results)
                    .HasForeignKey(d => d.StudentUsername)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Results_Users");

                entity.HasOne(d => d.Test)
                    .WithMany(p => p.Results)
                    .HasForeignKey(d => d.TestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Results_Tests");
            });

            modelBuilder.Entity<Test>(entity =>
            {
                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Tests)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tests_Courses");
            });

            modelBuilder.Entity<TestQuestionType>(entity =>
            {
                entity.HasOne(d => d.QuestionType)
                    .WithMany()
                    .HasForeignKey(d => d.QuestionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TestQuestionTypes_QuestionTypes");

                entity.HasOne(d => d.Test)
                    .WithMany()
                    .HasForeignKey(d => d.TestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TestQuestionTypes_Tests");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("PK_Users_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
