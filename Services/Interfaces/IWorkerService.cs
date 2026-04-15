using Database.Models;
using Shared.Dtos;
using Shared.Dtos.QueryFilters;
using Shared.Dtos.ReportsDtos;
using Shared.Dtos.WorkerDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IWorkerService
    {
        public Task<ViewWorkerDto?> GetWorkerByIdAsync(int id);
        public Task<ViewWorkerDto?> CreateWorkerAsync(CreateWorkerDto createWorkerDto);
        public Task<ViewWorkerDto?> UpdateWorkerAsync(int id, UpdateWorkerDto updateWorkerDto);
        public Task<ViewWorkerDto?> DeleteWorkerAsync(int id);
        public Task<BaseWeeklyPaymentDto?> GetWeeklyPaymentAsync(CreateWeeklyPaymentDto dto,bool addToExpense);
        public Task<IEnumerable<ViewWorkerDto>> GetWorkersByFilterAsync(WorkerFilter workerFilter);  
        public Task<IEnumerable<ViewEnumDto>> GetWorkerTypesAsync();
    }
}
