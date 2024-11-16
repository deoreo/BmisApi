using BmisApi.Models.DTOs;
using BmisApi.Models;

namespace BmisApi.Services
{
    public class ResidentService
    {
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

        public GetResidentResponse SetResidentResponse(Resident resident)
        {
            var response = new GetResidentResponse
                (
                resident.ResidentId,
                resident.FullName,
                GetAge(resident.Birthday),
                resident.Sex,
                resident.Birthday,
                resident.Occupation,
                resident.RegisteredVoter,
                resident.HouseholdId,
                resident.Household?.Address,
                resident.CreatedAt
                );

            return response;
        }
    }
}
