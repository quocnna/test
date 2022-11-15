package cg.thymeleaf_fragment;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;

@SpringBootApplication
@Controller
public class ThymeleafFragmentApplication {

    public static void main(String[] args) {
        SpringApplication.run(ThymeleafFragmentApplication.class, args);
    }

    @GetMapping
    public String home(){
        return "home";
    }

}
