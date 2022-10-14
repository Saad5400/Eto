using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectEtoPrototype.Models
{
    public class User
    {
        [Key]
        [DisplayName("User ID")]
        public string UserId { get; set; }

        public List<DailyTask> DailyTasks { get; set; }

        public Preference Preferences { get; set; }
    }

}
