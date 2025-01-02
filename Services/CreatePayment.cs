// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using project3api_be.Dtos;
// using MediatR;

// namespace project3api_be.Services
// {
//     public class CreatePayment : IRequest<BaseResultWithData<PaymentLinkDtos>>
//     {
//         public string PaymentCurrency { get; set; } = string.Empty; // mã tham chiếu
//         public string PaymentRefId { get; set; } = string.Empty;
//         public decimal? RequiredAmount { get; set; }
//         public DateTime? PaymentDate { get; set; } = DateTime.Now;
//         public DateTime? ExpireDate { get; set; } = DateTime.Now.AddMinutes(15);
//         public string? PaymentLanguage { get; set; } = string.Empty;
//         public string? MerchantId { get; set; } = string.Empty;
//         public string? PaymentDestinationId { get; set; } = string.Empty;
//         public decimal? PaidAmount { get; set; } 
//     }

//     public class Payment
// }