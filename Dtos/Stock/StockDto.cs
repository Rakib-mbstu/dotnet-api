using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Stock
{
    public class StockDto
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public decimal Purchase { get; set; }
        public string Industry { get; set; } = string.Empty;
    }
}