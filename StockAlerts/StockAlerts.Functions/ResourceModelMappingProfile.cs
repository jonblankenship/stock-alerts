using AutoMapper;
using StockAlerts.Resources.Model;

namespace StockAlerts.Functions
{
    public class ResourceModelMappingProfile : Profile
    {
        public ResourceModelMappingProfile()
        {
            MapResourceToDomainModel();
            MapDomainToDataModel();
        }

        private void MapResourceToDomainModel()
        {
            CreateMap<AlertDefinition, Domain.Model.AlertDefinition>()
                .ConstructUsingServiceLocator();
            CreateMap<AlertCriteria, Domain.Model.AlertCriteria>();
            CreateMap<UserPreferences, Domain.Model.UserPreferences>()
                .ConstructUsingServiceLocator();
        }

        private void MapDomainToDataModel()
        {
            CreateMap<Domain.Model.AlertDefinition, AlertDefinition>();

            CreateMap<Domain.Model.AlertCriteria, AlertCriteria>();

            CreateMap<Domain.Model.UserPreferences, UserPreferences>();
        }
    }
}
