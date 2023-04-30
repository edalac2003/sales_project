using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Shared.DTOs
{
    public class paginationDTO
    {
        public int Id { get; set; }
        public int Page { get; set; } = 1;
        public int RecordsNumber { get; set; } = 10;

    }
}
