using AutoMapper;
using AutoMapper.EquivalencyExpression;
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
            CreateMap<Stock, Domain.Model.Stock>()
                .ForMember(d => d.OpenPrice, opt => opt.Ignore())
                .ConstructUsingServiceLocator();
            CreateMap<AlertTriggerHistory, Domain.Model.AlertTriggerHistory>();
            CreateMap<AppUser, Domain.Model.AppUser>().ConstructUsingServiceLocator();
            CreateMap<UserPreferences, Domain.Model.UserPreferences>().ConstructUsingServiceLocator();
            CreateMap<AlertCriteria, Domain.Model.AlertCriteria>();
            CreateMap<RefreshToken, Domain.Model.RefreshToken>();
        }

        private void MapDomainToDataModel()
        {
            CreateMap<Domain.Model.Stock, Stock>()
                .ForMember(d => d.Created, opt => opt.Ignore())
                .ForMember(d => d.Modified, opt => opt.Ignore());

            CreateMap<Domain.Model.AlertDefinition, AlertDefinition>()
                .ForMember(d => d.AlertTriggerHistories, opt => opt.Ignore())
                .ForMember(d => d.Created, opt => opt.Ignore())
                .ForMember(d => d.Modified, opt => opt.Ignore());
            
            CreateMap<Domain.Model.ApiCall, ApiCall>()
                .ForMember(d => d.Created, opt => opt.Ignore())
                .ForMember(d => d.Modified, opt => opt.Ignore());

            CreateMap<Domain.Model.AlertTriggerHistory, AlertTriggerHistory>()
                .ForMember(d => d.AlertDefinition, opt => opt.Ignore())
                .ForMember(d => d.Created, opt => opt.Ignore())
                .ForMember(d => d.Modified, opt => opt.Ignore());

            CreateMap<Domain.Model.AppUser, AppUser>()
                .ForMember(d => d.AlertDefinitions, opt => opt.Ignore())
                .ForMember(d => d.Created, opt => opt.Ignore())
                .ForMember(d => d.Modified, opt => opt.Ignore());

            CreateMap<Domain.Model.UserPreferences, UserPreferences>()
                .ForMember(d => d.AppUser, opt => opt.Ignore())
                .ForMember(d => d.Created, opt => opt.Ignore())
                .ForMember(d => d.Modified, opt => opt.Ignore());

            CreateMap<Domain.Model.AlertCriteria, AlertCriteria>()
                .EqualityComparison((s, d) => s.AlertCriteriaId == d.AlertCriteriaId)
                .ForMember(d => d.Created, opt => opt.Ignore())
                .ForMember(d => d.Modified, opt => opt.Ignore())
                .ForMember(d => d.AlertDefinition, opt => opt.Ignore())
                .ForMember(d => d.ParentCriteria, opt => opt.Ignore());

            CreateMap<Domain.Model.RefreshToken, RefreshToken>()
                .ForMember(d => d.Created, opt => opt.Ignore())
                .ForMember(d => d.Modified, opt => opt.Ignore());
        }
    }
}
