package cg.m3_product_demo;

import cg.m3_product_demo.service.ProductService;
import cg.model.Product;

import java.io.*;
import java.util.List;
import javax.servlet.ServletException;
import javax.servlet.http.*;
import javax.servlet.annotation.*;

@WebServlet(name = "productServlet", value = "/product")
public class ProductServlet extends HttpServlet {
    private ProductService productService = new ProductService();

    public void doGet(HttpServletRequest request, HttpServletResponse response) throws IOException, ServletException {
        String id = request.getParameter("id");
        if(id != null){
            request.setAttribute("product", productService.findById(Integer.parseInt(id)));
            request.getRequestDispatcher("form.jsp").forward(request, response);
        }
        else{
            request.setAttribute("result", productService.findAll());
            request.getRequestDispatcher ("list.jsp").forward(request, response);
        }
    }

    @Override
    protected void doPost(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
        System.out.println("a");
    }
}
