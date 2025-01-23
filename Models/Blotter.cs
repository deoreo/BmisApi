namespace BmisApi.Models
{
    public class Blotter
    {
        public int Id { get; set; }

        // Details
        public DateOnly Date {  get; set; }
        public int ComplainantId { get; set; }
        public required Resident Complainant { get; set; }
        public int DefendantId { get; set; }
        public required Resident Defendant { get; set; }
        public string Nature { get; set; } = string.Empty;
        public Status Status { get; set; }
        public string Narrative { get; set; } = string.Empty;

        // Crud ops
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

    }

    public enum Status
    {
        FirstConfrontation = 0,
        SecondConfrontation = 1,
        ThirdConfrontation = 2,
        Settled = 3,
        Justice = 4
    }
}
