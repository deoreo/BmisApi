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
            var head =  Members.FirstOrDefault(r => r.Id == HeadId);

            if (head == null)
            {
                throw new Exception($"Head not found in the household. HeadId: {HeadId}");
            }

            return head;
        }
    }
}
