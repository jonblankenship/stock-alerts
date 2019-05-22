using AutoMapper;
using StockAlerts.Data.Model;

namespace StockAlerts.Data
{
    public class DataModelMappingProfile : Profile
    {
        public DataModelMappingProfile()
        {
            CreateMap<AlertDefinition, Domain.Model.AlertDefinition>();
            CreateMap<Stock, Domain.Model.Stock>();
        }
    }
}
