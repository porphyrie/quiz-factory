using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuizFactoryAPI.Entities
{
    public partial class Result
    {
        public Result()
        {
            ResultDetails = new HashSet<ResultDetail>();
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(50)]
        public string StudentUsername { get; set; } = null!;
        [Column("TestID")]
        public int TestId { get; set; }
        public double? Grade { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? FinishTime { get; set; }

        [ForeignKey("StudentUsername")]
        [InverseProperty("Results")]
        public virtual User StudentUsernameNavigation { get; set; } = null!;
        [ForeignKey("TestId")]
        [InverseProperty("Results")]
        public virtual Test Test { get; set; } = null!;
        [InverseProperty("Result")]
        public virtual ICollection<ResultDetail> ResultDetails { get; set; }
    }
}
