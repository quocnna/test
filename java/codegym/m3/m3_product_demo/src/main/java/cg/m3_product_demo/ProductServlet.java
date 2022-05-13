package cg.m3_product_demo;

import cg.m3_product_demo.model.Product;
import cg.m3_product_demo.service.ProductService;
import cg.m3_product_demo.util.CommonUtil;

import java.io.*;
import java.util.List;
import java.util.stream.Collectors;
import javax.servlet.ServletException;
import javax.servlet.http.*;

public class ProductServlet extends HttpServlet {
    private ProductService productService = new ProductService();

    public void doGet(HttpServletRequest req, HttpServletResponse res) throws IOException, ServletException {
        String id = req.getParameter("id");
        String q = req.getParameter("q");

        if(id != null){
            req.setAttribute("product", productService.findById(Integer.parseInt(id)));
            req.getRequestDispatcher("form.jsp").forward(req, res);
        }
        else{
            List<Product> products= productService.findAll();
            req.setAttribute("result", q!= null ? products.stream().filter(e -> e.getName().contains(q)).collect(Collectors.toList()) : products);
            req.getRequestDispatcher ("list.jsp").forward(req, res);
        }
    }

    @Override
    protected void doPost(HttpServletRequest req, HttpServletResponse res) throws ServletException, IOException {
        int id = CommonUtil.toInt(req.getParameter("id"));
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

        res.sendRedirect("/");
    }
}
