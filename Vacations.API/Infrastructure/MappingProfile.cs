using AutoMapper;
using Vacations.BLL.Models;
using Vacations.DAL.Models;

namespace Vacations.API.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            AllowNullCollections = true;

            CreateMap<Employee, ProfileDto>()
                .ForMember(p => p.TeamName, o => o.MapFrom(e => e.Team.Name))
                .ForMember(p => p.TeamLeadName, o=> o.MapFrom(e => e.Team.TeamLead.Name))
                .ForMember(p => p.TeamLeadSurname, o => o.MapFrom(e => e.Team.TeamLead.Surname))
                .ForMember(p => p.JobTitle, o => o.MapFrom(e => e.JobTitle.Name))
                .ForMember(p => p.EmployeeStatus, o => o.MapFrom(e => e.EmployeeStatus.Name));

            CreateMap<Vacation, VacationDto>()
                .ForMember(vdto => vdto.EmployeeName, o => o.MapFrom(v => v.Employee.Name))
                .ForMember(vdto => vdto.EmployeeSurname, o => o.MapFrom(v => v.Employee.Surname))
                .ForMember(vdto => vdto.EmployeeBalance, o => o.MapFrom(v => v.Employee.Balance))
                .ForMember(vdto => vdto.VacationStatusName, o => o.MapFrom(v => v.VacationStatus.Name))
                .ForMember(vdto => vdto.TeamName, o => o.MapFrom(v => v.Employee.Team.Name))
                .ForMember(vdto => vdto.VacationTypeName, o => o.MapFrom(v => v.VacationTypes.Name));

            CreateMap<VacationDto, Vacation>()
                .ForMember(vdto => vdto.Transaction, opt => opt.Ignore())
                .ForMember(vdto => vdto.VacationStatus, opt => opt.Ignore())
                .ForMember(vdto => vdto.VacationTypes, opt => opt.Ignore())
                .ForMember(vdto => vdto.Employee, opt => opt.Ignore());

            CreateMap<Team, TeamDto>()
                .ForMember(tdto => tdto.TeamLeadName, o => o.MapFrom(t => t.TeamLead.Name))
                .ForMember(tdto => tdto.TeamLeadSurname, o => o.MapFrom(t => t.TeamLead.Surname))
                .ForMember(tdto => tdto.MembersNumber, o => o.MapFrom(t => t.Employee.Count));

            CreateMap<EmployeeDto, Employee>()
            .ForMember(e => e.EmployeeStatus, opt => opt.Ignore())
            .ForMember(e => e.JobTitle, opt => opt.Ignore())
            .ForMember(e => e.Team, opt => opt.Ignore())
            .ForMember(e => e.User, opt => opt.Ignore())
            .ForMember(e => e.TeamNavigation, opt => opt.Ignore())
            .ForMember(e => e.Transaction, opt => opt.Ignore())
            .ForMember(e => e.Vacation, opt => opt.Ignore());         
        }
    }
}
