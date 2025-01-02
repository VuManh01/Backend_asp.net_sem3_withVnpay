using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project3api_be.Models;
using project3api_be.Dtos;

namespace project3api_be.Services
{
    // public interface IVnPayService
    // {
    //     string CreatePaymentUrl(HttpContext context, VnPayRequestDto model);
    //     VnPaymentResponseDto PaymentExecute(IQueryCollection collections);
    // }
    public interface IVnPayService
    {
    Task<string> CreatePaymentUrl(HttpContext context, VnPayRequestDto model);
    Task<VnPaymentResponseDto> PaymentExecute(IQueryCollection collections);
    }
}