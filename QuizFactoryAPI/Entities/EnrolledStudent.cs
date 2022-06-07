using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QuizFactoryAPI.Entities
{
    [Keyless]
    public partial class EnrolledStudent
    {
        [Column("CourseID")]
        public int CourseId { get; set; }
        [StringLength(50)]
        public string StudentUsername { get; set; } = null!;

        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; } = null!;
        [ForeignKey("StudentUsername")]
        public virtual User StudentUsernameNavigation { get; set; } = null!;
    }
}
