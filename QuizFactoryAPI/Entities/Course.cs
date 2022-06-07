using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuizFactoryAPI.Entities
{
    public partial class Course
    {
        public Course()
        {
            Tests = new HashSet<Test>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(50)]
        public string CourseName { get; set; } = null!;
        [StringLength(50)]
        public string ProfessorUsername { get; set; } = null!;

        [ForeignKey("ProfessorUsername")]
        [InverseProperty("Courses")]
        public virtual User ProfessorUsernameNavigation { get; set; } = null!;
        [InverseProperty("Course")]
        public virtual ICollection<Test> Tests { get; set; }
    }
}
