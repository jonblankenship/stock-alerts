### TODO

#### General

- Stripe integration

  

#### StockAlerts.Data

- Consider reorganizing dataclasses (model, repositories) by subject/aggregate rather than type

  

#### StockAlerts.Domain

- Split authentication out into its own project

- Consider reorganizing domain classes (model, services, factories, repos) by subject/aggregate rather than type

  

#### StockAlerts.Functions

- ~~Split HTTP endpoints out of Functions project into their own Web API project~~

  - ~~Create app service for Web API~~
  - ~~Set up release pipeline for Web API~~

- Split authentication out into its own class library project

- Create PushNotificationFunction

- Create SmsNotificationFunction

- Create EmailNotificationFunction

  

#### StockAlerts.App

- Alert Definition create screen
  - Search for security
  - Build criteria
- Edit Alert Definition
- Delete Alert Definition
- Display Alert History
- Edit User Preferences
- Implement push notifications
- Keep logged in/exchange refresh token

- Style app
- Create iOS app