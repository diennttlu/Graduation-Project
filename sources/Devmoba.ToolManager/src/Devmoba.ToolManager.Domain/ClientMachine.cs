using Devmoba.ToolManager.Users;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace Devmoba.ToolManager
{
    public class ClientMachine : Entity<long>
    {
        public string IPLan { get; set; }

        public string IPPublic { get; set; }

        public DateTime LastUpdate { get; set; }

        public Guid UserId { get; set; }

        //public ClientStatus ClientStatus { get; set; }

        public virtual ICollection<Tool> Tools { get; set; }

        protected ClientMachine()
        {
            
        }

    }
}
