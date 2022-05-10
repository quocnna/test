package cg.m3_product_demo;

import cg.m3_product_demo.model.Product;
import cg.m3_product_demo.service.ProductService;

import java.io.*;
import javax.servlet.ServletException;
import javax.servlet.http.*;
import javax.servlet.annotation.*;

@WebServlet(name = "productServlet", value = "/product")
public class ProductServlet extends HttpServlet {
    private ProductService productService = new ProductService();

    public void doGet(HttpServletRequest req, HttpServletResponse res) throws IOException, ServletException {
        String id = req.getParameter("id");
        if(id != null){
            req.setAttribute("product", productService.findById(Integer.parseInt(id)));
            req.getRequestDispatcher("form.jsp").forward(req, res);
        }
        else{
            req.setAttribute("result", productService.findAll());
            req.getRequestDispatcher ("list.jsp").forward(req, res);
        }
    }

    @Override
    protected void doPost(HttpServletRequest req, HttpServletResponse res) throws ServletException, IOException {
        int id = Integer.parseInt(req.getParameter("id"));
        String name = req.getParameter("name");
        if(name != null){
            Double price = Double.parseDouble(req.getParameter("price"));
            int quantity = Integer.parseInt(req.getParameter("quantity"));
            String color = req.getParameter("color");
            String description = req.getParameter("description");
            int categoryId = Integer.parseInt(req.getParameter("category"));
            productService.save(new Product(id, name, price, quantity, color, description, categoryId, ""));
        }
        else{
            productService.delete(id);
        }

        res.sendRedirect("/product");
    }
}
