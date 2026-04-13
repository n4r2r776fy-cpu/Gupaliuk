using System;

namespace lab28v2
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        // Зв'язок з категорією
        public Category Category { get; set; }
    }
}