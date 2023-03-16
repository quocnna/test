package com.example.remote_debugging;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

@SpringBootApplication
@RestController
public class RemoteDebuggingApplication {

    public static void main(String[] args) {
        SpringApplication.run(RemoteDebuggingApplication.class, args);
    }

//    https://www.rookout.com/blog/intellij-remote-debugging-java/
    @GetMapping("hello")
    public String hello(@RequestParam String name){
        return String.format("Hello %s", name);
    }
}
