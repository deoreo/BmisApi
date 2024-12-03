using BmisApi.Models.DTOs.Resident;
using BmisApi.Models;
using BmisApi.Models.DTOs.Household;
using BmisApi.Data;
using BmisApi.Repositories;

namespace BmisApi.Services
{
    public class HouseholdService : ICrudService<GetHouseholdResponse, GetAllHouseholdResponse, CreateHouseholdRequest, UpdateHouseholdRequest>
    {
        private readonly ICrudRepository<Household> _householdRepository;
        private readonly ICrudRepository<Resident> _residentRepository;

        public HouseholdService(ICrudRepository<Household> householdRepository, ICrudRepository<Resident> residentRepository)
        {
            _householdRepository = householdRepository;
            _residentRepository = residentRepository;
        }
        public async Task<GetHouseholdResponse?> GetByIdAsync(int id)
        {
            var household = await _householdRepository.GetByIdAsync(id);
            if (household == null)
            {
                return null;
            }

            return SetHouseholdResponse(household);
        }
        public async Task<GetHouseholdResponse> CreateAsync(CreateHouseholdRequest request)
        {
            var members = await _residentRepository.GetManyByIdAsync(request.membersId);

            var head = await _residentRepository.GetByIdAsync(request.headId);

            if (head == null)
            {
                throw new Exception($"Provided head resident with id {request.headId} not found");
            }

            if (!members.Any(m => m.Id == head.Id))
            {
                members.Add(head);
            }

            if (!members.Any())
            {
                throw new ArgumentException("No valid members found.");
            }

            var household = new Household()
            {
                Address = request.address,
                HeadId = request.headId,
                Members = members
            };

            household.GetHead().IsHouseholdHead = true;

            foreach (var member in household.Members)
            {
                member.HouseholdId = household.Id;

                if (member.Id != household.HeadId)
                {
                    member.IsHouseholdHead = false;
                }
            }

            var response = await _householdRepository.CreateAsync(household);
            return SetHouseholdResponse(response);
        }

        public async Task DeleteAsync(int id)
        {
            await _householdRepository.DeleteAsync(id);
        }

        public async Task<GetHouseholdResponse?> UpdateAsync(UpdateHouseholdRequest request, int id)
        {
            var household = await _householdRepository.GetByIdAsync(id);
            if (household == null)
            {
                return null;
            }

            if (request.newHeadId.HasValue)
            {
                var newHead = await _residentRepository.GetByIdAsync(request.newHeadId.Value);
                if (newHead == null)
                {
                    throw new Exception($"Provided head resident with id {request.newHeadId} not found");
                }

                household.HeadId = newHead.Id;

                if (!household.Members.Any(m => m.Id == newHead.Id))
                {
                    household.Members.Add(newHead);
                }
            }

            if (request.membersToAdd?.Any() == true)
            {
                var membersToAdd = await _residentRepository.GetManyByIdAsync(request.membersToAdd);
                foreach (var member in membersToAdd)
                {
                    if (!household.Members.Any(m => m.Id == member.Id))
                    {
                        household.Members.Add(member);
                    }
                }
            }

            if (request.membersToRemove?.Any() == true)
            {
                var membersToRemove = await _residentRepository.GetManyByIdAsync(request.membersToRemove);
                foreach (var member in membersToRemove)
                {
                    household.Members.Remove(member);
                }
            }

            household.LastUpdatedAt = DateTime.UtcNow;
            await _householdRepository.UpdateAsync(household);

            return SetHouseholdResponse(household);
        }

        public async Task<GetAllHouseholdResponse> GetAllAsync()
        {
            var households = await _householdRepository.GetAllAsync();

            var householdResponse = households.Select(SetHouseholdResponse).ToList();

            return new GetAllHouseholdResponse(householdResponse);
        }

        public Task<GetAllHouseholdResponse> Search(string name)
        {
            throw new NotImplementedException();
        }

        public GetHouseholdResponse SetHouseholdResponse(Household household)
        {
            var head = household.GetHead();

            var response = new GetHouseholdResponse
                (
                household.Id,
                household.Address,
                household.Members.Count,
                head.FullName,
                head.GetAge(head.Birthday),
                head.Sex,
                head.Birthday,
                head.Occupation,
                head.CreatedAt
                );

            return response;
        }
    }
}
