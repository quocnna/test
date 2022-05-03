package cg.crud_template_basic.controller;

import cg.crud_template_basic.model.Product;
import cg.crud_template_basic.service.CategoryService;
import cg.crud_template_basic.service.ProductService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Sort;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestParam;

@Controller
public class ProductController {
    @Autowired
    private ProductService productService;

    @Autowired
    private CategoryService categoryService;

    @GetMapping
    public String getProduct(Model model, @RequestParam(required = false) String searchBy, @RequestParam(required = false) String searchVal, Pageable pageable){
        Page<Product> res= productService.find(searchBy, searchVal, pageable);

        boolean isASC = false;
        Sort sort = res.getSort();

        if(sort.isSorted()){
            isASC = "asc".equalsIgnoreCase(sort.toString().split(":")[1].trim());
        }

//        if(sort.isSorted()){
//            isASC = sort.get().findFirst().get().isAscending();
//        }

        model.addAttribute("reverseSortDir", isASC ? "desc" : "asc");
        model.addAttribute("res", res);
        model.addAttribute("cate", categoryService.findAll());
        return "product";
    }

    @PostMapping
    public String saveProduct(Product product){
        productService.saveProduct(product);
        return "redirect:/";
    }
}
