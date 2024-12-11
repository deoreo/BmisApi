namespace BmisApi.Models
{
    public class BrgyProject
    {
        public int Id { get; set; }

        // Details
        public string ReferenceCode { get; set; } = string.Empty;
        public string ImplementingAgency { get; set; } = string.Empty ;
        public DateOnly StartingDate { get; set; }
        public DateOnly CompletionDate { get; set;}
        public string ExpectedOutput { get; set; } = string.Empty;
        public string FundingSource { get; set; } = string.Empty;
        public decimal PS { get; set; }
        public decimal MOE { get; set; }
        public decimal CO { get; set; }

        // Crud ops
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public decimal GetTotal()
        {
            return PS + MOE + CO;
        }
    }
}
