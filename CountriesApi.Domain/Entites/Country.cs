using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountriesApi.Domain.Entites
{
    public class Country
    {
        public int Id { get; set; }
        public string? CommonName { get; set; }
        public string? Capital { get; set; }
        public List<string> Borders { get; set; } = new();
    }
}
