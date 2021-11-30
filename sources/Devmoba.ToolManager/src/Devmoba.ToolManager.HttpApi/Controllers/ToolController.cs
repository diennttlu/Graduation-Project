using Devmoba.ToolManager.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Devmoba.ToolManager.Controllers
{
    [RemoteService(Name = ToolManagerHttpApiModule.RemoteServiceName)]
    [Route("/api/tools")]
    public class ToolController : AbpController, IToolAppService
    {
        private readonly IToolAppService _toolAppService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ToolController(IToolAppService toolAppService,
            IHttpContextAccessor httpContextAccessor)
        {
            _toolAppService = toolAppService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ToolDto> GetAsync(long id)
        {
            return await _toolAppService.GetAsync(id);
        }

        [HttpGet]
        public async Task<PagedResultDto<ToolDto>> GetListAsync(ToolFilterDto input)
        {
            return await _toolAppService.GetListAsync(input);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task DeleteAsync(long id)
        {
            await _toolAppService.DeleteAsync(id);
        }

        [HttpPost]
        [Route("report")]
        public async Task<ToolDto> CreateOrUpdateAsync(CreateUpdateToolDto input)
        {
            input.IPPublic = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            return await _toolAppService.CreateOrUpdateAsync(input);
        }

        [RemoteService(IsEnabled = false)]
        public Task<ToolDto> UpdateStateAsync(long id, DateTime? lastUpdate, ProcessState processState, bool SentMail)
        {
            throw new NotImplementedException();
        }

        [RemoteService(IsEnabled = false)]
        public Task<ToolReportDto> GetToolReportAsync()
        {
            throw new NotImplementedException();
        }

        [HttpGet("downloadfile/{fileName}"), DisableRequestSizeLimit]
        public IActionResult DownloadFileExe(string fileName)
        {
            try
            {
                var path = Path.Combine("wwwroot/files", fileName);
                var bytes = System.IO.File.ReadAllBytes(path);
                return File(bytes, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new 
                { 
                    message = ex.Message, 
                    currentDate = DateTime.UtcNow 
                });
            }
           
        }

        [RemoteService(IsEnabled = false)]
        public Task UpdateProcessesAsync(List<ToolProcessDto> input)
        {
            throw new NotImplementedException();
        }

        [RemoteService(IsEnabled = false)]
        public Task<List<ToolProcessDto>> GetToolProcessesAsync(long clientMachineId)
        {
            throw new NotImplementedException();
        }
    }
}
