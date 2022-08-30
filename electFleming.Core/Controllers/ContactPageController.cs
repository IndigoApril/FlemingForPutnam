using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


using System.Threading.Tasks;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Mail;
using Umbraco.Cms.Core.Models.Email;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

using Umbraco.Cms.Core.Models;
using electFleming.Core.ViewModels;

using Umbraco.Cms.Web.Common.Filters;

namespace electFleming.Core.Controllers
{
    public class ContactPageController : SurfaceController
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ContactPageController> _logger;
        private readonly IOptions<GlobalSettings> _globalSettings;

      
        public ContactPageController(
            IUmbracoContextAccessor umbracoContextAccessor,
            IUmbracoDatabaseFactory databaseFactory,
            ServiceContext services,
            AppCaches appCaches,
            IProfilingLogger profilingLogger,
            IPublishedUrlProvider publishedUrlProvider,
            IEmailSender emailSender,
            IOptions<GlobalSettings> globalSettings)
            : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _emailSender = emailSender;
            _globalSettings = globalSettings;
        }

        //public ContactSurfaceController(
        //    IUmbracoContextAccessor umbracoContextAccessor,
        //    IUmbracoDatabaseFactory databaseFactory,
        //    ServiceContext services,
        //    AppCaches appCaches,
        //    IProfilingLogger profilingLogger,
        //    IPublishedUrlProvider publishedUrlProvider,
        //    IEmailSender emailSender,
        //    ILogger<ContactSurfaceController> logger,
        //    IOptions<GlobalSettings> globalSettings)
        //    : base(umbracoContextAccessor, databaseFactory,
        //          services, appCaches, profilingLogger,
        //          publishedUrlProvider)
        //{
        //    _emailSender = emailSender;
        //    _logger = logger;
        //    _globalSettings = globalSettings.Value;
        //}

        //public async Task<IActionResult> Render()
        //{
        //    var contact = new ContactViewModel();
        //    return PartialView("contactFormView", contact);
        //}

        [HttpPost]
        [ValidateUmbracoFormRouteString]
        public async Task<IActionResult> Submit(ContactPageViewModel model)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            // caller user passes in 
            //var contentService = Services.ContentService;
            //var parentId = new Guid("Guid of Content Model Type");

            //var contentItem = contentService.Create(model.Name, parentId, "documentTypeAlias");

            //contentService.SaveAndPublish(contentItem);

            TempData["Success"] = await SendEmail(model);

            return RedirectToCurrentUmbracoPage();
        }

        private async Task<bool> SendEmail(ContactPageViewModel model)
        {
            try
            {
                var fromAddress = _globalSettings.Value.Smtp.From;

                var subject = string.Format("FlemingForPutnam.com Inquiry from: {0} - {1}", model.Name, model.Email);

                //var emailBody = GetEmailMessage()
                EmailMessage message = GetEmailNotification(model);
                    
                //await _emailSender.SendAsync(message, emailType: "Contact");

                _logger.LogInformation("Contact Form Submitted Successfully");

                return true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error When Submitting Contact Form");
                return false;
            }
        }

        private EmailMessage GetEmailNotification(ContactPageViewModel model)
        {
            var fromAddress = _globalSettings.Value.Smtp.From;
            var subject = "";
            EmailMessage message = new EmailMessage(fromAddress, fromAddress, subject, model.Message, false);
            return message;
        }
    }
}