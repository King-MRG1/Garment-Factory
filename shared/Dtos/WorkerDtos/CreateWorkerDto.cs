using Shared.Dtos.PhoneDtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.Dtos.WorkerDtos
{
    public class CreateWorkerDto
    {
        [Required(ErrorMessage = "Worker name is required")]
        [MaxLength(100, ErrorMessage = "Worker name cannot exceed 100 characters")]
        public string Worker_Name { get; set; }

        [Range(0, 3, ErrorMessage = "Worker type must be 0 (Stitcher), 1 (Cutter), 2 (Girls), or 3 (Ironer)")]
        public int Worker_Type { get; set; }

        public List<PhoneDto> Phones { get; set; } = new List<PhoneDto>();

        [Required(ErrorMessage = "Address is required")]
        [MaxLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string Address { get; set; }
    }
}
