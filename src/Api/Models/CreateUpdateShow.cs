using Domain.Models;

namespace Api.Models
{
    public class CreateUpdateShow
    {
        public string? Name { get; set; }
        public string? Language { get; set; }
        public DateTime? Premiered { get; set; }
        public IList<string> Genres { get; set; } = [];
        public string? Summary { get; set; }

        public static implicit operator Show(CreateUpdateShow d) => 
            new()
            {
                Name = d.Name ?? "Templated Name",
                Language = d.Language,
                Genres = d.Genres,
                Summary = d.Summary,
                Premiered = d.Premiered
            };
    }
}
