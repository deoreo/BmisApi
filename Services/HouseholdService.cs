﻿using BmisApi.Models.DTOs.Resident;
using BmisApi.Models;
using BmisApi.Models.DTOs.Household;
using BmisApi.Data;
using BmisApi.Repositories;

namespace BmisApi.Services
{
    public class HouseholdService : ICrudService<Household, GetHouseholdResponse, GetAllHouseholdResponse, CreateHouseholdRequest, UpdateHouseholdRequest>
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

            return SetResponse(household);
        }
        public async Task<GetHouseholdResponse> CreateAsync(CreateHouseholdRequest request)
        {
            var members = await _residentRepository.GetManyByIdAsync(request.MembersId);

            var head = await _residentRepository.GetByIdAsync(request.HeadId);

            if (head == null)
            {
                throw new Exception($"Provided head resident with id {request.HeadId} not found");
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
                Address = request.Address,
                HeadId = request.HeadId,
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
            return SetResponse(response);
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

            if (request.NewHeadId.HasValue)
            {
                var newHead = await _residentRepository.GetByIdAsync(request.NewHeadId.Value);
                if (newHead == null)
                {
                    throw new Exception($"Provided head resident with id {request.NewHeadId} not found");
                }

                household.HeadId = newHead.Id;

                if (!household.Members.Any(m => m.Id == newHead.Id))
                {
                    household.Members.Add(newHead);
                }
            }

            if (request.MembersToAdd?.Any() == true)
            {
                var membersToAdd = await _residentRepository.GetManyByIdAsync(request.MembersToAdd);
                foreach (var member in membersToAdd)
                {
                    if (!household.Members.Any(m => m.Id == member.Id))
                    {
                        household.Members.Add(member);
                    }
                }
            }

            if (request.MembersToRemove?.Any() == true)
            {
                var membersToRemove = await _residentRepository.GetManyByIdAsync(request.MembersToRemove);
                foreach (var member in membersToRemove)
                {
                    household.Members.Remove(member);
                }
            }

            household.LastUpdatedAt = DateTime.UtcNow;
            await _householdRepository.UpdateAsync(household);

            return SetResponse(household);
        }

        public async Task<GetAllHouseholdResponse> GetAllAsync()
        {
            var households = await _householdRepository.GetAllAsync();

            var householdResponse = households.Select(SetResponse).ToList();

            return new GetAllHouseholdResponse(householdResponse);
        }

        public Task<GetAllHouseholdResponse> Search(string name)
        {
            throw new NotImplementedException();
        }

        public GetHouseholdResponse SetResponse(Household household)
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
