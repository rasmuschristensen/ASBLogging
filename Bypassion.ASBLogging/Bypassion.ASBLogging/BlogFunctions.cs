using System;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Bypassion.ASBLogging;

public class BlogFunctions
{
    private readonly ILogger<BlogFunctions> _logger;

    public BlogFunctions(ILogger<BlogFunctions> logger)
    {
        _logger = logger;
    }

    [FunctionName(nameof(Run))]
    public void Run([ServiceBusTrigger("%InputQueue%", Connection = "ServiceBusConnection")] ServiceBusReceivedMessage message)
    {
        var user = message.Body.ToObjectFromJson<UserInfo>();
        throw new ApplicationException($"User {user.UserId} is not granted access to vault");
    }

}
