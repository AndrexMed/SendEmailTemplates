using Microsoft.AspNetCore.Mvc;
using SendEmailTemplates.Services;

namespace SendEmailTemplates.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController(IEmailService emailService) : ControllerBase
    {
        private readonly IEmailService _emailService = emailService;

        [HttpPost]
        public async Task<IActionResult> SendEmail(string to, string subject, IFormFile htmlTemplate)
        {
            await _emailService.SendEmailAsync(to, subject, htmlTemplate);
            return Ok();
        }
    }
}