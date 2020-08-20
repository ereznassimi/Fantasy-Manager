using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FantasyRestServer.Models.Data
{
    public class Transfer
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [Required]
        [ForeignKey("Player")]
        public int PlayerRefID { get; set; }

        [Required]
        public uint AskingPrice { get; set; }
    }
}
