﻿namespace BmisApi.Models.DTOs.Resident
{
    public record GetResidentResponse
        (int Id, string FullName, int Age, string Sex, DateOnly Birthday,
        string? Occupation, bool RegisteredVoter, int? HouseholdId, string? Address, DateTime CreatedAt)
    {
    }
}
