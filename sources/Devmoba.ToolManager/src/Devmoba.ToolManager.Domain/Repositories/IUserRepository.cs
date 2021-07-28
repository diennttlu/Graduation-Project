using Devmoba.ToolManager.Users;
using System;
using Volo.Abp.Domain.Repositories;

namespace Devmoba.ToolManager.Repositories
{
    public interface IUserRepository : IReadOnlyRepository<AppUser, Guid>
    {
    }
}
