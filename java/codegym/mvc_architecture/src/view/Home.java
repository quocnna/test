package view;

import controller.ProductController;
import model.Product;

public class Home {
    public static void main(String[] args) {
        Product product= new Product("Iphone");
        ProductController productController= new ProductController();
        productController.save(product);

        System.out.println("Display List");
        productController.getAll().forEach(System.out::println);
    }
}
