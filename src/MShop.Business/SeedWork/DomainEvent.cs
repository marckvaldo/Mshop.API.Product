using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MShop.Business.SeedWork
{
    public abstract class DomainEvent
    {
        public DateTime OccuredOn { get; set; }
        public DomainEvent()
        {
            OccuredOn = DateTime.Now;
        }

    }
}
