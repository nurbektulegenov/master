using System;
using System.Collections.Generic;
using BookTestProject.Interfaces;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Event.Default;
using NHibernate.Persister.Entity;

namespace BookTestProject {
    public class SoftDeleteEventListener : DefaultDeleteEventListener {
        protected override void DeleteEntity(IEventSource session, object entity,
            EntityEntry entityEntry, bool isCascadeDeleteEnabled,
            IEntityPersister persister, ISet<object> transientEntities) {
            if (entity is ISoftDeletable) {
                var e = (ISoftDeletable)entity;
                e.IsDeleted = true;
                CascadeBeforeDelete(session, persister, entity, entityEntry, transientEntities);
                CascadeAfterDelete(session, persister, entity, transientEntities);
            }
            else {
                base.DeleteEntity(session, entity, entityEntry, isCascadeDeleteEnabled, persister, transientEntities);
            }
        }
    }
}

