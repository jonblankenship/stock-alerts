using AutoMapper;
using StockAlerts.Resources.Model;

namespace StockAlerts.Api
{
    public class ResourceModelMappingProfile : Profile
    {
        public ResourceModelMappingProfile()
        {
            MapResourceToDomainModel();
            MapDomainToResourceModel();
        }

        private void MapResourceToDomainModel()
        {
            CreateMap<AlertDefinition, Domain.Model.AlertDefinition>()
                .ForMember(d => d.Stock, opt => opt.Ignore())
                .ForMember(d => d.AlertTriggerHistories, opt => opt.Ignore())
                .ForMember(d => d.AppUser, opt => opt.Ignore())
                .ForMember(d => d.Description, opt => opt.Ignore())
                .ConstructUsingServiceLocator();
            CreateMap<AlertCriteria, Domain.Model.AlertCriteria>()
                .ForMember(d => d.AlertDefinition, opt => opt.Ignore())
                .ForMember(d => d.ParentCriteria, opt => opt.Ignore());
                
            CreateMap<UserPreferences, Domain.Model.UserPreferences>()
                .ConstructUsingServiceLocator();
        }

        private void MapDomainToResourceModel()
        {
            CreateMap<Domain.Model.AlertDefinition, AlertDefinition>();

            CreateMap<Domain.Model.AlertCriteria, AlertCriteria>();

            CreateMap<Domain.Model.UserPreferences, UserPreferences>();

            CreateMap<Domain.Model.Stock, Stock>();
        }
    }
}
