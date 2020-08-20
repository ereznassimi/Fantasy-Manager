using System.ComponentModel.DataAnnotations;


namespace FantasyRestServer.Models.Mapping
{
    public class TransferOut
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
        public int PositionRefID { get; set; }

        public string Team { get; set; }

        public uint Value { get; set; }

        [Required]
        public uint Price { get; set; }
    }
}
