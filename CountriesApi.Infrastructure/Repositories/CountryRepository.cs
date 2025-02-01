using CountriesApi.Domain.Entites;
using CountriesApi.Domain.Interfaces;
using CountriesApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CountriesApi.Infrastructure.Repositories
{
    public class CountryRepository : IReposistory<Country>
    {
        private readonly AppDbContext _context;

        public CountryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddRangeAsync(IEnumerable<Country> entities)
        {
            await _context.Countries.AddRangeAsync(entities);
        }

        public async Task<List<Country>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Countries.ToListAsync(cancellationToken);
        }
    }
}
