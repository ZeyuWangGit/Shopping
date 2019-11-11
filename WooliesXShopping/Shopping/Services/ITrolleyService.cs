using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shopping.Common;
using Shopping.Models;
using Shopping.Models.Trolley;

namespace Shopping.Services
{
    public interface ITrolleyService
    {
        Task<decimal> CalculateTrolleyTotalViaApi(Trolley trolley);
        decimal CalculateTrolley(Trolley trolley);
    }
}
