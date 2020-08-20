using System.Collections.Generic;


namespace FantasyRestServer.Models.Mapping
{
    public class ErrorOut
    {
        public int Status { get; set; }

        public string Title { get; set; }

        public List<string> Errors { get; set; }
    }
}
