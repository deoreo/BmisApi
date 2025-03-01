namespace BmisApi.Models
{
    public class Blotter
    {
        public int Id { get; set; }

        // Details
        public int CaseId { get; set; }
        public DateOnly Date {  get; set; }
        public int ComplainantId { get; set; }
        public required Resident Complainant { get; set; }
        public int DefendantId { get; set; }
        public required Resident Defendant { get; set; }
        public string Nature { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public required ICollection<Narrative> NarrativeReports { get; set; }

        // Crud ops
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

    }
}
