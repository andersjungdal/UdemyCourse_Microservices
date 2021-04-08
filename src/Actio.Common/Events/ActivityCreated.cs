using System;
using System.Collections.Generic;
using System.Text;

namespace Actio.Common.Events
{
    public class ActivityCreated : IAuthenticatedEvent
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public string Category { get; }
        public string Name { get; }
        public string Description { get; }
        public DateTime CreatedAt { get; }
        protected ActivityCreated()
        {
                    
        }
        public ActivityCreated(Guid id, Guid userid, string category, string name, 
            string description, DateTime createdat)
        {
            Id = id;
            UserId = userid;
            Category = category;
            Name = name;
            Description = description;
            CreatedAt = createdat;
        }
    }
}
