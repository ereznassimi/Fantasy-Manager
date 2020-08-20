using FantasyRestServer.Models.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace FantasyRestServer.Models.Mapping
{
    public class FantasyUserOut
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public Team Team { get; set; }

        public IEnumerable<Player> Players { get; set; }
    }
}
