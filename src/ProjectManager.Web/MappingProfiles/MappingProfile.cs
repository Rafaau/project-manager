using ProjectManager.Core.ProjectAggregate;
using ProjectManager.Web.ApiModels;
using AutoMapper;

namespace ProjectManager.Web.MappingProfiles;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<User, UserComplex>().ReverseMap();
    CreateMap<User, UserSimplified>().ReverseMap();
    CreateMap<User, UserRequest>().ReverseMap();
    CreateMap<UserSimplified, UserComplex>().ReverseMap();
    CreateMap<Project2, ProjectComplex>().ReverseMap();
    CreateMap<Project2, ProjectSimplified>().ReverseMap();
    CreateMap<Project2, ProjectRequest>().ReverseMap();
    CreateMap<ChatMessage, ChatMessageComplex>().ReverseMap();
    CreateMap<ChatMessage, ChatMessageRequest>().ReverseMap();
    CreateMap<Assignment, AssignmentComplex>().ReverseMap();
    CreateMap<Assignment, AssignmentSimplified>().ReverseMap();
    CreateMap<Assignment, AssignmentRequest>().ReverseMap();
    CreateMap<AssignmentStage, AssignmentStageSimplified>().ReverseMap();
    CreateMap<AssignmentStage, AssignmentStageComplex>().ReverseMap();
    CreateMap<AssignmentStage, AssignmentStageRequest>().ReverseMap();
    CreateMap<ChatChannel, ChatChannelRequest>().ReverseMap();
    CreateMap<ChatChannel, ChatChannelComplex>().ReverseMap();
    CreateMap<ChatChannel, ChatChannelSimplified>().ReverseMap();
    CreateMap<Appointment, AppointmentRequest>().ReverseMap();
    CreateMap<Appointment, AppointmentComplex>().ReverseMap();
    CreateMap<Notification, NotificationRequest>().ReverseMap();
    CreateMap<Notification, NotificationComplex>().ReverseMap();
    CreateMap<PrivateMessage, PrivateMessageRequest>().ReverseMap();
    CreateMap<PrivateMessage, PrivateMessageComplex>().ReverseMap();
    CreateMap<PrivateMessage, PrivateMessageSimplified>().ReverseMap();
    CreateMap<InvitationLink, InvitationLinkRequest>().ReverseMap();
    CreateMap<InvitationLink, InvitationLinkComplex>().ReverseMap();
  }
}
