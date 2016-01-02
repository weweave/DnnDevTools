﻿using System.Net;
using System.Net.Http;
using System.Web.Http;
using DotNetNuke.Entities.Host;
using DotNetNuke.Web.Api;
using weweave.DnnDevTools.Util;

namespace weweave.DnnDevTools.Api.Controller
{
    [ValidateAntiForgeryToken]
    [SuperUserAuthorize]
    public class ConfigController : ApiControllerBase
    {

        [HttpPut]
        public HttpResponseMessage Enable(bool status)
        {
            ServiceLocator.ConfigService.SetEnable(status);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPut]
        public HttpResponseMessage EnableMailCatch(bool status)
        {
            if (status && !ServiceLocator.ConfigService.GetEnable())
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "NOT_ENABLED");
            }

            ServiceLocator.ConfigService.SetEnableMailCatch(status);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPut]
        public HttpResponseMessage EnableDnnEventCatch(bool status)
        {
            if (status && !ServiceLocator.ConfigService.GetEnable())
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "NOT_ENABLED");
            }

            ServiceLocator.ConfigService.SetEnableDnnEventCatch(status);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPut]
        public HttpResponseMessage SetLogMessageLevel(string level)
        {
            var log4NetLevel = Log4NetUtil.ParseLevel(level);

            if (log4NetLevel == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);


            ServiceLocator.ConfigService.SetLogMessageLevel(log4NetLevel);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPut]
        public HttpResponseMessage SetAllowedRoles([FromUri] string[] roles)
        {
            ServiceLocator.ConfigService.SetAllowedRoles(roles);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage SendTestMail()
        {
            if (string.IsNullOrWhiteSpace(Host.SMTPServer))
                return Request.CreateResponse(HttpStatusCode.BadRequest, "SMTP_NOT_CONFIGURED");

            if (!ServiceLocator.ConfigService.GetEnableMailCatch())
                return Request.CreateResponse(HttpStatusCode.BadRequest, "MAIL_CATCH_NOT_ENABLED");

            DotNetNuke.Services.Mail.Mail.SendEmail("sender@localhost", "receiver@localhost", "Test mail from DNN Dev Tools", "This is a test mail generated by DNN Dev Tools!");
            return Request.CreateResponse(HttpStatusCode.OK);
        }

    }
}
