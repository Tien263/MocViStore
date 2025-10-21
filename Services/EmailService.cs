using System.Net;
using System.Net.Mail;

namespace Exe_Demo.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendOtpEmailAsync(string toEmail, string otpCode, string userName)
        {
            var subject = "Mã OTP Xác Thực Tài Khoản - Mộc Vị Store";
            var body = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #4f6a4c 0%, #6b8f67 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
                        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
                        .otp-box {{ background: white; border: 3px dashed #4f6a4c; padding: 20px; text-align: center; margin: 20px 0; border-radius: 10px; }}
                        .otp-code {{ font-size: 36px; font-weight: bold; color: #4f6a4c; letter-spacing: 8px; }}
                        .footer {{ text-align: center; margin-top: 20px; color: #666; font-size: 12px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>🌿 Mộc Vị Store</h1>
                            <p>Xác Thực Tài Khoản</p>
                        </div>
                        <div class='content'>
                            <h2>Xin chào {userName}!</h2>
                            <p>Cảm ơn bạn đã đăng ký tài khoản tại <strong>Mộc Vị Store</strong>.</p>
                            <p>Để hoàn tất đăng ký, vui lòng nhập mã OTP bên dưới:</p>
                            
                            <div class='otp-box'>
                                <p style='margin: 0; color: #666;'>Mã OTP của bạn:</p>
                                <div class='otp-code'>{otpCode}</div>
                                <p style='margin: 10px 0 0 0; color: #999; font-size: 14px;'>Mã có hiệu lực trong 5 phút</p>
                            </div>
                            
                            <p><strong>Lưu ý:</strong></p>
                            <ul>
                                <li>Không chia sẻ mã OTP với bất kỳ ai</li>
                                <li>Mã OTP chỉ sử dụng một lần</li>
                                <li>Nếu bạn không yêu cầu đăng ký, vui lòng bỏ qua email này</li>
                            </ul>
                            
                            <p>Trân trọng,<br><strong>Đội ngũ Mộc Vị Store</strong></p>
                        </div>
                        <div class='footer'>
                            <p>© 2025 Mộc Vị Store - Hoa Quả Sấy Mộc Châu</p>
                            <p>Email này được gửi tự động, vui lòng không trả lời.</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

            await SendEmailAsync(toEmail, subject, body);
        }

        public async Task SendWelcomeEmailAsync(string toEmail, string userName)
        {
            var subject = "Chào Mừng Đến Với Mộc Vị Store! 🌿";
            var body = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #4f6a4c 0%, #6b8f67 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
                        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>🌿 Chào Mừng Đến Với Mộc Vị Store!</h1>
                        </div>
                        <div class='content'>
                            <h2>Xin chào {userName}!</h2>
                            <p>Tài khoản của bạn đã được kích hoạt thành công! 🎉</p>
                            <p>Bạn có thể bắt đầu khám phá và mua sắm các sản phẩm hoa quả sấy chất lượng cao từ Mộc Châu.</p>
                            <p>Chúc bạn có trải nghiệm mua sắm tuyệt vời!</p>
                            <p>Trân trọng,<br><strong>Đội ngũ Mộc Vị Store</strong></p>
                        </div>
                    </div>
                </body>
                </html>
            ";

            await SendEmailAsync(toEmail, subject, body);
        }

        private async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var smtpServer = _configuration["EmailSettings:SmtpServer"] ?? "smtp.gmail.com";
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
                var senderEmail = _configuration["EmailSettings:SenderEmail"] ?? "your-email@gmail.com";
                var senderPassword = _configuration["EmailSettings:SenderPassword"] ?? "your-app-password";
                var senderName = _configuration["EmailSettings:SenderName"] ?? "Mộc Vị Store";

                using var client = new SmtpClient(smtpServer, smtpPort)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(senderEmail, senderPassword)
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail, senderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Log error (trong production nên log vào file hoặc database)
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
        }
    }
}
