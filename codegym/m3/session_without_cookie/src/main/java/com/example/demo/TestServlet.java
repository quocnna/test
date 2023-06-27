package com.example.demo;

import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;
import java.io.IOException;

@WebServlet(name = "test", value = "/test")
public class TestServlet extends HttpServlet {
    @Override
    protected void doGet(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
        HttpSession session= req.getSession(false);
        if(session != null){
           /* String pathInfo = req.getPathInfo();
            if (pathInfo != null) {
                String[] pathParts = pathInfo.split(";");
                for (String pathPart : pathParts) {
                    if (pathPart.startsWith("jsessionid=")) {
                        String jsessionId = pathPart.substring("jsessionid=".length());
                        // Compare the jsessionId with session.getId()
                        if (session.getId().equals(jsessionId)) {
                            // Session ID matches
                            // Your session-related logic here
                            System.out.println("matches");
                        } else {
                            // Session ID doesn't match
                            // Your handling logic here
                            System.out.println("no matches");
                        }
                        break;
                    }
                }
            }*/
            String jsessionId = req.getRequestedSessionId();
            if (jsessionId != null) {
                // Compare the jsessionId with session.getId()
                if (session.getId().equals(jsessionId)) {
                    // Session ID matches
                    // Your session-related logic here
                    System.out.println("matches");
                } else {
                    // Session ID doesn't match
                    // Your handling logic here
                    System.out.println("doesn't match");
                }
            }
        }
        else{
            System.out.println("session is invalid");
        }
    }

    @Override
    protected void doPost(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
        super.doPost(req, resp);
    }
}
