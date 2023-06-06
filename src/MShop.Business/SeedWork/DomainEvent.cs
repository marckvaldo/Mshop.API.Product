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
