namespace BmisApi.Models
{
    public class Resident
    {
        public int ResidentId { get; set; }

        // Details
        public string FullName { get; set; } = string.Empty;
        public Sex Sex { get; set; }
        public DateOnly Birthday { get; set; }
        public string? Occupation { get; set; }
        public bool RegisteredVoter { get; set; }
        public bool HouseholdHead { get; set; }

        // Household connection
        public int? HouseholdId { get; set; }
        public Household? Household { get; set; }

        // Crud ops
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public Resident
        (string fullName, Sex sex, DateOnly birthday,
        string? occupation, bool registeredVoter)
        {
            FullName = fullName;
            Sex = sex;
            Birthday = birthday;
            Occupation = occupation;
            RegisteredVoter = registeredVoter;
        }
    }
    public enum Sex 
    {
        PreferNotToSay = 0,
        Male = 1,
        Female = 2 
    }

    
}
