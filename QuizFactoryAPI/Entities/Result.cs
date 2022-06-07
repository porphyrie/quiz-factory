using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuizFactoryAPI.Entities
{
    public partial class Result
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(50)]
        public string StudentUsername { get; set; } = null!;
        [Column("TestID")]
        public int TestId { get; set; }
        [Column("QuestionTypeID")]
        public int QuestionTypeId { get; set; }
        [StringLength(250)]
        public string Question { get; set; } = null!;
        [StringLength(50)]
        public string CorrectAnswer { get; set; } = null!;
        [StringLength(50)]
        public string? Answer { get; set; }
        [StringLength(250)]
        public string? Feedback { get; set; }

        [ForeignKey("QuestionTypeId")]
        [InverseProperty("Results")]
        public virtual QuestionType QuestionType { get; set; } = null!;
        [ForeignKey("StudentUsername")]
        [InverseProperty("Results")]
        public virtual User StudentUsernameNavigation { get; set; } = null!;
        [ForeignKey("TestId")]
        [InverseProperty("Results")]
        public virtual Test Test { get; set; } = null!;
    }
}
