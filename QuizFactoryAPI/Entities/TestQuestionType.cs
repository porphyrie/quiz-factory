using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuizFactoryAPI.Entities
{
    public partial class TestQuestionType
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("TestID")]
        public int TestId { get; set; }
        [Column("QuestionTypeID")]
        public int QuestionTypeId { get; set; }

        [ForeignKey("QuestionTypeId")]
        [InverseProperty("TestQuestionTypes")]
        public virtual QuestionType QuestionType { get; set; } = null!;
        [ForeignKey("TestId")]
        [InverseProperty("TestQuestionTypes")]
        public virtual Test Test { get; set; } = null!;
    }
}
