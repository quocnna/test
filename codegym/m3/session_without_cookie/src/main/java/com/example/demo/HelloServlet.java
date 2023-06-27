package com.example.demo;

import java.io.*;
import javax.servlet.http.*;
import javax.servlet.annotation.*;

@WebServlet(name = "helloServlet", value = "/hello-servlet")
public class HelloServlet extends HttpServlet {
    private String message;

    public void init() {
        message = "Hello World!";
    }

    public void doGet(HttpServletRequest request, HttpServletResponse response) throws IOException {
        HttpSession session = request.getSession(true);
        String sessionId = session.getId();

        String urlWithSessionId = response.encodeURL(request.getContextPath() + "/test;jsessionid=" + sessionId);

        response.sendRedirect(urlWithSessionId);
    }

    public void destroy() {
    }
}