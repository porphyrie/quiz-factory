using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizFactory_Backend.Models
{
    //I've just installed 3 entity core packages, core, sqlserver, tools
    //create model classes for the db
    public class Question
    {
        [Key] //we mark this with this key attribute
        public int QnId { get; set; } //this is the primary key - the column corresponding to this property will be the primary key for this table 
        [Column(TypeName = "nvarchar(250)")]
        public string QnInWords { get; set; } //if we leave these model strings properties as they are, when it is converted to the actual db, the sql server columns coressponding to these properties will be having the data type of n vocab max which is more than what we need
                                              //so in order to limit the data type, we will specify the required data type here
        [Column(TypeName = "nvarchar(50)")]
        public string? ImageName { get; set; } //? declares it as a nullable property
        [Column(TypeName = "nvarchar(50)")]
        public string Option1 { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Option2 { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Option3 { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Option4 { get; set; }
        public int Answer { get; set; }
    }
}
