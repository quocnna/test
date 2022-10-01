using System;
namespace demo_delegate.model
{
    public class Product
    {
        private int id;
        private String name;
        private String color;
        private double weight;
        private double price;

        public Product()
        {
        }

        public Product(int id, string name, string color, double weight, double price)
        {
            this.id = id;
            this.name = name;
            this.color = color;
            this.weight = weight;
            this.price = price;
        }

        public void setId(int id)
        {
            this.id = id;
        }

        public int getId()
        {
            return id;
        }

        public double getPrice()
        {
            return price;
        }

        public double getWeight()
        {
            return weight;
        }

        
    public override String ToString()
        {
            return "Product{" +
                    "id=" + id +
                    ", name='" + name + '\'' +
                    ", color='" + color + '\'' +
                    ", weight=" + weight +
                    ", price=" + price +
                    '}';
        }
    }
}
