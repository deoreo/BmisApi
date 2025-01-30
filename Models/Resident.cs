using System.ComponentModel.DataAnnotations.Schema;

namespace BmisApi.Models
{
    public class Resident
    {
        public int Id { get; set; }

        // Details
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string? Suffix { get; set; }
        
        public Sex Sex { get; set; }
        public DateOnly Birthday { get; set; }
        public string? Occupation { get; set; }
        public bool RegisteredVoter { get; set; }
        public bool IsHouseholdHead { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} " +
            $"{(MiddleName != null ? MiddleName + " " : "")}" +
            $"{LastName}" +
            $"{(Suffix != null ? " " + Suffix : "")}"
            .Trim();

        // Household connection
        public int? HouseholdId { get; set; }
        public Household? Household { get; set; }

        // Crud ops
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public int GetAge(DateOnly birthday)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - birthday.Year;


            if (today < birthday.AddYears(age))
            {
                age--;
            }

            return age;
        }
    }
    public enum Sex 
    {
        PreferNotToSay = 0,
        Male = 1,
        Female = 2 
    }

    
}
