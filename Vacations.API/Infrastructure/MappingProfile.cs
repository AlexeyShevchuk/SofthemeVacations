using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Vacations.BLL.Models;
using Vacations.DAL.Models;

namespace Vacations.API.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<User, UserDto>();
            //CreateMap<UserDto, User>();

            //CreateMap<IQueryable<User>, IQueryable<UserDto>>();
            //CreateMap<IQueryable<UserDto>, IQueryable<User>>();

            //CreateMap<UserDto, UserModel>();
            //CreateMap<UserModel, UserDto>();
        }
    }
}
