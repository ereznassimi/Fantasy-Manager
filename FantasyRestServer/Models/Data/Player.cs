using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FantasyRestServer.Models.Data
{
    public class Player
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public uint Age { get; set; }

        [Required]
        [ForeignKey("Position")]
        public int PositionRefID { get; set; }

        [ForeignKey("Team")]
        public int TeamRefID { get; set; }

        public uint MarketValue { get; set; }
    }
}
