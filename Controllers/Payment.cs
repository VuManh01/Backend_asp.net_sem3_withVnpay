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
    private readonly ApplicationDbContext _context;

    public PaymentController(IVnPayService vnPayService,  ApplicationDbContext context)
    {
        _vnPayService = vnPayService;
        _context = context;

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

    // [HttpGet("payment-callback")]
    // public async Task<IActionResult> PaymentCallback()
    // {
    //       try
    //    {
    //        var response = await _vnPayService.PaymentExecute(Request.Query);
    //        // Log response để debug
    //        Console.WriteLine($"VNPay Response: {System.Text.Json.JsonSerializer.Serialize(response)}");
    //        return Ok(response);
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Payment Callback Error: {ex.Message}");
    //             return BadRequest(new { message = ex.Message });
    //         }
    //     }
    

    [HttpPost("purchase-membership")]
    public async Task<IActionResult> PurchaseMembership([FromBody] MembershipPurchaseDto request)
    {   
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // 1. Tạo order_membership
            var orderMembership = new OrderMembership
            {
                MembershipServiceId = request.MembershipServiceId,
                Price = request.Price,
                Status = "pending",
                OrderStatus = "pending",
                CreatedAt = DateTime.Now
            };
            _context.OrderMembership.Add(orderMembership);
            await _context.SaveChangesAsync();

            // 2. Tạo payment_member và liên kết với order_membership
            var paymentMember = new PaymentMember
            {   
                OrderMembershipId = orderMembership.OrderMembershipId, // Sử dụng order_membership_id vừa tạo
                PaymentType = "Monthly",
                CreatedAt = DateTime.Now
            };
            _context.PaymentMembers.Add(paymentMember);
            await _context.SaveChangesAsync();

            // 3. Tạo VNPay payment URL
            var paymentRequest = new VnPayRequestDto
            {
                OrderId = orderMembership.OrderMembershipId.ToString(),
                Amount = (int)(request.Price * 100),
                Description = "Membership Payment",
                FullName = request.FullName,
                CreatedDate = DateTime.Now
            };

            var paymentUrl = await _vnPayService.CreatePaymentUrl(HttpContext, paymentRequest);
            await transaction.CommitAsync();
            return Ok(new { paymentUrl });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Error in PurchaseMembership: {ex.Message}");
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

        // Tìm OrderMembership liên quan đến OrderId từ response
        var orderMembershipId = int.Parse(response.OrderId); // Chuyển đổi sang int
        var orderMembership = await _context.OrderMembership
            .FirstOrDefaultAsync(om => om.OrderMembershipId == orderMembershipId);

        if (orderMembership != null)
        {
            // Nếu thanh toán thành công, cập nhật trạng thái Order thành 'complete'
            if (response.Success)
            {
               

                orderMembership.OrderStatus = "completed"; // Hoàn thành trong OrderMembership
                orderMembership.UpdatedAt = DateTime.Now;
            }
            else
            {
                

                orderMembership.OrderStatus = "error"; // Thất bại trong OrderMembership
                orderMembership.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
        }

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
