using System.Collections.Generic;
using Volo.Abp.Domain.Repositories;

namespace Devmoba.ToolManager.Repositories
{
    public interface IClientMachineRepository : IRepository<ClientMachine, long>
    {
        void BulkUpdate(List<ClientMachine> clientMachines, int batchSize);
    }
}
