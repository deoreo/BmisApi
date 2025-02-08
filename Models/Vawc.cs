namespace BmisApi.Models
{
    public class Vawc
    {
        public int Id { get; set; }

        // Details
        public int CaseId { get; set; }
        public DateOnly Date { get; set; }
        public int ComplainantId { get; set; }
        public required Resident Complainant { get; set; }
        public int DefendantId { get; set; }
        public required Resident Defendant { get; set; }
        public string Nature { get; set; } = string.Empty;
        public VawcStatus Status { get; set; }
        public string Narrative { get; set; } = string.Empty;

        // Crud ops
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public enum VawcStatus
    {
        Settled = 0,
        Unsettled = 1
    }
}
