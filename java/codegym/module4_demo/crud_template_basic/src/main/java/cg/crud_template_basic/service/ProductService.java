package cg.crud_template_basic.service;

import cg.crud_template_basic.model.Product;
import cg.crud_template_basic.repository.ProductRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;

import java.util.List;


@Service
public class ProductService {
    @Autowired
    private ProductRepository productRepository;

    public List<Product> findAll(Pageable pageable){
        return productRepository.findAll(pageable).getContent();
    }
}
