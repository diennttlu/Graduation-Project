using AutoMapper;
using Devmoba.ToolManager.Scripts;

namespace Devmoba.ToolManager.Web
{
    public class ToolManagerWebAutoMapperProfile : Profile
    {
        public ToolManagerWebAutoMapperProfile()
        {
            //Define your AutoMapper configuration here for the Web project.
            CreateMap<ScriptDto, CreateUpdateScriptDto>();
        }
    }
}
