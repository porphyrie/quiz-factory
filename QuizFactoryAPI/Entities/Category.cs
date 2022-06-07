using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuizFactoryAPI.Entities
{
    public partial class Category
    {
        public Category()
        {
            QuestionTypes = new HashSet<QuestionType>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("SubjectID")]
        public int SubjectId { get; set; }
        [StringLength(50)]
        public string CategoryName { get; set; } = null!;

        [ForeignKey("SubjectId")]
        [InverseProperty("Categories")]
        public virtual Subject Subject { get; set; } = null!;
        [InverseProperty("Category")]
        public virtual ICollection<QuestionType> QuestionTypes { get; set; }
    }
}
