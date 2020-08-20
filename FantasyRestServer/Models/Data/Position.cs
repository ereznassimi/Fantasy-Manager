using System.ComponentModel.DataAnnotations;


namespace FantasyRestServer.Models.Data
{
    public class Position
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public uint AmountInTeam { get; set; }
    }
}
