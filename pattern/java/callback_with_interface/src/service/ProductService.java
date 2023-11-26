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
        products.add(new Product(5,"airpod", "green", 123, 923));
    }

    public List<Product> search(ProductPredicate productPredicate){
        ArrayList<Product> res = new ArrayList<>();
        System.out.println("b");

        for (Product product : products) {
            if(productPredicate.test(product)){
                res.add(product);
            }
        }

        return res;
    }

    public List<Product> filterGreenApple() {
        List<Product> result = new ArrayList<>();

        for (Product apple : products) {
            if ("green".equals(apple.getColor())) {
                result.add(apple);
            }
        }
        return result;
    }

    public List<Product> findByColor(String color){
        ArrayList<Product> res = new ArrayList<>();
        for (Product product : products) {
            if (product.getColor().equals(color)) {
                res.add(product);
            }
        }

        return res;
    }

    public List<Product> findByWeight(int weight){
        ArrayList<Product> res = new ArrayList<>();
        for (Product product : products) {
            if (product.getWeight() == weight) {
                res.add(product);
            }
        }

        return res;
    }

    public List<Product> getAll(){
        return products;
    }
}
