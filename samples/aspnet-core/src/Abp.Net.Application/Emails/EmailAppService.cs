using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.Emailing;
using Volo.Abp.Emailing.Templates;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Settings;
using Volo.Abp.TextTemplating;

namespace Abp.Net.Emails
{
    public class EmailAppService : NetAppService
    {
        protected AbpIdentityOptions AbpOptions { get; }

        private readonly IEmailSender _emailSender;
        private readonly ISettingProvider _settingProvider;
        private readonly ITemplateRenderer _templateRenderer;
        private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;
        public EmailAppService(
            IEmailSender emailSender,
            ISettingProvider settingProvider,
            ITemplateRenderer templateRenderer,
            IOptions<AbpIdentityOptions> options,
           ICurrentPrincipalAccessor currentPrincipalAccessor)
        {
            _emailSender = emailSender;
            _settingProvider = settingProvider;
            _templateRenderer = templateRenderer;
            _currentPrincipalAccessor = currentPrincipalAccessor;
            AbpOptions = options.Value;
        }

        public async Task<string> GetPassword()
        {

            var allClaims = _currentPrincipalAccessor.Principal.Claims.ToList();
            return await _settingProvider.GetOrNullAsync(EmailSettingNames.Smtp.Password);
        }

        public async Task SendAsync()
        {
            await _emailSender.SendAsync(
                "outman94.fyc@gmail.com",     // target email address
                "Email subject",         // subject
                "This is email body..."  // email body
            );
        }

        public async Task QueueAsync()
        {
            await _emailSender.QueueAsync(
                "outman94.fyc@gmail.com",     // target email address
                "Email subject",         // subject
                "This is email body..."  // email body
            );
        }
        public async Task<string> GetTemplateAsync()
        {
            return await _templateRenderer.RenderAsync(
                StandardEmailTemplates.Message,
                new
                {
                    message = "This is email body..."
                }
            );
        }

        public async Task SendMessageAsync()
        {
            var body = await _templateRenderer.RenderAsync(
                StandardEmailTemplates.Message,
                new
                {
                    message = "This is email body..."
                }
            );

            await _emailSender.SendAsync(
                "outman94.fyc@gmail.com",     // target email address
                "Email subject",         // subject
                body
            );
        }

        public async Task SendCustomMessageAsync()
        {
            var body = await _templateRenderer.RenderAsync(
                "WelcomeEmail",
                new
                {
                    message = "This is email body..."
                }
            );

            await _emailSender.SendAsync(
                "outman94.fyc@gmail.com",     // target email address
                "Email subject",         // subject
                body
            );
        }


    }

}