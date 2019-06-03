using StockAlerts.Domain.Enums;
using StockAlerts.Domain.QueueMessages;
using StockAlerts.Domain.Repositories;
using StockAlerts.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using StockAlerts.Domain.Exceptions;
using StockAlerts.Domain.Factories;

namespace StockAlerts.Domain.Model
{
    public class AlertDefinition
    {
        private readonly IAlertDefinitionsRepository _alertDefinitionsRepository;
        private readonly IAlertCriteriaSpecificationFactory _alertCriteriaSpecificationFactory;
        private readonly INotificationsService _notificationsService;

        public AlertDefinition()
        { }

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

        public Guid AppUserId { get; set; }

        public Guid StockId { get; set; }

        public AlertDefinitionStatuses Status { get; set; }

        public DateTimeOffset? LastSent { get; set; }

        public string Name { get; set; }
        
        public ICollection<AlertTriggerHistory> AlertTriggerHistories { get; set; }

        public AlertCriteria RootCriteria {get; set; }
        
        public Stock Stock { get; set; }

        public AppUser AppUser { get; set; }

        public bool ContainsAlertCriteriaId(Guid alertCriteriaId) =>
            RootCriteria.ContainsAlertCriteriaId(alertCriteriaId);

        public async Task SaveAsync()
        {
            if (_alertDefinitionsRepository == null)
                throw new ApplicationException($"{nameof(AlertDefinition)} instantiated without an {nameof(IAlertDefinitionsRepository)}.");

            Validate();
            await _alertDefinitionsRepository.SaveAsync(this);
        }

        public async Task DeleteAsync()
        {
            if (_alertDefinitionsRepository == null)
                throw new ApplicationException($"{nameof(AlertDefinition)} instantiated without an {nameof(IAlertDefinitionsRepository)}.");

            await _alertDefinitionsRepository.DeleteAsync(this);
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

        private void Validate()
        {
            var errors = new List<string>();

            if (StockId == Guid.Empty)
                errors.Add("StockId must be provided.");
            if (AppUserId == Guid.Empty)
                errors.Add("AppUserId must be provided.");
            if (string.IsNullOrWhiteSpace(Name))
                errors.Add("Alert Definition Name must be provided.");
            if (RootCriteria == null)
                errors.Add("Alert Definition must have a root criteria.");

            RootCriteria.Validate(errors);
            
            if (errors.Any())
                throw new BadRequestException(string.Join(Environment.NewLine, errors));
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

