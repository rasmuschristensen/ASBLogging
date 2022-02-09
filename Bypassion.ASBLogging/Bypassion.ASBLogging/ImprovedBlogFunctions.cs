using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;

namespace Bypassion.ASBLogging;

public class ImprovedBlogFunctions
{
    private readonly ILogger<BlogFunctions> _logger;

    public ImprovedBlogFunctions(ILogger<BlogFunctions> logger)
    {
        _logger = logger;
    }

    [FunctionName(nameof(RunAsync))]
    public async Task RunAsync([ServiceBusTrigger("%InputQueue%", 
        Connection = "ServiceBusConnection",
        AutoCompleteMessages = false)] ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        try
        {
            var user = message.Body.ToObjectFromJson<UserInfo>();

            if (user.UserId != 123)
            {
                throw new ApplicationException($"User {user.UserId} is not granted access to vault"); 
            }
        }
        catch (Exception ex)
        {
            await messageActions.DeadLetterMessageAsync(message, ex.Message);
            throw;
        }

        await messageActions.CompleteMessageAsync(message);
    }
}
