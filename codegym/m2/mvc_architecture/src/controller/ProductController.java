package controller;

import model.Product;
import service.ProductService;

import java.util.List;

public class ProductController {
    private ProductService productService= new ProductService();

    public List<Product> getAll(){
        return productService.getAll();
    }

    public void save(Product product){
        productService.save(product);
    }

    public boolean delete(int id){
        return productService.delete(id);
    }
}
