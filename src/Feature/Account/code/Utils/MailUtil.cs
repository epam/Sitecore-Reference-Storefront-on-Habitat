namespace Sitecore.Feature.Account.Utils
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net.Mail;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Foundation.CommerceServer.Infrastructure.Constants;
    using Sitecore.Foundation.CommerceServer.Managers;
    using Sitecore.Globalization;

    /// <summary>
    /// Defines the MailUtil class
    /// </summary>
    public class MailUtil
    {
        private string _mailFrom = string.Empty;
        private string _mailTo = string.Empty;
        private string _mailBody = string.Empty;
        private string _mailSubject = string.Empty;
        private string _mailAttachmentFileName = string.Empty;

        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="bodyField">Email body</param>
        /// <param name="toEmail">Receiver email.</param>
        /// <param name="fromEmail">From email.</param>
        /// <param name="subjectParameters">The subject parameters.</param>
        /// <param name="bodyParameters">The body parameters.</param>
        /// <param name="subjectField"></param>
        /// <returns>True if the email was sent, false otherwise</returns>
        public virtual bool SendMail([NotNull] string subjectField, [NotNull] string bodyField, [NotNull] string toEmail, [NotNull] string fromEmail, [NotNull] object subjectParameters, [NotNull] object[] bodyParameters)
        {
            Assert.ArgumentNotNull(toEmail, "toEmail");
            Assert.ArgumentNotNull(fromEmail, "fromEmail");
            Assert.ArgumentNotNull(subjectParameters, "subjectParameters");
            Assert.ArgumentNotNull(bodyParameters, "bodyParameters");
            
            var subject = string.Format(CultureInfo.InvariantCulture, subjectField, subjectParameters);
            var body = string.Format(CultureInfo.InvariantCulture, bodyField, bodyParameters);

            return SendMail(toEmail, fromEmail, subject, body, string.Empty);
        }

        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="toEmail">To email.</param>
        /// <param name="fromEmail">From email.</param>
        /// <param name="subject">The mail subject.</param>
        /// <param name="body">The mail body.</param>
        /// <param name="attachmentFileName">Name of the attachment file.</param>
        /// <returns>True if the email was sent, false otherwise</returns>
        public virtual bool SendMail([NotNull] string toEmail, [NotNull] string fromEmail, [NotNull] string subject, [NotNull] string body, [NotNull] string attachmentFileName)
        {
            Assert.ArgumentNotNull(toEmail, "toEmail");
            Assert.ArgumentNotNull(fromEmail, "fromEmail");
            Assert.ArgumentNotNull(subject, "subject");
            Assert.ArgumentNotNull(body, "body");
            Assert.ArgumentNotNull(attachmentFileName, "attachmentFileName");

            _mailTo = toEmail;
            _mailFrom = fromEmail;
            _mailBody = body;
            _mailAttachmentFileName = attachmentFileName;
            _mailSubject = subject;

            return SendMail();
        }

        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <returns>True if the email was sent, false otherwise</returns>
        protected virtual bool SendMail()
        {
            var message = new MailMessage
            {
                From = new MailAddress(this._mailFrom),
                Body = _mailBody,
                Subject = _mailSubject,
                IsBodyHtml = true
            };
            message.To.Add(_mailTo);

            if (_mailAttachmentFileName != null && File.Exists(_mailAttachmentFileName))
            {
                Attachment attachment = new Attachment(_mailAttachmentFileName);
                message.Attachments.Add(attachment);
            }

            try
            {
                MainUtil.SendMail(message);
                var infoMessage = StorefrontManager.GetSystemMessage(StorefrontConstants.SystemMessages.MailSentToMessage);
                Log.Info(Translate.Text(string.Format(CultureInfo.InvariantCulture, infoMessage, message.To, message.Subject)), "SendMailFromTemplate");

                return true;
            }
            catch (Exception e)
            {
                var errorMessage = StorefrontManager.GetSystemMessage(StorefrontConstants.SystemMessages.CouldNotSendMailMessageError);
                Log.Error(Translate.Text(string.Format(CultureInfo.InvariantCulture, errorMessage, message.Subject, message.To)), e, "SendMailFromTemplate");

                return false;
            }
        }
    }
}