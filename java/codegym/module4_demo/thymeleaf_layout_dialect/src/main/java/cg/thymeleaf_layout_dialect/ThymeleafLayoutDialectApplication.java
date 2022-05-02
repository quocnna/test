package cg.thymeleaf_layout_dialect;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;

@SpringBootApplication
@Controller
public class ThymeleafLayoutDialectApplication {

    public static void main(String[] args) {
        SpringApplication.run(ThymeleafLayoutDialectApplication.class, args);
    }

    @GetMapping
    public String getHome(){
        return "index";
    }

    @GetMapping("/contact")
    public String contactPage() {
        return "contact";
    }
}
