using Devmoba.ToolManager.ClientMachines;
using Devmoba.ToolManager.Tools;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;
using RazorLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Identity;
using Volo.Abp.Threading;

namespace Devmoba.ToolManager.BackgroundWorkers.Workers
{
    internal class ErrorWatchWorker : AsyncPeriodicBackgroundWorkerBase
    {
        private readonly IConfiguration _cfg;
        private readonly int _emailReportInterval;
        private readonly string _username;
        private readonly string _password;

        private readonly IClientMachineAppService _clientMachineAppService;
        private readonly IToolAppService _toolAppService;
        private readonly IHostEnvironment _hostEnvironment;

        public ErrorWatchWorker(AbpTimer timer,
            IServiceScopeFactory serviceScopeFactory,
            IHostEnvironment hostEnvironment,
            IConfiguration cfg,
            IClientMachineAppService clientMachineAppService,
            IToolAppService toolAppService)
            : base(timer, serviceScopeFactory)
        {
            _hostEnvironment = hostEnvironment;
            _cfg = cfg;
            Timer.Period = _cfg.GetValue<int>(CommonContants.ErrorWatchWorkerInterval);
            _emailReportInterval = _cfg.GetValue<int>(CommonContants.EmailReportInterval);
            _username = _cfg.GetValue<string>(CommonContants.EmailReportUsername);
            _password = _cfg.GetValue<string>(CommonContants.EmailReportPassword);
            _clientMachineAppService = clientMachineAppService;
            _toolAppService = toolAppService;
        }

        protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            Logger.LogInformation("Start worker: Do something...");

            //if (IsSendMailNow(_emailReportInterval))
            //{
            //    var reports = await _clientMachineAppService.GetClientMachineReportEmailsAsync();
            //    if (reports.Count > 0)
            //    {
            //        var identityUserRepository = workerContext.ServiceProvider.GetRequiredService<IIdentityUserRepository>();
            //        var users = await identityUserRepository.GetListByNormalizedRoleNameAsync(CommonContants.RoleNameAdmin);
            //        var toAddress = users.Select(x => MailboxAddress.Parse(x.Email)).ToList();
            //        var reportContent = await GetReportContentAsync(reports);
            //        var subject = $"THÔNG BÁO TOOL NGỪNG HOẠT ĐỘNG";
            //        SendMail(subject, reportContent, toAddress);
            //        await UpdateSendMail(reports);
            //    }
            //}

            await Task.FromResult(1);
            Logger.LogInformation("Finish worker: Something done...");
        }

        private async Task<string> GetReportContentAsync(List<ClientMachineReportEmailDto> reports)
        {
            var engine = new RazorLightEngineBuilder()
               .UseFileSystemProject(_hostEnvironment.ContentRootPath)
               .UseMemoryCachingProvider()
               .Build();
            var content = await engine.CompileRenderAsync("Template/Report.cshtml", reports);
            return content;
        }

        private bool IsSendMailNow(int interval)
        {
            var totalMinutes = (DateTime.UtcNow.Hour) * 60 + (DateTime.UtcNow.Minute);
            return totalMinutes % interval == 0;
        }

        private void SendMail(string subject, string content, List<MailboxAddress> toAddress)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_username));
            email.To.AddRange(toAddress);
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = content                                  
            };

            using (var smtp = new SmtpClient())
            {
                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
                smtp.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
                smtp.AuthenticationMechanisms.Remove("XOAUTH2");
                smtp.Authenticate(_username, _password);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }   

        private async Task UpdateSendMail(List<ClientMachineReportEmailDto> reports)
        {
            foreach (var rp in reports)
            {
                foreach (var tool in rp.ToolInactives)
                {
                    await _toolAppService.UpdateStateAsync(tool.Id, null, ProcessState.NA, true);
                }
            } 
        }
    }
}
