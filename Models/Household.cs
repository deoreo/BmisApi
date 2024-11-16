namespace BmisApi.Models
{
    public class Household
    {
        public int HouseholdId { get; set; }
        public string? Address { get; set; }
        public int? HeadId { get; set; }
        public ICollection<Resident>? Members { get; set; }

        // Crud ops
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
