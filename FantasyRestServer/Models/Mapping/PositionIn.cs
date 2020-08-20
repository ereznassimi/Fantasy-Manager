using System.ComponentModel.DataAnnotations;


namespace FantasyRestServer.Models.Mapping
{
    public class PositionIn
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1, 10)]
        public uint AmountInTeam { get; set; }
    }
}
