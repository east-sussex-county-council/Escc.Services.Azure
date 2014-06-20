Escc.Services.Azure
===================

Windows Azure implementations of services defined in the Escc.Services project. 

Email
-----

Windows Azure does not have a built-in SMTP service. One option is to serialise emails to Azure storage, and have an on-premise application watch the queue and send the emails using your existing SMTP server. See [Escc.AzureEmailForwarder](https://github.com/east-sussex-county-council/Escc.AzureEmailForwarder) for more details.

See the [Escc.Services](https://github.com/east-sussex-county-council/Escc.Services) project for examples of how to configure and send emails using an `IEmailSender`. Simply specify `AzureQueuedEmailSender` as the `IEmailSender` in web.config or app.config to send emails to Azure storage instead.
