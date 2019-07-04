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
                .ConstructUsingServiceLocator();
            CreateMap<AlertCriteria, Domain.Model.AlertCriteria>();
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
