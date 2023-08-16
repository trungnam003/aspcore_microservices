using Contracts.Configurations;
using Contracts.Services;
using Microsoft.Extensions.Logging;
using Shared.Services.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using System.Text;
using System.Threading.Tasks;
using ILogger = Serilog.ILogger;
using MimeKit;
using Infrastructure.Configurations;
using System.Runtime;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService<MailRequest>
    {
        private readonly ILogger _logger;
        private readonly EmailSMTPSettings _emailSMTPSettings;
        private readonly SmtpClient _smtpClient;

        public EmailService(ILogger logger, IOptions<EmailSMTPSettings> emailSMTPSettings)
        {
            _logger = logger;
            _emailSMTPSettings = emailSMTPSettings.Value;
            _smtpClient = new SmtpClient();
        }
        public async Task SendEmailAsync(MailRequest request, CancellationToken cancellationToken = default)
        {
            var emailMessage = GetMimeMessage(request);

            try
            {
                await _smtpClient.ConnectAsync(_emailSMTPSettings.SMTPServer, _emailSMTPSettings.Port, _emailSMTPSettings.UseSsl, cancellationToken);
                await _smtpClient.AuthenticateAsync(_emailSMTPSettings.Username, _emailSMTPSettings.Password, cancellationToken);
                await _smtpClient.SendAsync(emailMessage, cancellationToken);
                await _smtpClient.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            finally
            {
                await _smtpClient.DisconnectAsync(true, cancellationToken);
                _smtpClient.Dispose();
            }

        }

        private MimeMessage GetMimeMessage(MailRequest request)
        {
            var emailMessage = new MimeMessage
            {
                Sender = new MailboxAddress(_emailSMTPSettings.DisplayName, request.From ?? _emailSMTPSettings.From),
                Subject = request.Subject,
                Body = new BodyBuilder
                {
                    HtmlBody = request.Body
                }.ToMessageBody()
            };
            if (request.ToAddAdress != null && request.ToAddAdress.Any())
            {
                foreach (var toAddress in request.ToAddAdress)
                {
                    emailMessage.To.Add(MailboxAddress.Parse(toAddress));
                }
            }
            else
            {
                var toAddress = request.To;
                emailMessage.To.Add(MailboxAddress.Parse(toAddress));
            }
            return emailMessage;
        }
    }
}
