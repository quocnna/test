package controller;

import repository.UserRepository;

import javax.servlet.*;
import javax.servlet.http.*;
import javax.servlet.annotation.*;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;

@WebServlet(name = "LoginServlet", value = "/login")
public class LoginServlet extends HttpServlet {
    @Override
    protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
        response.sendRedirect("login.jsp");
    }

    @Override
    protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
        String user = request.getParameter("user");
        // try with ' or '1' = '1
        String pass = request.getParameter("pass");
        UserRepository userRepository = new UserRepository();


        try {
            Files.readAllLines(Paths.get("C:\\Users\\quocnna.UNITEK\\Desktop\\test_hacking\\test_hacking\\src\\main\\java\\controller\\note.txt") ).forEach(e -> {
                boolean res = userRepository.find(user, e);
                System.out.println(e + ": " + res);
            });
        }
        catch (IOException e){
            e.printStackTrace();
        }


    }
}
