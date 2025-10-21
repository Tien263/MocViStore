namespace Exe_Demo.Services
{
    public interface IEmailService
    {
        Task SendOtpEmailAsync(string toEmail, string otpCode, string userName);
        Task SendWelcomeEmailAsync(string toEmail, string userName);
    }
}
