using StockAlerts.Domain.Enums;
using StockAlerts.Domain.QueueMessages;
using StockAlerts.Domain.Repositories;
using StockAlerts.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockAlerts.Domain.Factories;

namespace StockAlerts.Domain.Model
{
    public class AlertDefinition
    {
        private readonly IAlertDefinitionsRepository _alertDefinitionsRepository;
        private readonly IAlertCriteriaSpecificationFactory _alertCriteriaSpecificationFactory;
        private readonly INotificationsService _notificationsService;

        public AlertDefinition(
            IAlertDefinitionsRepository alertDefinitionsRepository,
            IAlertCriteriaSpecificationFactory alertCriteriaSpecificationFactory,
            INotificationsService notificationsService)
        {
            _alertDefinitionsRepository = alertDefinitionsRepository ?? throw new ArgumentNullException(nameof(alertDefinitionsRepository));
            _alertCriteriaSpecificationFactory = alertCriteriaSpecificationFactory ?? throw new ArgumentNullException(nameof(alertCriteriaSpecificationFactory));
            _notificationsService = notificationsService ?? throw new ArgumentNullException(nameof(notificationsService));
        }

        public Guid AlertDefinitionId { get; set; }
        
        public AlertDefinitionStatuses Status { get; set; }

        public AlertDefinitionType Type { get; set; }

        public decimal? PriceLevel { get; set; }

        public DateTimeOffset? LastSent { get; set; }

        public string Name { get; set; }

        public ComparisonOperator ComparisonOperator { get; set; }

        public ICollection<AlertTriggerHistory> AlertTriggerHistories { get; set; }

        public ICollection<AlertCriteria> AlertCriterias { get; set; }

        public AlertCriteria RootCriteria => (from ac in AlertCriterias
                                              where ac.RootCriteriaId == ac.AlertCriteriaId
                                              select ac).Single();
        
        public Stock Stock { get; set; }

        public AppUser AppUser { get; set; }

        public async Task SaveAsync()
        {
            if (_alertDefinitionsRepository == null)
                throw new ApplicationException($"{nameof(AlertDefinition)} instantiated without an {nameof(IAlertDefinitionsRepository)}.");

            await _alertDefinitionsRepository.SaveAsync(this);
        }

        public async Task EvaluateAsync(AlertEvaluationMessage message)
        {
            if (Status == AlertDefinitionStatuses.Enabled)
            {
                if (!LastSent.HasValue || DateTimeOffset.UtcNow.Date > LastSent.Value.Date) // Alert not already sent today
                {
                    var specification = _alertCriteriaSpecificationFactory.CreateSpecification(this);
                    if (specification.IsSatisfiedBy(message))
                    {
                        var subject = $"Stock Alert Triggered: {Stock.Symbol}";
                        var notificationMessage = $"Stock Alert Triggered {Environment.NewLine}" +
                                                  $"Notification Name: {Name}{Environment.NewLine}" +
                                                  $"Stock: {Stock.Symbol} ({message.LastPrice:C}){Environment.NewLine}" +
                                                  $"Criteria: {RootCriteria}{Environment.NewLine}";

                        await TriggerAlertAsync(subject, notificationMessage);
                    }
                }
            }
        }

        private async Task TriggerAlertAsync(string subject, string notificationMessage)
        {
            // Log alert
            AlertTriggerHistories.Add(new AlertTriggerHistory
            {
                AlertDefinitionId = AlertDefinitionId,
                TimeTriggered = DateTimeOffset.UtcNow
            });

            // Send notifications
            await _notificationsService.SendNotificationsAsync(AppUser, subject, notificationMessage);

            // Update AlertDefinition
            LastSent = DateTimeOffset.UtcNow;
            await SaveAsync();
        }
    }
}

