namespace BmisApi.Models
{
    public class Blotter
    {
        public int BlotterId { get; set; }

        // Details
        public DateOnly Date {  get; set; }
        public int ComplainantId { get; set; }
        public required Resident Complainant { get; set; }
        public int DefendantId { get; set; }
        public required Resident Defendant { get; set; }
        public string Nature { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        // Crud ops
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

    }
}
