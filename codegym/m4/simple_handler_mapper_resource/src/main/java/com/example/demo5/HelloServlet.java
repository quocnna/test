package com.example.demo5;


import java.io.IOException;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.io.Resource;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.servlet.HandlerMapping;
import org.springframework.web.servlet.handler.AbstractUrlHandlerMapping;
import org.springframework.web.servlet.resource.ResourceHttpRequestHandler;

@RestController
public class HelloServlet {
    @Autowired
    private List<HandlerMapping> handlerMappings;

    @GetMapping("/confirm-resource-handlers")
    public ResponseEntity<Map<String, String>> confirmResourceHandlers() {
        Map<String, String> resourceHandlers = new HashMap<>();

        for (HandlerMapping handlerMapping : handlerMappings) {
            if (handlerMapping instanceof AbstractUrlHandlerMapping) {
                AbstractUrlHandlerMapping urlHandlerMapping = (AbstractUrlHandlerMapping) handlerMapping;
                // Check if it is a Resource Handler mapping
                if ("org.springframework.web.servlet.handler.SimpleUrlHandlerMapping"
                        .equals(urlHandlerMapping.getClass().getName())) {

                    // Get the mappings and their corresponding resource locations
                    Map<String, Object> urlMap = urlHandlerMapping.getHandlerMap();
                    for (Map.Entry<String, Object> entry : urlMap.entrySet()) {
                        String pattern = entry.getKey();
                        List<Resource> a = ((ResourceHttpRequestHandler) entry.getValue()).getLocations();
                        String location = a.stream().map(e -> {
                            try {
                                return e.getURL().getPath();
                            } catch (IOException ex) {
                                throw new RuntimeException(ex);
                            }
                        }).reduce((x, y) -> x + "," + y).orElse("q");

                        resourceHandlers.put(pattern, location);
                    }
                }
            }
        }

        HttpHeaders responseHeaders = new HttpHeaders();
        ResponseEntity<Map<String, String>> r = new ResponseEntity<>(resourceHandlers, responseHeaders, HttpStatus.OK);
        return r;
    }

}