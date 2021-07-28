using System;
using System.ComponentModel.DataAnnotations;

namespace Devmoba.ToolManager.Tools
{
    public class CreateUpdateToolDto
    {
        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        public Guid AppId { get; set; }

        public string Version { get; set; }

        [Required]
        public string IPLan { get; set; }

        public string IPPublic { get; set; }

        public long ClientId { get; set; }

        [Required]
        public string ExeFilePath { get; set; }

        [Required]
        public int ProcessId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;

        public bool SentMail { get; set; }
    }
}
