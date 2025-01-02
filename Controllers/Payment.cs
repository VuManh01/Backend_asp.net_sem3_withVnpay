using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project3api_be.Data;
using project3api_be.Helpers;
using project3api_be.Models;
using project3api_be.Services;
using project3api_be.Dtos;



namespace project3api_be.Controllers
{ 
    
    // [Route("api/[controller]")]
    // [ApiController]
    // public class PaymentController : ControllerBase
    // {
    //     private readonly IVnPayService _vnPayService;

    //     public PaymentController(IVnPayService vnPayService)
    //     {
    //         _vnPayService = vnPayService;
    //     }

    //     [HttpPost]
    //     public IActionResult Checkout(string payment = "COD")
    //     {   
    //         var vnPayModel = new Dtos.VnPayRequestDto
    //         {
    //             Amount = Cart.Sum(p => p.ThanhTien),
    //             CreatedDate = DateTime.Now,
    //             Description = "Thanh toán hóa đơn",
    //             OrderId = new Random().Next(1000, 100000).ToString()
    //         };
    //         return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPayModel));
    //     }

    //     public IActionResult PaymentSuccess()
    //     {
    //         return Ok();
    //     }
        
    //     public IActionResult PaymentFail()
    //     {
    //         return BadRequest("Thanh toán thất bại");
    //     }

    //     public IActionResult PaymentCallback() 
    //     {
    //         var response = _vnPayService.PaymentExecute(Request.Query);

    //         if(response == null || response.VnPayResponseCode != "00")
    //         {   
    //             TempData["Message"] = "Thanh toán thất bại VnPay: {response.VnPayResponseCode}";
    //             return PaymentFail();
    //         }   

    //         //Lưu đơn hàng vào database => xem vid trước


    //         // xử lý thanh toán thành công
    //         TempData["Message"] = "Thanh toán VnPay thành công";
    //         return PaymentSuccess();
    //     }

    // }

    [ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IVnPayService _vnPayService;
    public PaymentController(IVnPayService vnPayService)
    {
        _vnPayService = vnPayService;
    }

    [HttpPost("create-payment")]
    public async Task<IActionResult> CreatePayment([FromBody] VnPayRequestDto request)
    {
        try
        {   
           // Test data mẫu
           var testRequest = new VnPayRequestDto
           {
               OrderId = DateTime.Now.Ticks.ToString(), // Tạo mã đơn hàng unique
                FullName = "Test User",
                Description = "Test Payment",
                Amount = 10000, // 10,000 VND
                CreatedDate = DateTime.Now
            };
            var paymentUrl = await _vnPayService.CreatePaymentUrl(HttpContext, testRequest);
            return Ok(new { paymentUrl });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("payment-callback")]
    public async Task<IActionResult> PaymentCallback()
    {
          try
       {
           var response = await _vnPayService.PaymentExecute(Request.Query);
           // Log response để debug
           Console.WriteLine($"VNPay Response: {System.Text.Json.JsonSerializer.Serialize(response)}");
           return Ok(response);
       }
       catch (Exception ex)
       {
           Console.WriteLine($"Payment Callback Error: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}