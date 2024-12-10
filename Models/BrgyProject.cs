namespace BmisApi.Models
{
    public class BrgyProject
    {
        public int Id { get; set; }

        // Details
        public string BrgyProjectName { get; set; } = string.Empty;
        public string ImplementingAgency { get; set; } = string.Empty ;
        public DateOnly StartingDate { get; set; }
        public DateTime CompletionDate { get; set;}
        public string ExpectedOutput { get; set; } = string.Empty;
        public string FundingSource { get; set; } = string.Empty;
        public string PS { get; set; } = string.Empty;
        public string MOE { get; set; } = string.Empty;
        public string CO { get; set; } = string.Empty;

        // Crud ops
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
