﻿using BmisApi.Models;
using BmisApi.Models.DTOs.BrgyProject;
using BmisApi.Models.DTOs.Resident;
using BmisApi.Repositories;

namespace BmisApi.Services
{
    public class BrgyProjectService : ICrudService<BrgyProject,GetBrgyProjectResponse, GetAllBrgyProjectResponse, CreateBrgyProjectRequest, UpdateBrgyProjectRequest>
    {
        private readonly ICrudRepository<BrgyProject> _repository;

        public BrgyProjectService (ICrudRepository<BrgyProject> repository)
        {
            _repository = repository;
        }

        public async Task<GetBrgyProjectResponse?> GetByIdAsync(int id)
        {
            var brgyProject = await _repository.GetByIdAsync(id);
            if (brgyProject == null)
            {
                throw new KeyNotFoundException($"Project with ID {id} not found");
            }

            return SetResponse(brgyProject);
        }
        public async Task<GetBrgyProjectResponse> CreateAsync(CreateBrgyProjectRequest request)
        {
            var brgyProject = new BrgyProject()
            {
                ReferenceCode = request.ReferenceCode.Trim(),
                ProjectDescription = request.ProjectDescription,
                ImplementingAgency = request.ImplementingAgency.Trim(),
                StartingDate = request.StartingDate,
                CompletionDate = request.CompletionDate,
                ExpectedOutput = request.ExpectedOutput,
                FundingSource = request.FundingSource.Trim(),
                PS = request.PS ?? 0m,
                MOE = request.MOE ?? 0m,
                CO = request.CO ?? 0m
            };

            brgyProject = await _repository.CreateAsync(brgyProject);

            return SetResponse(brgyProject);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<GetBrgyProjectResponse?> UpdateAsync(UpdateBrgyProjectRequest request, int id)
        {
            var brgyProject = await _repository.GetByIdAsync(id);
            if (brgyProject == null)
            {
                throw new KeyNotFoundException($"Project with ID {id} not found");
            }

            brgyProject.ReferenceCode = request.ReferenceCode.Trim();
            brgyProject.ProjectDescription = request.ProjectDescription;
            brgyProject.ImplementingAgency = request.ImplementingAgency.Trim();
            brgyProject.StartingDate = request.StartingDate;
            brgyProject.CompletionDate = request.CompletionDate;
            brgyProject.ExpectedOutput = request.ExpectedOutput;
            brgyProject.FundingSource = request.FundingSource.Trim();
            brgyProject.PS = request.PS ?? 0m;
            brgyProject.MOE = request.MOE ?? 0m;
            brgyProject.CO = request.CO ?? 0m;
            brgyProject.LastUpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(brgyProject);

            return SetResponse(brgyProject);
        }

        public async Task<GetAllBrgyProjectResponse> GetAllAsync()
        {
            var brgyProjects = await _repository.GetAllAsync();

            var responses = brgyProjects.Select(SetResponse).ToList();

            return new GetAllBrgyProjectResponse(responses);
        }

        public Task<GetAllBrgyProjectResponse> Search(string name)
        {
            throw new NotImplementedException();
        }

        public GetBrgyProjectResponse SetResponse(BrgyProject entity)
        {
            var response = new GetBrgyProjectResponse
                (
                entity.Id,
                entity.ReferenceCode,
                entity.ProjectDescription,
                entity.ImplementingAgency,
                entity.StartingDate,
                entity.CompletionDate,
                entity.ExpectedOutput,
                entity.FundingSource,
                entity.PS,
                entity.MOE,
                entity.CO,
                entity.GetTotal(),
                entity.CreatedAt
                );

            return response;
        }
    }
}
