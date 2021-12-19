using lin.app.Application.Contracts.Dtos;
using lin.app.Application.Contracts.Services;
using lin.app.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace lin.app.Application.ServicesImp
{
    public class ChatUserService : CrudAppService<ChatUser, ChatUserDto, Guid, PagedAndSortedResultRequestDto, ChatUserDto>, IChatUserApplicationService
    {
        public ChatUserService(IRepository<ChatUser, Guid> repository) : base(repository)
        {
        }
    }
}
