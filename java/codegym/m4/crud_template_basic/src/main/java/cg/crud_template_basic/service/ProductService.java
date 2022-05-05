package cg.crud_template_basic.service;

import cg.crud_template_basic.model.Product;
import cg.crud_template_basic.repository.ProductRepository;
import cg.crud_template_basic.util.CommonUtil;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;
import org.thymeleaf.util.StringUtils;

import java.time.LocalDate;

@Service
public class ProductService {
    @Autowired
    private ProductRepository productRepository;

    public Page<Product> find(String searchBy, String searchVal, Pageable pageable){
        if(StringUtils.isEmpty(searchBy) && StringUtils.isEmpty(searchVal)){
            return productRepository.findAll(pageable);
        }

        switch(searchBy){
            case "name":
                return productRepository.findByNameContaining(searchVal, pageable);
            case "price":
                return productRepository.findByPriceGreaterThanEqual(CommonUtil.parseDouble(searchVal), pageable);
            case "exp_date":
                if(CommonUtil.parseLocalDate(searchVal)){
                    return productRepository.findByEXPDate(LocalDate.parse(searchVal).toString(), LocalDate.now().toString(), pageable);
                }
                break;
            case "manufacturer":
                return productRepository.findByManufacturerContaining(searchVal, pageable);
            case "category":
                return productRepository.findByCategoryName("%".concat(searchVal).concat("%"), pageable);
            default:
                Page<Product> products= productRepository.findAllByValue("%".concat(searchVal).concat("%"), pageable);
                return products;
        }

        return Page.empty();
    }

    public Product saveProduct(Product product){
        Product p= productRepository.save(product);
        return p;
    }
}
