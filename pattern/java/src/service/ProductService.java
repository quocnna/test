package service;

import model.Product;

import java.util.ArrayList;
import java.util.List;

public class ProductService {
    private static List<Product> products= new ArrayList<>();

    static {
        products.add(new Product(1,"iphone", "gold", 15, 1000));
        products.add(new Product(2,"nokia", "yellow", 10, 500));
        products.add(new Product(3,"samsung", "gold", 8, 700));
        products.add(new Product(4,"oppo", "red", 5, 300));
        products.add(new Product(5,"lg", "blue", 120, 900));
    }


    public List<Product> search(ProductSearch productSearch){
        return productSearch.search();
    }

    public List<Product> getAll(){
        return products;
    }
}
