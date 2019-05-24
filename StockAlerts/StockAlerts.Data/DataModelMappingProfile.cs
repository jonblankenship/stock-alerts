using AutoMapper;
using StockAlerts.Data.Model;

namespace StockAlerts.Data
{
    public class DataModelMappingProfile : Profile
    {
        public DataModelMappingProfile()
        {
            MapDataToDomainModel();
            MapDomainToDataModel();
        }

        private void MapDataToDomainModel()
        {
            CreateMap<AlertDefinition, Domain.Model.AlertDefinition>().ConstructUsingServiceLocator();
            CreateMap<Stock, Domain.Model.Stock>().ConstructUsingServiceLocator();
        }

        private void MapDomainToDataModel()
        {
            CreateMap<Domain.Model.Stock, Stock>()
                .ForMember(d => d.Created, opt => opt.Ignore())
                .ForMember(d => d.Modified, opt => opt.Ignore());

            CreateMap<Domain.Model.AlertDefinition, AlertDefinition>()
                .ForMember(d => d.Created, opt => opt.Ignore())
                .ForMember(d => d.Modified, opt => opt.Ignore());
        }
    }
}
