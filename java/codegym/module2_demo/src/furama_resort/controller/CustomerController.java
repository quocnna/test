package furama_resort.controller;

import furama_resort.model.Customer;
import furama_resort.service.CustomerService;
import furama_resort.service.impl.CustomerServiceImpl;

public class CustomerController {
    private static CustomerService customerService= new CustomerServiceImpl();

    public void save(Customer customer){
        customerService.save(customer);
    }
}
