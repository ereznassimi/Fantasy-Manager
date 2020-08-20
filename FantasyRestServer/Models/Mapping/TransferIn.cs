using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FantasyRestServer.Models.Mapping
{
    public class TransferIn
    {
        [Required]
        [ForeignKey("Player")]
        public int PlayerRefID { get; set; }

        [Required]
        public uint AskingPrice { get; set; }
    }
}
