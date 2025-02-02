using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountriesApi.Application.Common.Interfaces
{
    public interface ICacheableQuery
    {
        string CacheKey { get; }
    }
}
