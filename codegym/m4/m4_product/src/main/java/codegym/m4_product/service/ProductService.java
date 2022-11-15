package codegym.m4_product.service;

import codegym.m4_product.model.Product;
import codegym.m4_product.repository.ProductRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.Collections;
import java.util.List;
import java.util.Optional;

@Service
public class ProductService {
    @Autowired
    private ProductRepository productRepository;

    public List<Product> getAll(){
        List<Product> res= productRepository.findAll();
        Collections.reverse(res);
        return res;
    }

    public int delete(int id){
        return id;
    }

    public Optional<Product> save(Product product){
        return Optional.of(productRepository.save(product));
    }

    public List<Product> search(String field, String value){
        return Collections.emptyList();
    }
}
