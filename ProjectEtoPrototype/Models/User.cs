using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectEtoPrototype.Models
{
    public class User
    {
        [Key]
        [DisplayName("User ID")]
        public string UserId { get; set; }

        [Required]
        public List<DailyTask> DailyTasks { get; set; } = new List<DailyTask>();
        [Required]
        public Preference? Preference { get; set; }

        [NotMapped]
        public string? TempData { get; set; }
    }
}
