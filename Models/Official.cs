namespace BmisApi.Models
{
    public class Official
    {
        public int Id { get; set; }
        public string Position { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int ResidentId { get; set; }
        public required Resident Resident { get; set; }
        public DateOnly TermStart { get; set; }
        public DateOnly TermEnd { get; set; }

        // Crud ops
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
