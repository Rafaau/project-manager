using ProjectManager.Core.ProjectAggregate;
using ProjectManager.Web.ApiModels;
using AutoMapper;
using ProjectManager.Core.ProjectAggregate.Specifications;

namespace ProjectManager.Web.MappingProfiles;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<User, UserComplex>().ReverseMap();
    CreateMap<User, UserSimplified>().ReverseMap();
    CreateMap<User, UserRequest>().ReverseMap();
    CreateMap<Project2, ProjectComplex>().ReverseMap();
    CreateMap<Project2, ProjectSimplified>().ReverseMap();
    CreateMap<Project2, ProjectRequest>().ReverseMap();
    CreateMap<Message, MessageComplex>().ReverseMap();
    CreateMap<Message, MessageRequest>().ReverseMap();
    CreateMap<Assignment, AssignmentComplex>().ReverseMap();
    CreateMap<Assignment, AssignmentSimplified>().ReverseMap();
    CreateMap<Assignment, AssignmentRequest>().ReverseMap();
    CreateMap<AssignmentStage, AssignmentStageSimplified>().ReverseMap();
    CreateMap<AssignmentStage, AssignmentStageComplex>().ReverseMap();
    CreateMap<AssignmentStage, AssignmentStageRequest>().ReverseMap();
    CreateMap<ChatChannel, ChatChannelRequest>().ReverseMap();
    CreateMap<ChatChannel, ChatChannelComplex>().ReverseMap();
  }
}
