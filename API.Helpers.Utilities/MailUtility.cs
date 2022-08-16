using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace API.Helpers.Utilities;
public interface IMailUtility
{
    void SendMail(MailRequest request);
    Task SendMailAsync(MailRequest request);
}

public class MailUtility : IMailUtility
{
    private readonly IConfiguration _configuration;
    private readonly IOptions<MailSettings> _mailSettings;

    public MailUtility(
        IConfiguration configuration,
        IOptions<MailSettings> mailSettings)
    {
        _configuration = configuration;
        _mailSettings = mailSettings;
    }

    public void SendMail(MailRequest request)
    {
        var smtpServer = new SmtpClient
        {
            Host = _mailSettings.Value.Host,
            Port = _mailSettings.Value.Port,
            EnableSsl = _mailSettings.Value.EnableSsl,
            Credentials = new NetworkCredential(_mailSettings.Value.Mail, _mailSettings.Value.Password)
        };

        var mail = new MailMessage();
        mail.From = new MailAddress(_mailSettings.Value.Mail, _mailSettings.Value.DisplayName);
        mail.To.Add(request.ToEmail);
        mail.Subject = request.Subject;
        mail.Body = request.Body;
        mail.IsBodyHtml = request.IsBodyHtml;

        if (request.Attachments != null)
        {
            foreach (var file in request.Attachments)
            {
                if (file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        var att = new Attachment(new MemoryStream(fileBytes), file.FileName);
                        mail.Attachments.Add(att);
                    }
                }
            }
        }

        smtpServer.Send(mail);
        mail.Dispose();
        smtpServer.Dispose();
    }

    public async Task SendMailAsync(MailRequest request)
    {
        var smtpServer = new SmtpClient
        {
            Host = _mailSettings.Value.Host,
            Port = _mailSettings.Value.Port,
            EnableSsl = _mailSettings.Value.EnableSsl,
            Credentials = new NetworkCredential(_mailSettings.Value.Mail, _mailSettings.Value.Password)
        };

        var mail = new MailMessage();
        mail.From = new MailAddress(_mailSettings.Value.Mail, _mailSettings.Value.DisplayName);
        mail.To.Add(request.ToEmail);
        mail.Subject = request.Subject;
        mail.Body = request.Body;
        mail.IsBodyHtml = request.IsBodyHtml;

        if (request.Attachments != null)
        {
            foreach (var file in request.Attachments)
            {
                if (file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        var att = new Attachment(new MemoryStream(fileBytes), file.FileName);
                        mail.Attachments.Add(att);
                    }
                }
            }
        }

        await smtpServer.SendMailAsync(mail);
        mail.Dispose();
        smtpServer.Dispose();
    }
}

public class MailRequest
{
    public string ToEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsBodyHtml { get; set; }
    public List<IFormFile> Attachments { get; set; }
}

public class MailSettings
{
    public string Mail { get; set; }
    public string DisplayName { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public bool EnableSsl { get; set; }
}