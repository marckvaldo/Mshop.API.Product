using MShop.Core.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Business.Events.Category
{
    public class CategoryUpdateEvent : DomainEvent
    {
        public CategoryUpdateEvent(Guid id, string name, bool isActive)
        {
            Id = id;
            Name = name;
            IsActive = isActive;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
