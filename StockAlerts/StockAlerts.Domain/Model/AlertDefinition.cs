using StockAlerts.Domain.Enums;
using StockAlerts.Domain.QueueMessages;
using StockAlerts.Domain.Repositories;
using StockAlerts.Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockAlerts.Domain.Model
{
    public class AlertDefinition
    {
        private readonly IAlertDefinitionsRepository _alertDefinitionsRepository;
        private readonly INotificationsService _notificationsService;

        public AlertDefinition(
            IAlertDefinitionsRepository alertDefinitionsRepository,
            INotificationsService notificationsService)
        {
            _alertDefinitionsRepository = alertDefinitionsRepository ?? throw new ArgumentNullException(nameof(alertDefinitionsRepository));
            _notificationsService = notificationsService ?? throw new ArgumentNullException(nameof(notificationsService));
        }

        public Guid AlertDefinitionId { get; set; }
        
        public AlertDefinitionStatuses Status { get; set; }

        public AlertDefinitionType Type { get; set; }

        public decimal? PriceLevel { get; set; }

        public DateTimeOffset? LastSent { get; set; }

        public ComparisonOperator ComparisonOperator { get; set; }

        public ICollection<AlertTriggerHistory> AlertTriggerHistories { get; set; }

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
                if (Type == AlertDefinitionType.PriceAlert)
                {
                    await EvaluatePriceAlertAsync(message);
                }

                // Evaluate other alerts - Dividend change alert? PE alert? Earnings announcement?
            }
        }

        private async Task EvaluatePriceAlertAsync(AlertEvaluationMessage message)
        {
            if (!LastSent.HasValue || DateTimeOffset.UtcNow.Date > LastSent.Value.Date) // Alert not already sent today
            {
                // Price Greater Than Alert
                if (ComparisonOperator == ComparisonOperator.GreaterThan &&
                    message.LastPrice > PriceLevel &&
                    message.PreviousLastPrice <= PriceLevel)
                {
                    var subject = $"Alert: {Stock.Symbol} greater than {PriceLevel:C}";
                    var notificationMessage =
                        $"{Stock.Symbol} has exceeded {PriceLevel:C} and triggered an alert.  It is currently trading at {message.LastPrice:C}.";

                    await TriggerAlertAsync(subject, notificationMessage);
                }

                // Price Less Than Alert
                if (ComparisonOperator == ComparisonOperator.LessThan &&
                    message.LastPrice < PriceLevel &&
                    message.PreviousLastPrice >= PriceLevel)
                {
                    var subject = $"Alert: {Stock.Symbol} less than {PriceLevel:C}";
                    var notificationMessage =
                        $"{Stock.Symbol} has dropped below {PriceLevel:C} and triggered an alert.  It is currently trading at {message.LastPrice:C}.";

                    await TriggerAlertAsync(subject, notificationMessage);
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

