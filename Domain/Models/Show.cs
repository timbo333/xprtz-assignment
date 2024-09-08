namespace Domain.Models
{
    public class Show
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Template Name";
        public string? Language { get; set; }
        public DateTime? Premiered { get; set; }
        public IList<string> Genres { get; set; } = [];
        public string? Summary { get; set; }
    }
}
