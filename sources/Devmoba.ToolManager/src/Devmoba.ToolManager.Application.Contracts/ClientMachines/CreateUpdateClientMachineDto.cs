using System;
using System.ComponentModel.DataAnnotations;

namespace Devmoba.ToolManager.ClientMachines
{
    public class CreateUpdateClientMachineDto
    {
        [Required]
        [StringLength(15)]
        public string IPLan { get; set; }

        [Required]
        [StringLength(15)]
        public string IPPublic { get; set; }

        [Required]
        public ClientStatus ClientStatus { get; set; }

        [Required]
        public DateTime LastUpdate { get; set; }

        [Required]
        public Guid UserId { get; set; }
    }
}
