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
import org.springframework.validation.BindingResult;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.servlet.mvc.support.RedirectAttributes;

@Controller
public class ProductController {
    @Autowired
    private ProductService productService;

    @Autowired
    private CategoryService categoryService;

    @GetMapping
    public String view(Model model, Product product, BindingResult bindingResult, @RequestParam(defaultValue = "") String q, Pageable pageable){
        model.addAttribute("res", productService.find(q, pageable));
        model.addAttribute("cate", categoryService.findAll());
        model.addAttribute("product", product);
        model.addAttribute("q", q);
        model.addAttribute("sort", pageable.getSort());
        model.addAttribute("isError", bindingResult.hasErrors());

        return "product";
    }

    @PostMapping
    public String save(Model model, @Validated Product product, BindingResult bindingResult, Pageable pageable, RedirectAttributes redirect){
        if (bindingResult.hasErrors())
            return view(model,product, bindingResult,"", pageable);

        productService.save(product);
        redirect.addFlashAttribute("msg", String.format("%s successfully",(product.getId()>0?"Updated ":"Created ")+ product.getName()));

        return "redirect:/";
    }

    @DeleteMapping
    public String delete(@RequestParam int id, RedirectAttributes redirect){
        productService.delete(id);
        redirect.addFlashAttribute("msg", "Deleted successfully");

        return "redirect:/";
    }
}
