package test.generic;

import java.util.List;

public class ProductService {
    void display(List<Product> productList){
        productList.forEach(System.out::println);
    }
}
