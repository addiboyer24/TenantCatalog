using MediatR;
using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;

namespace TenantCatalog.Events.Controllers
{
    /// <summary>
    /// The authorization controller.
    /// </summary>
    public class AppTemplateController
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppTemplateController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public AppTemplateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// The hello azure function.
        /// </summary>
        /// <param name="timerInfo">The timer info.</param>
        /// <returns>Task.</returns>
        [FunctionName("hello-azure-function")]
        public async Task HelloAzureFunction([TimerTrigger("0 */5 * * * *", RunOnStartup = true)] TimerInfo timerInfo)
        {
            System.Diagnostics.Debug.WriteLine($"Hello azure function at {timerInfo.Schedule}");
            await Task.CompletedTask;
        }
    }
}
