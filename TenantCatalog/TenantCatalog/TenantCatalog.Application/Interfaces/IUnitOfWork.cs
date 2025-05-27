using System.Threading;
using System.Threading.Tasks;

namespace TenantCatalog.Application.Interfaces
{
    /// <summary>
    /// The unit of work interface.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// The save entities async.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>bool.</returns>
        Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken);

        /// <summary>
        /// The save entities async.
        /// </summary>
        /// <returns>bool.</returns>
        Task<bool> SaveEntitiesAsync();
    }
}
