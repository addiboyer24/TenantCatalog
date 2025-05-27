using CDMS.CP.Platform.Common.Domain;
using CDMS.CP.Platform.Common.Helpers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TenantCatalog.Application.Interfaces;

namespace TenantCatalog.Infrastructure
{
    /// <summary>
    /// The mediator extension.
    /// </summary>
    public static class MediatorExtension
    {
        /// <summary>
        /// The dispatch domain events async.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        /// <param name="ctx">The context.</param>
        /// <returns>Task.</returns>
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, IDomainContext ctx)
        {
            Guard.NotNull(mediator, nameof(mediator));
            Guard.NotNull(ctx, nameof(ctx));

            IList<AggregateRoot> entitiesToRemove = new List<AggregateRoot>();
            foreach (AggregateRoot entity in ctx.RetrieveDomainEntities())
            {
                IDictionary<Type, IList<INotification>> events = await entity.GetEventsAsync();
                foreach (IList<INotification> notificationList in events.Values)
                {
                    foreach (INotification notification in notificationList)
                    {
                        await mediator.Publish(notification);
                        entitiesToRemove.Add(entity);
                    }
                }

                entity.ClearEvents();
            }

            foreach (AggregateRoot entity in entitiesToRemove)
            {
                ctx.RemoveDomainEntity(entity);
            }
        }
    }
}
