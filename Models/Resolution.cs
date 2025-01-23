namespace BmisApi.Models
{
    public class Resolution
    {
        public int Id { get; set; }

        //Details
        public string ResolutionNumber { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateOnly Date { get; set; }

        // Crud ops
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
