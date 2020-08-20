using System.ComponentModel.DataAnnotations;


namespace FantasyRestServer.Models.Mapping
{
    public class TeamIn
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Country { get; set; }

        public uint TotalValue { get; set; }

        public uint AvailableBudget { get; set; }
    }
}
