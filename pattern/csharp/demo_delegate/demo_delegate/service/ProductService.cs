using System;
using demo_delegate.model;
using System.Collections.Generic;
using System.Collections;

namespace demo_delegate.service
{
    public class ProductService
    {
        private List<Product> products = new List<Product>();

        public ProductService()
        {
            products.Add(new Product(1, "iphone", "gold", 15, 1000));
            products.Add(new Product(2, "nokia", "yellow", 10, 500));
            products.Add(new Product(3, "samsung", "gold", 8, 700));
            products.Add(new Product(4, "oppo", "red", 5, 300));
            products.Add(new Product(5, "lg", "blue", 120, 900));
        }

        public List<Product> getAll()
        {
            return products;
        }

        public List<Product> search(search search)
        {
            return search.Invoke();
        }
    }
}
