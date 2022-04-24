package codegym.m4_product.controller;

import codegym.m4_product.model.Product;
import codegym.m4_product.service.ProductService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.servlet.mvc.support.RedirectAttributes;

import java.util.Optional;

@Controller
public class ProductController {
    @Autowired
    private ProductService productService;

    @GetMapping
    public String home(Model model){
        model.addAttribute("result", productService.getAll());
        return "product";
    }

    @PostMapping("/save")
    public String save(Product product, RedirectAttributes redirectAttributes){
        Optional<Product> p = productService.save(product);
        if(p.isPresent()) {
            redirectAttributes.addFlashAttribute("success", p.get().getName()+ " created successfully!");
        }else{
            redirectAttributes.addFlashAttribute("error",p.get().getName()+ " created fail!");
        }

        return "redirect:/";
    }
}
