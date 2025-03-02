namespace BmisApi.Models
{
    public class Incident
    {
        public int Id { get; set; }

        // Details
        public string CaseId { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public int ComplainantId { get; set; }
        public required Resident Complainant { get; set; }
        public string Nature { get; set; } = string.Empty;
        public required ICollection<Narrative> NarrativeReports { get; set; }
        public string? PicturePath { get; set; }

        // Crud ops
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
