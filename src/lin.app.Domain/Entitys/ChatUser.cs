using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace lin.app.Domain.Entitys
{
    public class ChatUser : AuditedEntity<Guid>
    {
        [MaxLength(16)]
        public string? Name { get; set; }

        public int Type { get; set; }= 0;
    }
}