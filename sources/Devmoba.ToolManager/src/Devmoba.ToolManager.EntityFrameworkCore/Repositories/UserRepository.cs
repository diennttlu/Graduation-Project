using Devmoba.ToolManager.EntityFrameworkCore;
using Devmoba.ToolManager.Users;
using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Devmoba.ToolManager.Repositories
{
    public class UserRepository : EfCoreRepository<ToolManagerDbContext, AppUser, Guid>, IUserRepository
    {
        public UserRepository(IDbContextProvider<ToolManagerDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {

        }
    }
}
