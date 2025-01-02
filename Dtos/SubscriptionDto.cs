using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project3api_be.Dtos
{
   public class SubscriptionDto
    {
        public int MembershipServiceId { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}