using Database.Models;
using Shared.Dtos.AdvanceAndDeductionDtos;
using Shared.Dtos.PhoneDtos;
using Shared.Dtos.WorkerDtos;


namespace Shared.Mapping
{
    public static class WorkerMapping
    {
        public static ViewWorkerDto ToWorkerDto(this Worker worker)
        {
            return new ViewWorkerDto
            {
                Id = worker.Id,
                Worker_Name = worker.Worker_Name,
                Address = worker.Address,
                Hire_Date = worker.Hire_Date,
                Last_Update = worker.Last_Update,
                Worker_Type = worker.Worker_Type.ToString(),
                Phones = worker.Phones != null ? worker.Phones.ConvertAll(p => p.ToPhoneDto()) : new List<PhoneDto>(),
                AdvanceAndDeduction = worker.AdvanceAndDeductions != null
                ? worker.AdvanceAndDeductions.ConvertAll(ad => ad.ToAdvanceAndDeductionDto())
                : new List<ViewAdvanceAndDeductionDto>()

            };
        }
         public static Worker ToWorker(this CreateWorkerDto createWorkerDto)
        {
            return new Worker
            {
                Worker_Name = createWorkerDto.Worker_Name,
                Address = createWorkerDto.Address,
                Hire_Date = DateOnly.FromDateTime(DateTime.Now),
                Last_Update = DateOnly.FromDateTime(DateTime.Now),
                Worker_Type = (WorkerType)createWorkerDto.Worker_Type,
                Phones = createWorkerDto.Phones != null 
                ? createWorkerDto.Phones.ConvertAll(p => p.ToPhone()) 
                : new List<Phone>()
            };
        }
        public static void UpdateWorker(this Worker worker, UpdateWorkerDto updateWorkerDto)
        {
            worker.Worker_Name = updateWorkerDto.Worker_Name;
            worker.Address = updateWorkerDto.Address;
            worker.Hire_Date = updateWorkerDto.Hire_Date;
            worker.Last_Update = DateOnly.FromDateTime(DateTime.Now);
            worker.Worker_Type = (WorkerType)updateWorkerDto.Worker_Type;
        }
        
    }
}
