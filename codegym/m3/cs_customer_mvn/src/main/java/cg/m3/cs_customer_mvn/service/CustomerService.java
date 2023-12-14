package cg.m3.cs_customer_mvn.service;

import cg.m3.cs_customer_mvn.model.Customer;
import cg.m3.cs_customer_mvn.repository.CustomerRepository;
import java.util.List;
import java.util.Optional;
import java.util.stream.Collectors;

public class CustomerService {
    private final CustomerRepository customerRepository = new CustomerRepository();

    public List<Customer> findAll() {
        return customerRepository.findAll();
    }

    public boolean save(Customer customer){
        return customerRepository.save(customer);
    }

    public Optional<Customer> findById(int id){
        return findAll().stream().filter(e -> e.id()==id).findFirst();
    }

    public List<Customer> find(String name){
        return findAll().stream().filter(e -> e.name().contains(name)).collect(Collectors.toList());
    }

    public boolean delete(int id){
        return customerRepository.delete(id);
    }
}
