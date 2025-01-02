using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project3api_be.Helpers;
using project3api_be.Models;
using project3api_be.Services;
using project3api_be.Dtos;
using project3api_be.Services;


namespace project3api_be.Services
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _config;
        public VnPayService(IConfiguration config) 
        {
            _config = config;
        }
        public async Task<string> CreatePaymentUrl(HttpContext context, VnPayRequestDto model)
        {
            // Validate input
            if (model == null)
            throw new ArgumentNullException(nameof(model));
            var tick = DateTime.Now.Ticks.ToString();
            var vnpay = new VnPayLibrary();
// Log các thông tin quan trọng
Console.WriteLine($"Creating payment URL for OrderId: {model.OrderId}");
Console.WriteLine($"Amount: {model.Amount}");
            vnpay.AddRequestData("vnp_Version", _config["VnPay:Version"]);
            vnpay.AddRequestData("vnp_Command", _config["VnPay:Command"]);
            vnpay.AddRequestData("vnp_TmnCode", _config["VnPay:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", (model.Amount * 100).ToString()); //Số tiền thanh toán. Số tiền không 
            //mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND 
            //  (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY 
            //là: 10000000
            vnpay.AddRequestData("vnp_CreateDate", model.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _config["VnPay:CurrCode"]);
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", _config["VnPay:Locale"]);
            vnpay.AddRequestData("vnp_OrderInfo", $"{model.Description} - {model.FullName}");
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", _config["VnPay:PaymentBackReturnUrl"]);
            vnpay.AddRequestData("vnp_TxnRef", tick); // Mã tham chiếu của giao dịch tại hệ 
            //thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được 
            //trùng lặp trong ngày
            var paymentUrl = vnpay.CreateRequestUrl(_config["VnPay:BaseUrl"], _config["VnPay:HashSecret"]);
    Console.WriteLine($"Generated Payment URL: {paymentUrl}");

            return paymentUrl;
        }

        public async Task<VnPaymentResponseDto> PaymentExecute(IQueryCollection collections)
        {
            try 
            {
                var vnpay = new VnPayLibrary();
                        // Log tất cả các parameters nhận được
                    Console.WriteLine("Received VNPay Parameters:");
                foreach (var (key, value) in collections)
                {   
                    Console.WriteLine($"{key}: {value}");
                    if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(key, value.ToString());
                    }
                }
                // lấy secure hash từ Vnpay response
                // Kiểm tra xem có nhận được SecureHash không
                var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value.ToString();
                if (string.IsNullOrEmpty(vnp_SecureHash))
                {
                    Console.WriteLine("No SecureHash received from VNPay");
                    return new VnPaymentResponseDto
                    {
                    Success = false,
                    PaymentMethod = "VnPay",
                    Message = "No SecureHash received"
                };
            }
        Console.WriteLine($"Received SecureHash: {vnp_SecureHash}");
                // Log HashSecret được sử dụng
                var hashSecret = _config["VnPay:HashSecret"];
                    Console.WriteLine($"Used HashSecret: {hashSecret}");
                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _config["VnPay:HashSecret"]);
                    Console.WriteLine($"Signature validation result: {checkSignature}");
                if (!checkSignature)
           return new VnPaymentResponseDto
           {
               Success = false,
               PaymentMethod = "VnPay",
               Message = "Invalid signature"
           };

                
                var orderInfo = vnpay.GetResponseData("vnp_OrderInfo");
                var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                var vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                var vnp_TransactionNo = vnpay.GetResponseData("vnp_TransactionNo");
                var vnp_TxnRef = vnpay.GetResponseData("vnp_TxnRef");

                // if (!checkSignature)
                //     return new VnPaymentResponseDto
                //     {
                //         Success = false,
                //         PaymentMethod = "VnPay",
                //         Message = "Invalid signature"
                //     };

                return new VnPaymentResponseDto
                {
                    Success = vnp_ResponseCode == "00",
                    PaymentMethod = "VnPay",
                    OrderDescription = orderInfo,
                    OrderId = vnp_TxnRef,
                    TransactionId = vnp_TransactionNo,
                    Token = vnp_SecureHash,
                    VnPayResponseCode = vnp_ResponseCode,
                    Message = GetResponseMessage(vnp_ResponseCode)
                };
            }
            catch (Exception ex)
            {   
                Console.WriteLine($"Error in PaymentExecute: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return new VnPaymentResponseDto
                {
                    Success = false,
                    PaymentMethod = "VnPay",
                    Message = ex.Message
                };
            }
        }

        private string GetResponseMessage(string responseCode)
        {
            switch (responseCode)
            {
                case "00": return "Giao dịch thành công";
                case "07": return "Trừ tiền thành công. Giao dịch bị nghi ngờ (liên quan tới lừa đảo, giao dịch bất thường).";
                case "09": return "Giao dịch không thành công do: Thẻ/Tài khoản của khách hàng chưa đăng ký dịch vụ InternetBanking tại ngân hàng.";
                case "10": return "Giao dịch không thành công do: Khách hàng xác thực thông tin thẻ/tài khoản không đúng quá 3 lần";
                // Thêm các mã khác nếu cần
                default: return "Giao dịch không thành công";
            }
        }
    }
}