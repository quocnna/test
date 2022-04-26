package cg.crud_template_basic.controller;

import cg.crud_template_basic.model.Product;
import cg.crud_template_basic.service.ProductService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;

import java.util.List;

@Controller
public class ProductController {
    @Autowired
    private ProductService productService;

    @GetMapping
    public String getProduct(Pageable pageable){
        List<Product> res= productService.findAll(pageable);
        String a= res.get(1).getCategory().getName();
        return "product";
    }
}
