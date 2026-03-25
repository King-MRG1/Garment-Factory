using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.Dtos.ReportsDtos
{
    public class CreateWeeklyPaymentDto : IValidatableObject
    {
        [Required]
        public int Worker_Id { get; set; }

        [Required]
        public int workerType { get; set; }

        public List<WorkItemDto> WorkItems { get; set; } = new List<WorkItemDto>();

        [Range(0.01, double.MaxValue, ErrorMessage = "Manual salary must be greater than zero")]
        public decimal? ManualSalary { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (workerType == 3)
            {
                if (ManualSalary == null)
                {
                    yield return new ValidationResult(
                        "ManualSalary is required for Girls workers",
                        [nameof(ManualSalary)]);
                }
            }
            else
            {
                if (WorkItems == null || WorkItems.Count == 0)
                {
                    yield return new ValidationResult(
                        "WorkItems must have at least one item for Stitcher, Cutter, and Ironer workers",
                        [nameof(WorkItems)]);
                }
            }
        }
    }

    public class WorkItemDto
    {
        [Required]
        public int Model_Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }
}
