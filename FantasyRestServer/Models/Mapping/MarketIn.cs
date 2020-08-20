using System.ComponentModel.DataAnnotations;


namespace FantasyRestServer.Models.Mapping
{
    public class MarketIn
    {
        [Required]
        public int PlayerID { get; set; }
    }
}
