using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuizFactoryAPI.Entities
{
    public partial class Test
    {
        public Test()
        {
            Results = new HashSet<Result>();
            TestQuestionTypes = new HashSet<TestQuestionType>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("CourseID")]
        public int CourseId { get; set; }
        [StringLength(50)]
        public string TestName { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime TestDate { get; set; }
        public int TestDuration { get; set; }

        [ForeignKey("CourseId")]
        [InverseProperty("Tests")]
        public virtual Course Course { get; set; } = null!;
        [InverseProperty("Test")]
        public virtual ICollection<Result> Results { get; set; }
        [InverseProperty("Test")]
        public virtual ICollection<TestQuestionType> TestQuestionTypes { get; set; }
    }
}
