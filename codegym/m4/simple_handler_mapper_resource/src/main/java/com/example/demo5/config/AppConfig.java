package com.example.demo5.config;



import org.springframework.beans.BeansException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.context.ApplicationContext;
import org.springframework.context.ApplicationContextAware;
import org.springframework.context.annotation.ComponentScan;
import org.springframework.context.annotation.Configuration;

import org.springframework.core.io.Resource;
import org.springframework.web.servlet.HandlerMapping;
import org.springframework.web.servlet.config.annotation.EnableWebMvc;
import org.springframework.web.servlet.config.annotation.ResourceHandlerRegistry;
import org.springframework.web.servlet.config.annotation.WebMvcConfigurer;
import org.springframework.web.servlet.handler.SimpleUrlHandlerMapping;
import org.springframework.web.servlet.resource.ResourceHttpRequestHandler;
import org.springframework.web.servlet.resource.WebJarsResourceResolver;

import java.util.List;
import java.util.Map;

@Configuration
@EnableWebMvc
@ComponentScan("com.example")
public class AppConfig implements WebMvcConfigurer, ApplicationContextAware {

    private ApplicationContext applicationContext;

    @Override
    public void setApplicationContext(ApplicationContext applicationContext) throws BeansException {
        this.applicationContext = applicationContext;
    }



    @Override
    public void addResourceHandlers(ResourceHandlerRegistry registry) {
        registry
                .addResourceHandler("/a/**")
                .addResourceLocations("classpath:/a/");
        registry
                .addResourceHandler("/b/**")
                .addResourceLocations("classpath:/b/");

//        HandlerMapping handlerMapping = applicationContext.getBean(HandlerMapping.class);
//        if (handlerMapping instanceof SimpleUrlHandlerMapping) {
//            SimpleUrlHandlerMapping urlHandlerMapping = (SimpleUrlHandlerMapping) handlerMapping;
//
//            // Get the handlerMap
//            Map<String, Object> handlerMap = urlHandlerMapping.getHandlerMap();
//
//            // Iterate over the handlerMap to retrieve the patterns and locations
//            for (Map.Entry<String, Object> entry : handlerMap.entrySet()) {
//                String pattern = entry.getKey();
//                Object handler = entry.getValue();
//
//                if (handler instanceof ResourceHttpRequestHandler) {
//                    ResourceHttpRequestHandler resourceHandler = (ResourceHttpRequestHandler) handler;
//                    List<Resource> locations = resourceHandler.getLocations();
//
//                    // Do something with the pattern and locations
//                    System.out.println("Pattern: " + pattern);
//                    System.out.println("Locations: " + locations);
//                }
//            }
//
//        }
    }
}
