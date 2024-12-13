using System.ComponentModel.DataAnnotations;

namespace VinylShop.Web.Data.Entities
{
    public class OrderEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required Guid UserId { get; set; }
        public ICollection<OrderProductEntity> Products { get; set; } = [];
        public required DateTime Timestamp { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Processing;
    }

    public enum OrderStatus
    {
        [Display(Name = "Processing")]
        Processing,
        [Display(Name = "On the way")]
        OnTheWay,
        [Display(Name = "At the post office")]
        PostOffice,
        [Display(Name = "Received")]
        Received
    }
}
