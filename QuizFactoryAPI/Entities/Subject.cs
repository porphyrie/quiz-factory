using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuizFactoryAPI.Entities
{
    public partial class Subject
    {
        public Subject()
        {
            Categories = new HashSet<Category>();
            QuestionTypes = new HashSet<QuestionType>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(50)]
        public string SubjectName { get; set; } = null!;

        [InverseProperty("Subject")]
        public virtual ICollection<Category> Categories { get; set; }
        [InverseProperty("Subject")]
        public virtual ICollection<QuestionType> QuestionTypes { get; set; }
    }
}
