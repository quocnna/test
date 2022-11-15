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

    public Page<Product> find(String searchQuery, Pageable pageable){
        if(!StringUtils.isEmpty(searchQuery)){
            String[] tmp = searchQuery.split(":");
            String searchBy = tmp[0];
            String searchVal = tmp[1];

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
        }

        return productRepository.findAll(pageable);
    }

    public Product save(Product product){
        return productRepository.save(product);
    }

    public void delete(int id){
        productRepository.deleteById(id);
    }
}
