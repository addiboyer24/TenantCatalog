using CDMS.CP.Platform.Common.Domain;
using System.Collections.Generic;

namespace TenantCatalog.Application.Interfaces
{
    /// <summary>
    /// The domain context interface.
    /// </summary>
    public interface IDomainContext : IUnitOfWork
    {
        /// <summary>
        /// The retrieve domain entities.
        /// </summary>
        /// <returns>Aggregate root.</returns>
        IEnumerable<AggregateRoot> RetrieveDomainEntities();

        /// <summary>
        /// The add domain entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void AddDomainEntity(AggregateRoot entity);

        /// <summary>
        /// The add domain entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void AddDomainEntities(IEnumerable<AggregateRoot> entities);

        /// <summary>
        /// The remove domain entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void RemoveDomainEntity(AggregateRoot entity);
    }
}
