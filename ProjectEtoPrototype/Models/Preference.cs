using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProjectEtoPrototype.Models
{
    public class Preference
    {
        [Key]
        public int PreferenceId { get; set; }

        public string Theme { get; set; } = "LightOrange";

        public int MaxCalories { get; set; } = 1800;

        public int SurahId { get; set; } = 0;
        public int VerseId { get; set; } = 0;

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
