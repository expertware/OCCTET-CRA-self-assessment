using exp.Models.Helpers;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using MimeKit.Utils;
using System.Xml;

namespace exp.Services.Email
{
    public interface IEmailService
    {
        void Send(string to, string subject, string html, string? from = null);
        public void SendMailRegister(bool forApprove, string contactEmail, string vat, string companyName, string? appUrl = null, string? accessCode = null);
        public void SendEmailAccessCode(string contactEmail, string appUrl, string accessCode, string bodyText, string vat, string companyName);
        public void SendTemplate(string to, string subject, string html, string? from = null);
        public void SendRequestForCodeEmail(string contactEmail, string userEmailRequest, string organizationName, string organizationVat, string appUrl);
        public void NotifyAdmin(string contactEmail, string vat, string companyName, string appUrl);
        public void RejectedRequest(string contactEmail, string? appUrl, string vat, string companyName);
    }

    public class EmailService : IEmailService
    {
        private readonly VaultSettings _vaultSettings;
        private readonly IWebHostEnvironment _env;

        public EmailService(VaultSettings vaultSettings, IWebHostEnvironment env)
        {
            _vaultSettings = vaultSettings;
            _env = env;
        }
        public void NotifyAdmin(string contactEmail, string vat, string companyName, string appUrl)
        {
            string basePath = _env.WebRootPath ?? _env.ContentRootPath;

            string emailPath = System.IO.Path.Combine(basePath, "EmailTemplates/CompanyRequestForAdmin.tem");

            if (!File.Exists(emailPath))
            {
                throw new Exception("Template not found!");
            }

            var htmlText = File.ReadAllText(emailPath);
            htmlText = htmlText.Replace("[Link]", appUrl + "/requests" );
            htmlText = htmlText.Replace("[BaseLink]", appUrl);
            htmlText = htmlText.Replace("[vat]", vat);
            htmlText = htmlText.Replace("[companyName]", companyName);

            SendTemplate(contactEmail, "New request", htmlText);
        }
        public void SendMailRegister(bool forApprove, string contactEmail, string vat, string companyName, string? appUrl, string? accessCode = null)
        {
            string basePath = _env.WebRootPath ?? _env.ContentRootPath;


            if (!forApprove)
            {
                string emailPath = System.IO.Path.Combine(basePath, "EmailTemplates/ConfirmEmail.tem");

                if (!File.Exists(emailPath))
                {
                    throw new Exception("Template not found!");
                }

                var htmlText = File.ReadAllText(emailPath);
                htmlText = htmlText.Replace("[Link]", appUrl + "/confirm-email?code=" + accessCode);
                htmlText = htmlText.Replace("[BaseLink]", appUrl);
                htmlText = htmlText.Replace("[vat]", vat);
                htmlText = htmlText.Replace("[companyName]", companyName);
                SendTemplate(contactEmail, "Company Registration", htmlText);
            }
            else
            {
                string emailPath = System.IO.Path.Combine(basePath, "EmailTemplates/CompanyRequest.tem");

                if (!File.Exists(emailPath))
                {
                    throw new Exception("Template not found!");
                }


                var htmlText = File.ReadAllText(emailPath);
                htmlText = htmlText.Replace("[BaseLink]", appUrl);
                htmlText = htmlText.Replace("[vat]", vat);
                htmlText = htmlText.Replace("[companyName]", companyName);
                SendTemplate(contactEmail, "Company Registration", htmlText);
            }

        }
        public void SendEmailAccessCode(string contactEmail, string? appUrl, string? accessCode, string bodyText, string vat, string companyName)
        {
            string basePath = _env.WebRootPath ?? _env.ContentRootPath;

            string emailPath = System.IO.Path.Combine(basePath, "EmailTemplates/AccessCodeEmail.tem");

            if (!File.Exists(emailPath))
            {
                throw new Exception("Template not found!");
            }

            var htmlText = File.ReadAllText(emailPath);
            htmlText = htmlText.Replace("[AccessCode]", accessCode);
            htmlText = htmlText.Replace("[BaseLink]", appUrl);
            htmlText = htmlText.Replace("[BodyText]", bodyText);
            htmlText = htmlText.Replace("[vat]", vat);
            htmlText = htmlText.Replace("[companyName]", companyName);

            SendTemplate(contactEmail, "Access Code", htmlText);

        }
        public void RejectedRequest(string contactEmail, string? appUrl, string vat, string companyName)
        {
            string basePath = _env.WebRootPath ?? _env.ContentRootPath;

            string emailPath = System.IO.Path.Combine(basePath, "EmailTemplates/CompanyRequestRejected.tem");

            if (!File.Exists(emailPath))
            {
                throw new Exception("Template not found!");
            }

            var htmlText = File.ReadAllText(emailPath);
            htmlText = htmlText.Replace("[BaseLink]", appUrl);
            htmlText = htmlText.Replace("[vat]", vat);
            htmlText = htmlText.Replace("[companyName]", companyName);

            SendTemplate(contactEmail, "Occtet rejected request", htmlText);

        }

