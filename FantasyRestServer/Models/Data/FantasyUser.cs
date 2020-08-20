using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;


namespace FantasyRestServer.Models.Data
{
    public class FantasyUser : IdentityUser
    {
        [ForeignKey("Team")]
        public int TeamRefID { get; set; }
    }
}
