using System;


namespace SupplyChain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }

        public byte[] Image { get; set; }
        public string imgPath { get; set; }

    }
}
