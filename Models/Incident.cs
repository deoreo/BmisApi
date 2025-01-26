namespace BmisApi.Models
{
    public class Incident
    {
        public int Id { get; set; }

        // Details
        public DateOnly Date { get; set; }
        public int ComplainantId { get; set; }
        public required Resident Complainant { get; set; }
        public string Nature { get; set; } = string.Empty;
        public string Narrative { get; set; } = string.Empty;

        // Crud ops
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
