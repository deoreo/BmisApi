namespace BmisApi.Models
{
    public class Narrative
    {
        public int Id { get; set; }
        public string CaseId { get; set; } = string.Empty;
        public int? BlotterId { get; set; }
        public int? VawcId { get; set; }
        public int? JusticeId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string NarrativeReport { get; set; } = string.Empty;
        public  DateOnly Date { get; set; }

        // Crud ops
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
