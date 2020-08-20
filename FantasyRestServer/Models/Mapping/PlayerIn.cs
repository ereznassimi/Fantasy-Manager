using System.ComponentModel.DataAnnotations;


namespace FantasyRestServer.Models.Mapping
{
    public class PlayerIn
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public uint Age { get; set; }

        [Required]
        public int PositionRefID { get; set; }

        public int TeamRefID { get; set; }

        public uint MarketValue { get; set; }
    }
}
