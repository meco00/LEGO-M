namespace LegoM.Services.Orders.Models
{
    public class OrderDetailsServiceModel:OrderServiceModel
    {
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string ZipCode { get; set; }

        public int OrderedItems { get; set; }

        public bool IsAccomplished { get; set; }

    }
}
