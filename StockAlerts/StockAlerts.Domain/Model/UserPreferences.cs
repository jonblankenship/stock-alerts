using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockAlerts.Domain.Exceptions;
using StockAlerts.Domain.Repositories;

namespace StockAlerts.Domain.Model
{
    public class UserPreferences
    {
        private readonly IUserPreferencesRepository _userPreferencesRepository;

        public UserPreferences()
        { }

        public UserPreferences(IUserPreferencesRepository userPreferencesRepository)
        {
            _userPreferencesRepository = userPreferencesRepository ?? throw new ArgumentNullException(nameof(userPreferencesRepository));
        }

        public Guid UserPreferencesId { get; set; }

        public Guid AppUserId { get; set; }

        public bool ShouldSendEmail { get; set; }

        public string EmailAddress { get; set; }

        public bool ShouldSendPush { get; set; }

        public bool ShouldSendSms { get; set; }

        public string SmsNumber { get; set; }

        public async Task SaveAsync()
        {
            if (_userPreferencesRepository == null)
                throw new ApplicationException($"{nameof(UserPreferences)} instantiated without an {nameof(IUserPreferencesRepository)}.");

            Validate();
            await _userPreferencesRepository.SaveAsync(this);
        }

        private void Validate()
        {
            var errors = new List<string>();

            if (AppUserId == Guid.Empty)
                errors.Add("AppUserId must be provided.");
            if (ShouldSendEmail && string.IsNullOrWhiteSpace(EmailAddress))
                errors.Add("E-mail address must be provided if e-mail notifications are selected.");
            if (ShouldSendSms && string.IsNullOrWhiteSpace(SmsNumber))
                errors.Add("Phone number must be provided if SMS notifications are selected.");

            //TODO: Validate format of e-mail address and sms number

            if (errors.Any())
                throw new BadRequestException(string.Join(Environment.NewLine, errors));
        }
    }
}
