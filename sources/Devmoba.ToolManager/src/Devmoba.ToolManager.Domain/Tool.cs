using System;
using Volo.Abp.Domain.Entities;

namespace Devmoba.ToolManager
{
    public class Tool : Entity<long>
    {
        public string Name { get; set; }

        public Guid AppId { get; set; }

        public string Version { get; set; }

        public long ClientId { get; set; }

        public string ExeFilePath { get; set; }

        public ProcessState ProcessState { get; set; }

        public int ProcessId { get; set; }

        public DateTime LastUpdate { get; set; }

        public bool SentMail { get; set; }

        public virtual ClientMachine ClientMachine { get; set; }
    }
}
