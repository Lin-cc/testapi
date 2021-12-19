using AutoMapper;
using lin.app.Application.Contracts.Dtos;
using lin.app.Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lin.app.Application.Profiles
{
    public class LinAppApplicationAutoMapperProfile : Profile
    {
        public LinAppApplicationAutoMapperProfile()
        {
            CreateMap<ChatUser, ChatUserDto>().ReverseMap();
        }
    }
}
