using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ProjectEtoPrototype.Models
{
    public class Bank
    {
        [Key]
        public int BankId { get; set; }

        public float Balance { get; set; } = 0f;

        public List<Operation> Operations { get; set; } = new List<Operation>();

        public User User { get; set; }
        public string UserId { get; set; }
    }

    public class Operation
    {
        [Key]
        public int OperationId { get; set; }

        [Required]
        public float Amount { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Class { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.Now;


        // relations

        public Bank? Bank { get; set; }
        public int BankId { get; set; }

        [NotMapped]
        public List<string>? lastClasses { get; set; }
    }
}
