package com.example.test_login_session;

import jakarta.servlet.*;
import jakarta.servlet.annotation.WebFilter;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import jakarta.servlet.http.HttpSession;

import java.io.IOException;
import java.util.Set;

@WebFilter("/*")
public class AuthenFilter implements Filter {
    private static final Set<String> ALLOWED_PATHS = Set.of("", "/login", "/logout", "/register","/login.jsp");
//    private static final Set<String> ALLOWED_PATHS = Set.of("", "/login", "/logout", "/register","/login.jsp");

    @Override
    public void doFilter(ServletRequest servletRequest, ServletResponse servletResponse, FilterChain filterChain) throws IOException, ServletException {
        HttpServletRequest httpRequest = (HttpServletRequest) servletRequest;
        HttpServletResponse httpResponse = (HttpServletResponse) servletResponse;
        HttpSession session = httpRequest.getSession(false);

//        String path = httpRequest.getRequestURI().substring(httpRequest.getContextPath().length()).replaceAll("[/]+$", "");
        String path = httpRequest.getServletPath();
        String encodedURL=httpResponse.encodeURL(path);
        boolean isLoggedIn = session != null && session.getAttribute("username") != null;
        boolean allowedPath = ALLOWED_PATHS.contains(path);

        if (isLoggedIn || allowedPath) {
            filterChain.doFilter(servletRequest, servletResponse);
        } else {
            httpResponse.sendRedirect(httpRequest.getContextPath() + "/login");
        }
    }
}

