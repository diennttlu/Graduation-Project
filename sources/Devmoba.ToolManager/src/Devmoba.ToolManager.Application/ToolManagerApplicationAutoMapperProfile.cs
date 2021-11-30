using AutoMapper;
using Devmoba.ToolManager.ClientMachines;
using Devmoba.ToolManager.Dependencies;
using Devmoba.ToolManager.Scripts;
using Devmoba.ToolManager.Tools;

namespace Devmoba.ToolManager
{
    public class ToolManagerApplicationAutoMapperProfile : Profile
    {
        public ToolManagerApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            CreateMap<Script, ScriptDto>();
            CreateMap<Script, ScriptSelectionDto>();
            CreateMap<CreateUpdateScriptDto, Script>();

            CreateMap<ClientMachine, ClientMachineDto>();
            CreateMap<CreateUpdateClientMachineDto, ClientMachine>();

            CreateMap<Tool, ToolDto>();
            CreateMap<Tool, ToolProcessDto>();
            CreateMap<ToolProcessDto, Tool>();
            CreateMap<CreateUpdateToolDto, Tool>();

            CreateMap<Dependency, DependencyDto>();
            CreateMap<CreateUpdateDependencyDto, Dependency>();
        }
    }
}
