using System.Collections.Generic;
using AutoMapper;
using StockAlerts.Resources.Converters;
using StockAlerts.Resources.Model;

namespace StockAlerts.Resources
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
                //.ForMember(d => d.AlertCriterias, opt => opt.ConvertUsing(new AlertCriteriaValueConverter(), nameof(AlertDefinition.RootCriteria)))
                .ConstructUsingServiceLocator();
            CreateMap<AlertCriteria, Domain.Model.AlertCriteria>();
        }

        private void MapDomainToDataModel()
        {
            CreateMap<Domain.Model.AlertDefinition, AlertDefinition>();

            CreateMap<Domain.Model.AlertCriteria, AlertCriteria>();
        }
    }
}
