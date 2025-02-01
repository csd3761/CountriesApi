using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountriesApi.Domain.Configuration
{
    public class DatabaseSettings
    {
        public const string SectionName = "DatabaseSettings";
        public string ConnectionString { get; set; }
    }
}
