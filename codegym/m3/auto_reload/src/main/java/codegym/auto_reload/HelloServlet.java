package codegym.auto_reload;

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
        response.setContentType("text/html");

        // Hello
        PrintWriter out = response.getWriter();
        out.println("<html><body>");
        out.println("<h1>hello</h1>");
        out.println("</body></html>");
    }

    public void destroy() {
    }

    private void test(int a, int b){
        System.out.println("a");
    }

    private int test(String e, int b){
        System.out.println("b");
        return 0;
    }
}