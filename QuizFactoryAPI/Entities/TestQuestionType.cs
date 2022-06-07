using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuizFactoryAPI.Entities
{
    [Keyless]
    public partial class TestQuestionType
    {
        [Column("TestID")]
        public int TestId { get; set; }
        [Column("QuestionTypeID")]
        public int QuestionTypeId { get; set; }

        [ForeignKey("QuestionTypeId")]
        public virtual QuestionType QuestionType { get; set; } = null!;
        [ForeignKey("TestId")]
        public virtual Test Test { get; set; } = null!;
    }
}
