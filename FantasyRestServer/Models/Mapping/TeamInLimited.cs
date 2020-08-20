using System.ComponentModel.DataAnnotations;


namespace FantasyRestServer.Models.Mapping
{
    public class TeamInLimited
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Country { get; set; }
    }
}
