package cg.m3.cs_customer_mvn.controller;

import cg.m3.cs_customer_mvn.model.Customer;
import cg.m3.cs_customer_mvn.service.CustomerService;
import jakarta.servlet.ServletException;
import jakarta.servlet.annotation.WebServlet;
import jakarta.servlet.http.HttpServlet;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;

import java.io.IOException;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;
import java.util.Optional;

@WebServlet(name = "CustomerController", value = "/customers")
public class CustomerController extends HttpServlet {
    private final CustomerService customerService = new CustomerService();

    public void doGet(HttpServletRequest req, HttpServletResponse resp) throws IOException, ServletException {
        String id = req.getParameter("id");
        if (id != null) {
            Optional<Customer> customer = customerService.findById(Integer.parseInt(id));
            customer.ifPresent(e -> req.setAttribute("customer", e));
            req.getRequestDispatcher("/customer/form.jsp").forward(req, resp);
        }

        String name = req.getParameter("q");
        List<Customer> customers = name != null ? customerService.find(name) : customerService.findAll();
        req.setAttribute("customers", customers);
        req.getRequestDispatcher("/customer/list.jsp").forward(req, resp);
    }

    protected void doPost(HttpServletRequest req, HttpServletResponse resp) throws IOException, ServletException {
        try {
            String tid = req.getParameter("id");
            int id = tid.isEmpty() ? 0 : Integer.parseInt(tid);

            String name = req.getParameter("name");
            Boolean gender= Boolean.parseBoolean(req.getParameter("gender"));
            String card = req.getParameter("card");
            String phone = req.getParameter("phone");
            String email = req.getParameter("email");
            int customerTypeId = Integer.parseInt(req.getParameter("customerTypeId"));

            String tBirthday = req.getParameter("birthday");
            Date birthDate = new SimpleDateFormat("yyyy-MM-dd").parse(tBirthday);

            customerService.save(new Customer(id, name, birthDate, gender,card,phone,email,"",customerTypeId,""));
            resp.sendRedirect("/customers");

        } catch (ParseException e) {
            throw new RuntimeException(e);
        }
    }

    @Override
    protected void doDelete(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
        customerService.delete(Integer.parseInt(req.getParameter("id")));
    }
}