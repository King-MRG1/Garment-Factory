using Database.Models;
using Shared.Dtos.AdvanceAndDeductionDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Mapping
{
    public static class AdvanceAndDeductionMapping
    {
        public static ViewAdvanceAndDeductionDto ToAdvanceAndDeductionDto(this AdvanceAndDeduction advanceAndDeduction)
        {
            return new ViewAdvanceAndDeductionDto
            {
                Id = advanceAndDeduction.Id,
                Amount = advanceAndDeduction.Amount,
                Description = advanceAndDeduction.Description,
                Type = advanceAndDeduction.Type.ToString(),
                Date = advanceAndDeduction.Date,
                Worker_Name = advanceAndDeduction.Worker.Worker_Name,
                IsUsed = advanceAndDeduction.IsUsed
            };
        }
        public static AdvanceAndDeduction ToAdvanceAndDeduction(this CreateAdvanceAndDeductionDto createAdvanceAndDeductionDto)
        {
            return new AdvanceAndDeduction
            {
                Amount = createAdvanceAndDeductionDto.Amount,
                Description = createAdvanceAndDeductionDto.Description,
                Type = (AdvanceOrDeduction)createAdvanceAndDeductionDto.Type,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Worker_Id = createAdvanceAndDeductionDto.Worker_Id,
                IsUsed = false
            };
        }
        public static AdvanceAndDeduction UpdateAdvanceAndDeduction(this AdvanceAndDeduction advanceAndDeduction,
            UpdateAdvanceAndDeductionDto updateAdvanceAndDeductionDto)
        {
            advanceAndDeduction.Amount = updateAdvanceAndDeductionDto.Amount;
            advanceAndDeduction.Description = updateAdvanceAndDeductionDto.Description;
            advanceAndDeduction.Type = (AdvanceOrDeduction)updateAdvanceAndDeductionDto.Type;
            advanceAndDeduction.Date = DateOnly.FromDateTime(DateTime.Now);
            advanceAndDeduction.Worker_Id = updateAdvanceAndDeductionDto.Worker_Id;
            return advanceAndDeduction;
        }
    }
}
