using CountriesApi.Domain.Interfaces;

namespace CountriesApi.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Rollback()
        {
            _context.ChangeTracker.Entries()
                .ToList()
                .ForEach(e => e.Reload());
        }
    }
}
