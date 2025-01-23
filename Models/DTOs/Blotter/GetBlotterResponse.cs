﻿namespace BmisApi.Models.DTOs.Blotter
{
    public record GetBlotterResponse
        (int Id, DateOnly Date, string ComplainantName, string DefendantName, string Nature, Status Status, string Narrative, DateTime CreatedAt)
    {
    }
}
