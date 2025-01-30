namespace BmisApi.Models
{
    public class Household
    {
        public int Id { get; set; }
        public required string Address { get; set; }
        public int HeadId { get; set; }
        public required ICollection<Resident> Members { get; set; }

        // Crud ops
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Resident GetHead()
        {
            var head = Members.FirstOrDefault(r => r.Id == HeadId) ?? new Resident // Return a dummy resident if head is null
            {
                FirstName = "N/A",
                LastName = "N/A",
                Sex = 0,
                Birthday = DateOnly.MinValue,
                RegisteredVoter = false
            };

            return head;
        }
    }
}
