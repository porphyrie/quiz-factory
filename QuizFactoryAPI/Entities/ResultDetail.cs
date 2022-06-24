using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuizFactoryAPI.Entities
{
    public partial class ResultDetail
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("ResultID")]
        public int ResultId { get; set; }
        [Column("QuestionTypeID")]
        public int QuestionTypeId { get; set; }
        [StringLength(1000)]
        public string Question { get; set; } = null!;
        [StringLength(50)]
        public string CorrectAnswer { get; set; } = null!;
        [StringLength(50)]
        public string? Answer { get; set; }
        [StringLength(250)]
        public string? Feedback { get; set; }

        [ForeignKey("QuestionTypeId")]
        [InverseProperty("ResultDetails")]
        public virtual QuestionType QuestionType { get; set; } = null!;
        [ForeignKey("ResultId")]
        [InverseProperty("ResultDetails")]
        public virtual Result Result { get; set; } = null!;
    }
}