        public void SendRequestForCodeEmail(string contactEmail, string userEmailRequest, string organizationName, string organisationVat, string appUrl)
        {
            string basePath = _env.WebRootPath ?? _env.ContentRootPath;

            string emailPath = System.IO.Path.Combine(basePath, "EmailTemplates/AccessRequest.tem");

            if (!File.Exists(emailPath))
            {
                throw new Exception("Template not found!");
            }

            var htmlText = File.ReadAllText(emailPath);
            htmlText = htmlText.Replace("[Email]", userEmailRequest);
            htmlText = htmlText.Replace("[OrganisationName]", organizationName);
            htmlText = htmlText.Replace("[BaseLink]", appUrl);
            htmlText = htmlText.Replace("[vat]", organisationVat);
            htmlText = htmlText.Replace("[companyName]", organizationName);

            htmlText = htmlText.Replace("[Link]", appUrl + $"/accept-request?organisationVat={organisationVat}&userEmailRequest={userEmailRequest}");

            SendTemplate(contactEmail, "Access Request ", htmlText);

        }
        public void Send(string to, string subject, string html, string? from = null)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? _vaultSettings.EmailFrom));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };
            // send email
            using var smtp = new SmtpClient();
            if (_vaultSettings.SmtpPort == null)
            {
                throw new Exception("Smtp port is not configurate");
            }
            smtp.Connect(_vaultSettings.SmtpHost, Convert.ToInt32(_vaultSettings.SmtpPort), SecureSocketOptions.StartTls);
            smtp.Authenticate(_vaultSettings.SmtpUser, _vaultSettings.SmtpPass);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
        public void SendTemplate(string to, string subject, string html, string? from = null)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? _vaultSettings.EmailFrom));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };
            var bodyBuilder = new BodyBuilder();

            var htmlTemp = AttachImg(html, bodyBuilder, "EmailTemplates/Images/logo-occtet.png", "Logo");
            htmlTemp = AttachImg(htmlTemp, bodyBuilder, "EmailTemplates/Images/top-image.png", "TopImage");

            var multipartAlternative = new MultipartAlternative
                {
                new TextPart(TextFormat.Html) { Text = htmlTemp },
                };

            bodyBuilder.Attachments.Add(multipartAlternative);

            //bodyBuilder.HtmlBody = htmlText;

            email.Body = bodyBuilder.ToMessageBody();

            // send email
            using var smtp = new SmtpClient();
            if (_vaultSettings.SmtpPort == null)
            {
                throw new Exception("Smtp port is not configurate");
            }
            smtp.Connect(_vaultSettings.SmtpHost, Convert.ToInt32(_vaultSettings.SmtpPort), SecureSocketOptions.StartTls);
            smtp.Authenticate(_vaultSettings.SmtpUser, _vaultSettings.SmtpPass);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
        #region Helpers
        public string AttachImg(string html, BodyBuilder bodyBuilder, string url, string idToReplace)
        {

            // Dictionary to store ContentId for each image
            var contentIds = new Dictionary<string, string>();
            // Function to add image as an inline attachment
            string AddImageAsInlineAttachment(string imagePath)
            {
                var attachment = bodyBuilder.Attachments.Add(imagePath);
                var contentId = MimeUtils.GenerateMessageId();
                attachment.ContentId = contentId;
                contentIds.Add(Path.GetFileName(imagePath), contentId);
                return contentId;
            }

            string basePath = _env.WebRootPath ?? _env.ContentRootPath;

            string logo = System.IO.Path.Combine(basePath, url);


            var logoContentId = AddImageAsInlineAttachment(logo);

            // Replace placeholders in the HTML template with the ContentIds
            var replacedHtmlTemplate = html.Replace(idToReplace, logoContentId);
            //.Replace("NewsletterBgId", newsletterBgId);

            return replacedHtmlTemplate;
        }
        #endregion
    }
}
