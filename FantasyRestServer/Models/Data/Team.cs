using System.ComponentModel.DataAnnotations;


namespace FantasyRestServer.Models.Data
{
    public class Team
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Country { get; set; }

        public uint TotalValue { get; set; }

        public uint AvailableBudget { get; set; }
    }
}
