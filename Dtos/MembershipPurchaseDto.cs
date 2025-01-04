using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project3api_be.Dtos
{
   public class MembershipPurchaseDto
    {
    public int MembershipServiceId { get; set; }
    public decimal Price { get; set; }
    public string FullName { get; set; }
    public int AccountId { get; set; }  // Thêm trường này

    }
}