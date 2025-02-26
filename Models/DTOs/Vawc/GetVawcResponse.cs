﻿namespace BmisApi.Models.DTOs.Blotter
{
    public record GetVawcResponse
        (int Id, int CaseId, DateOnly Date, string ComplainantName, string DefendantName, string Nature, VawcStatus Status, string Narrative, DateTime CreatedAt)
    {
    }
}
