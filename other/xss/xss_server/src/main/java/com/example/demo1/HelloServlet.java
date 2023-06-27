package com.example.demo1;

import java.io.*;
import java.util.stream.Collectors;

import com.google.gson.Gson;
import jakarta.servlet.ServletException;
import jakarta.servlet.http.*;
import jakarta.servlet.annotation.*;

@WebServlet(name = "helloServlet", value = "/second")
public class HelloServlet extends HttpServlet {
    private String message;

    public void init() {
        message = "Hello World!";
    }

    public void doGet(HttpServletRequest request, HttpServletResponse response) throws IOException {
       String ck = request.getParameter("ck");
        System.out.println(ck);
    }

    @Override
    protected void doPost(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
        String requestData = req.getReader().lines().collect(Collectors.joining());


        BufferedReader reader = req.getReader();
        Gson gson = new Gson();


        User user1 = new User("Sun", "4");
        String userJson = gson.toJson(user1);

        User myBean = gson.fromJson(requestData, User.class);
        System.out.println("aaa");



    }

    public void destroy() {
    }
}