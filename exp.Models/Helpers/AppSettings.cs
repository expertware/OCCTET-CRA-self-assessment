namespace exp.Models.Helpers
{
  
    public class VaultSettings
    {
        #region JWTToken
        public string JWTTokenSecret { get; set; }
        #endregion

        #region SMTP
        public string EmailFrom { get; set; }
        public string SmtpHost { get; set; }
        public string SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPass { get; set; }
        #endregion

        public string ApplicationUrl { get; set; }
        public string EmailContact { get; set; }
        #region Google recaptcha
        public required string RecaptchaSecretKey { get; set; }
        #endregion

        public required string AgentCopilotBearer { get; set; }
    }
}
