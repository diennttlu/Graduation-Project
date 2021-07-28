using Devmoba.ToolManager.EntityFrameworkCore;
using EFCore.BulkExtensions;
using System.Collections.Generic;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Devmoba.ToolManager.Repositories
{
    public class ClientMachineRepository : EfCoreRepository<ToolManagerDbContext, ClientMachine, long>, IClientMachineRepository
    {
        private static readonly List<string> BulkUpdateExcludeProperties = new List<string>()
        {
            nameof(ClientMachine.UserId)

        };

        public ClientMachineRepository(IDbContextProvider<ToolManagerDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public void BulkUpdate(List<ClientMachine> clientMachines, int batchSize)
        {
            DbContext.BulkUpdate(clientMachines, new BulkConfig() { BatchSize = batchSize, PropertiesToExclude = BulkUpdateExcludeProperties });
        }
    }
}
