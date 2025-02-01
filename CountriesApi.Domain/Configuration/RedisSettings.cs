using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountriesApi.Domain.Configuration
{
    public class RedisSettings
    {
        public const string SectionName = "RedisSettings";
        public string ConnectionString { get; set; }
        public int CacheDurationMinutes { get; set; }
    }
}
