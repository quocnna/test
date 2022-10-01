import model.Product;
import service.ProductSearch;
import service.ProductService;

import java.util.List;
import java.util.stream.Collectors;

public class Test {
    public static void main(String[] args) {
        ProductService productService= new ProductService();
        List<Product> productSearchPrice= productService.search(new ProductSearch() {
            @Override
            public List<Product> search() {
                System.out.println("a");
                return productService.getAll().stream().filter(e-> e.getPrice()> 500).collect(Collectors.toList());
            }
        });

        productSearchPrice.forEach(System.out:: println);

        System.out.println("---");

        List<Product> productSearchWeight= productService.search(()->
             productService.getAll().stream().filter(e-> e.getWeight()> 5).collect(Collectors.toList())
        );

        productSearchWeight.forEach(System.out:: println);
    }
}
