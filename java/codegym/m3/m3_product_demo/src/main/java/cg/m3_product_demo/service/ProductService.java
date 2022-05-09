package cg.m3_product_demo.service;

import cg.m3_product_demo.repository.ProductRepository;
import cg.model.Product;

import java.util.List;

public class ProductService {
    private ProductRepository productRepository= new ProductRepository();

    public int create(Product product) {
        return 0;
    }

    public List findAll() {
        return productRepository.findAll();
    }

    public Product findById(int id){
        return productRepository.findById(id);
    }

    public int delete(int id) {
        return 0;
    }

    public List searchByName(String name) {
        return null;
    }
}
