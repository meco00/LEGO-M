namespace LegoM.Services.Orders.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class OrderServiceModel
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string OrderedOn { get; set; }
       
        public string TotalAmount { get; set; }
    }
}
