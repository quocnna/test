using System;
using System.Collections.Generic;
using demo_delegate.model;
using demo_delegate.service;

namespace demo_delegate
{
    public delegate List<Product> search();
    class Program
    {
        private static ProductService productService = new ProductService();
           
        static search test = productSearchPrice;

        static void Main(string[] args)
        {
            
            List<Product> productSearchPrice= productService.search(test);
            foreach (Product p in productSearchPrice) {
                Console.WriteLine(p);
            }

            Console.WriteLine("---");

            List<Product> productSearchWeight = productService.search(
                ()=> {
                    List<Product> result = new List<Product>();
                    List<Product> products = productService.getAll();
                    foreach (Product p in products)
                    {
                        if (p.getWeight() > 5)
                        {
                            result.Add(p);
                        }
                    }
                    return result;
                }
                );
            foreach (Product p in productSearchWeight)
            {
                Console.WriteLine(p);
            }
        }

        public static List<Product> productSearchPrice()
        {
            List<Product> result = new List<Product>();
            List<Product> products= productService.getAll();
            foreach (Product p in products) {
                if(p.getPrice()> 500)
                {
                    result.Add(p);
                }
            }

            return result;
        }
    }
}
