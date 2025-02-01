using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountriesApi.Domain.Interfaces
{
    public interface IReposistory<T> where T : class
    {
        Task AddRangeAsync(IEnumerable<T> entites);
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
