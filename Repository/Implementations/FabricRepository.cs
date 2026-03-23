using Database.Data;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;

namespace Repository.Implementations
{
    public class FabricRepository : GenericRepository<Fabric>, IFabricRepository
    {
        public FabricRepository(AppDbContext context) : base(context)
        {
            
        }

        public async Task<IEnumerable<Fabric>> GetAllFabricsWithTradersAsync()
        {
           var fabrics = await _context.Fabrics.Include(f => f.Trader).ToListAsync();
           return fabrics;
        }

        public async Task<IEnumerable<Fabric>> GetFabricsByDateRangeAsync(DateOnly startDate, DateOnly endDate)
        {
            var fabrics = await _context.Fabrics.Where(f => f.DateAdded >= startDate && f.DateAdded <= endDate).ToListAsync();
            return fabrics;
        }

        public async Task<IEnumerable<Fabric>> GetFabricsByNameAsync(string name)
        {
            var fabrics = await _context.Fabrics.Where(f => f.Fabric_Name.Contains(name)).ToListAsync();

            return fabrics;
        }

        public async Task<IEnumerable<Fabric>> GetFabricsByTraderIdAsync(int traderId)
        {
            var fabrics = await _context.Fabrics.Where(f => f.Trader_Id == traderId).ToListAsync(); 
            return fabrics;
        }

        public async Task<Fabric> GetFabricWithTraderByIdAsync(int id)
        {
            var fabric = await _context.Fabrics.Include(f => f.Trader).FirstOrDefaultAsync(f => f.Id == id);

            return fabric;
        }
    }
}
