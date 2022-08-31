using FlemingForPutnam.Core.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Mail;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;
using Umbraco.Cms.Core.Models.Email;
using Umbraco.Cms.Web.Common.Filters;
using Umbraco.Extensions;
using FlemingForPutnam.Core.ViewModels;
using System.Text;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Web.Common;

namespace electFleming.Controllers.Surface.ignore
{
    public class ContactFormController : SurfaceController
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ContactFormController> _logger;
        private readonly IOptions<GlobalSettings> _globalSettings;
        private Umbraco.Cms.Core.Services.ServiceContext _services;
        private IUmbracoHelperAccessor _umbracoHelperAccessor;

        public ContactFormController(
         IUmbracoContextAccessor umbracoContextAccessor,
         IUmbracoDatabaseFactory databaseFactory,
         ServiceContext services,
         AppCaches appCaches,
         IProfilingLogger profilingLogger,
         IPublishedUrlProvider publishedUrlProvider,
         ILogger<ContactFormController> logger,
         IEmailSender emailSender,
         IUmbracoHelperAccessor umbracoHelperAccessor,
         IOptions<GlobalSettings> globalSettings)
                  : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _emailSender = emailSender;
            _globalSettings = globalSettings;
            _logger = logger;
            _services = services;
            _umbracoHelperAccessor = umbracoHelperAccessor;
        }

     

        [HttpPost]
        [ValidateUmbracoFormRouteString]
        public async Task<IActionResult> SubmitContactReq(ContactFormViewModel model)
        {
         
            try
            {
                var subject = "FlemingForPutnam.com Contact";

                bool status = await SendEmail(model,subject);
                TempData["Success"] = status;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error When Submitting {model.Name} contact request Form");
            }
         
            return RedirectToCurrentUmbracoPage();
        }
        [HttpPost]
        [ValidateUmbracoFormRouteString]
        public async Task<IActionResult> SubmitVolunteerReq(ContactFormViewModel model)
        {

            try
            {
                var subject = "FlemingForPutnam.com Volunteer";

                bool status = await SendEmail(model, subject);
                TempData["Success"] = status;
            }
            catch (Exception ex2)
            {
                _logger.LogError(ex2, $"Error When Submitting {model.Name} volunteer request Form");
            }

            return RedirectToCurrentUmbracoPage();
        }

        private async Task<bool> SendEmail(ContactFormViewModel model, string subject)
        {
            try
            {
             
                EmailMessage message = GetEmailNotification(model,subject);

                await _emailSender.SendAsync(message, emailType: "Contact");

                _logger.LogInformation($"{subject} Form Submitted Successfully");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error When Submitting {subject} Form");
                return false;
            }
        }

     
        private EmailMessage GetEmailNotification(ContactFormViewModel model, string subject)
        {
            var fromAddress = _globalSettings.Value.Smtp.From;
            var contactList = "flemingesq@msn.com,marshawaldman2@gmail.com";
            var email_subject = string.Format("{0} Request from: {1} - {2}", subject, model.Name, model.Email);
            var email_doc = BuildEmailDocument(model,subject);
            UmbracoHelper hlpr;
            var haveHelper = _umbracoHelperAccessor.TryGetUmbracoHelper(out hlpr);
            if (haveHelper)
            {
                contactList = hlpr.GetDictionaryValue("Site.ContactList");
            }
            string[] sendTo = contactList.Replace(" ", "").Split(',');

            string[] ccTo = new string[] { "support@dalyweb.com" };
            string[] bccTo = null, replyTo = null;
            bool isHtmlBody = true;

            EmailMessage message = new EmailMessage(fromAddress,
                sendTo, ccTo, bccTo, replyTo, email_subject, email_doc, isHtmlBody, null);
            return message;
        }

        private string BuildEmailDocument(ContactFormViewModel model,string subject)
        {
            StringBuilder bldr = new StringBuilder();
            //bldr.Append("<!doctype html><html>");
            //bldr.Append("<head>");
            //bldr.Append("<title>Fleming for Putnam Contact Request Report<title>");
            //bldr.Append("<style> body {color: #07074E;}</style>");
            //bldr.Append("</head>>");
            //bldr.Append("<body>");
            bldr.Append("<style>");
            bldr.Append("dd {color: #07074E;} dt { color: #97DAf8; }");
            bldr.Append("h1 {color: #97DAf8;} ");
            bldr.Append("</style>");
            bldr.Append("<div>");            
            bldr.Append($"<h1>{subject} Request</h1><hr/>");
            bldr.Append("<dl>");
            bldr.Append($"<dt>Name:</dt><dd>{model.Name}</dd>");
            bldr.Append($"<dt>Email:</dt><dd>{model.Email}</dd>");
            bldr.Append($"<dt>Phone Number:</dt><dd>{model.PhoneNumber}</dd>");
            bldr.Append($"<dt>Message:</dt><dd>{model.Message}</dd>");
            bldr.Append("</dl>");
            bldr.Append("<br/>");
            bldr.Append($"<img src=\"https://flemingforputnam.com/lib/img/FlemingForPutnamSignThumbnail.png\" alt=\"Fleming for Putnam Logo\"/>");
            bldr.Append("</div>");
            //bldr.Append("</body><html>");

            return bldr.ToString();
        }

    }
}
