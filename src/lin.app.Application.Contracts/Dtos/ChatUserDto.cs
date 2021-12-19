using lin.app.Domain.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace lin.app.Application.Contracts.Dtos
{
    public class ChatUserDto:AuditedEntityDto<Guid>
    {
        public string? Name { get; set; }

        public int Type { get; set; }
    }
}
