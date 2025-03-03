namespace BmisApi.Models
{
    public class Incident
    {
        public int Id { get; set; }

        // Details
        public string CaseId { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public List<ComplainantInfo> Complainants { get; set; } = new();
        public string Nature { get; set; } = string.Empty;
        public string NarrativeReport { get; set; } = string.Empty;
        public string? PicturePath { get; set; }

        // Crud ops
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public class ComplainantInfo
    {
        public string Name { get; set; } = string.Empty;
        public string? ContactInfo { get; set; }
    }
}
