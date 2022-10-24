using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectEtoPrototype.Models
{
    public class Preference
    {
        [Key]
        public int PreferenceId { get; set; }

        public string Theme { get; set; } = "LightOrange";

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int CurrentCalories { get; set; } = 0;
        public int MaxCalories { get; set; } = 1800;

        public int SurahId { get; set; } = 1;
        public int VerseId { get; set; } = 1;

        public User User { get; set; }
        public string UserId { get; set; }
    }
}
