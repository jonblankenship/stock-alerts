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
            CreateMap<AlertTriggerHistory, Domain.Model.AlertTriggerHistory>();
            CreateMap<AppUser, Domain.Model.AppUser>().ConstructUsingServiceLocator();
            CreateMap<UserPreferences, Domain.Model.UserPreferences>();
            CreateMap<AlertCriteria, Domain.Model.AlertCriteria>();
        }

        private void MapDomainToDataModel()
        {
            CreateMap<Domain.Model.Stock, Stock>()
                .ForMember(d => d.Created, opt => opt.Ignore())
                .ForMember(d => d.Modified, opt => opt.Ignore());

            CreateMap<Domain.Model.AlertDefinition, AlertDefinition>()
                .ForMember(d => d.Created, opt => opt.Ignore())
                .ForMember(d => d.Modified, opt => opt.Ignore());
            
            CreateMap<Domain.Model.ApiCall, ApiCall>()
                .ForMember(d => d.Created, opt => opt.Ignore())
                .ForMember(d => d.Modified, opt => opt.Ignore());

            CreateMap<Domain.Model.AlertTriggerHistory, AlertTriggerHistory>()
                .ForMember(d => d.Created, opt => opt.Ignore())
                .ForMember(d => d.Modified, opt => opt.Ignore());

            CreateMap<Domain.Model.AppUser, AppUser>()
                .ForMember(d => d.Created, opt => opt.Ignore())
                .ForMember(d => d.Modified, opt => opt.Ignore());

            CreateMap<Domain.Model.UserPreferences, UserPreferences>()
                .ForMember(d => d.Created, opt => opt.Ignore())
                .ForMember(d => d.Modified, opt => opt.Ignore());

            CreateMap<Domain.Model.AlertCriteria, AlertCriteria>()
                .ForMember(d => d.Created, opt => opt.Ignore())
                .ForMember(d => d.Modified, opt => opt.Ignore());
        }
    }
}
