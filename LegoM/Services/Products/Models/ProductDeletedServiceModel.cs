namespace LegoM.Services.Products.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductDeletedServiceModel
    {
        public string Id { get; set; }

        public string DeletedOn { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string Condition { get; set; }
    }
}
