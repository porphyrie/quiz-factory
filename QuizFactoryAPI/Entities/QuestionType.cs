using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuizFactoryAPI.Entities
{
    public partial class QuestionType
    {
        public QuestionType()
        {
            Results = new HashSet<Result>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("SubjectID")]
        public int SubjectId { get; set; }
        [Column("CategoryID")]
        public int CategoryId { get; set; }
        [StringLength(250)]
        public string QuestionTemplateString { get; set; } = null!;
        [Unicode(false)]
        public string ConfigurationFile { get; set; } = null!;
        [Unicode(false)]
        public string ProducingFile { get; set; } = null!;
        [Unicode(false)]
        public string? GrammarFile { get; set; }

        [ForeignKey("CategoryId")]
        [InverseProperty("QuestionTypes")]
        public virtual Category Category { get; set; } = null!;
        [ForeignKey("SubjectId")]
        [InverseProperty("QuestionTypes")]
        public virtual Subject Subject { get; set; } = null!;
        [InverseProperty("QuestionType")]
        public virtual ICollection<Result> Results { get; set; }
    }
}
