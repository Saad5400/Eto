using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectEtoPrototype.Models
{
    public class DailyTask
    {
        [Key]
        public int TaskID { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        public string Name { get; set; }

        //public string Description { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }
    }
}
