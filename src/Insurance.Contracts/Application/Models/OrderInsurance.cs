using System.Collections.Generic;

namespace Insurance.Contracts.Application.Models
{
    public class OrderInsurance
    {
        public List<int> ProductIds { get; set; } = new List<int>();
    }
}
