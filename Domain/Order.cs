using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Order
    {              

        [Key]
        public int Id { get; set; }

        public int Number { get; set; }

        [NotMapped]
        public OrderStatus Status
        {
            get { return (OrderStatus)StatusValue; }
            set { StatusValue = (int)value; }
        }
        public int StatusValue { get; set; }

        public ICollection<OrderProducts> OrderProducts { get; set; }
    }

    public enum OrderStatus
    {
        Pending,
        Paid,
    }
}
