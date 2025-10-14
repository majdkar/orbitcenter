using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Threading;
using System.Threading.Tasks;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Shared.Wrapper;
using SchoolV01.Application.Requests.Mail;

namespace SchoolV01.Application.Features.ContactUs.Commands.AddEdit
{
    public partial class SendContactUsCommand : IRequest<Result<int>>
    {
        public string ToEmail { get; set; }              // يمكن أن تكون "a@x.com,b@y.com"
        public string CcEmail { get; set; }              // يمكن أن تكون "c@z.com,d@w.com"
        public string Subject { get; set; }
        public string Message { get; set; }
    }

    internal class SendContactUsCommandHandler : IRequestHandler<SendContactUsCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly MailSettings _mailSettings;

        public SendContactUsCommandHandler(
            IUnitOfWork<int> unitOfWork,
            IMapper mapper,
            IUploadService uploadService,
            IOptions<MailSettings> mailSettings)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _mailSettings = mailSettings.Value;
        }

        public async Task<Result<int>> Handle(SendContactUsCommand command, CancellationToken cancellationToken)
        {
            await SendEmailAsync(command, cancellationToken);
            return await Result<int>.SuccessAsync("Email sent successfully");
        }

        private async Task SendEmailAsync(SendContactUsCommand command, CancellationToken ct)
        {
            var email = new MimeMessage();

            email.Sender = MailboxAddress.Parse(_mailSettings.From);
            email.From.Add(MailboxAddress.Parse(_mailSettings.From));

            // ✅ دعم عدة مستلمين في To
            if (!string.IsNullOrWhiteSpace(command.ToEmail))
            {
                foreach (var address in command.ToEmail.Split(',', ';'))
                {
                    if (!string.IsNullOrWhiteSpace(address))
                        email.To.Add(MailboxAddress.Parse(address.Trim()));
                }
            }

            // ✅ دعم عدة مستلمين في CC
            if (!string.IsNullOrWhiteSpace(command.CcEmail))
            {
                foreach (var address in command.CcEmail.Split(',', ';'))
                {
                    if (!string.IsNullOrWhiteSpace(address))
                        email.Cc.Add(MailboxAddress.Parse(address.Trim()));
                }
            }

            email.Subject = command.Subject;

            var builder = new BodyBuilder
            {
                TextBody = "شكراً لتواصلك معنا.",
                HtmlBody = $"<div style='font-family:Tahoma;'><p>{command.Message}</p><p>تحياتنا،<br>فريق Orbit Engineering</p></div>"
            };

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            // ⚙️ الاتصال الآمن الموصى به
            await smtp.ConnectAsync(_mailSettings.Host, 587, SecureSocketOptions.StartTls, ct);

            await smtp.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password, ct);
            await smtp.SendAsync(email, ct);
            await smtp.DisconnectAsync(true, ct);
        }
    }
}