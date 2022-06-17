using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuizFactoryAPI.Entities
{
    public partial class User
    {
        public User()
        {
            Courses = new HashSet<Course>();
            Results = new HashSet<Result>();
            CoursesNavigation = new HashSet<Course>();
        }

        [Key]
        [StringLength(50)]
        public string Username { get; set; } = null!;
        [StringLength(50)]
        public string LastName { get; set; } = null!;
        [StringLength(50)]
        public string FirstName { get; set; } = null!;
        [StringLength(50)]
        public string Role { get; set; } = null!;
        [StringLength(60)]
        public string PasswordHash { get; set; } = null!;

        [InverseProperty("ProfessorUsernameNavigation")]
        public virtual ICollection<Course> Courses { get; set; }
        [InverseProperty("StudentUsernameNavigation")]
        public virtual ICollection<Result> Results { get; set; }

        [ForeignKey("StudentUsername")]
        [InverseProperty("StudentUsernames")]
        public virtual ICollection<Course> CoursesNavigation { get; set; }
    }
}
