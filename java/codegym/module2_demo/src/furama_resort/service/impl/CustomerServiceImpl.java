package furama_resort.service.impl;

import furama_resort.model.Customer;
import furama_resort.service.CustomerService;

import java.util.LinkedList;
import java.util.List;
import java.util.Optional;

public class CustomerServiceImpl implements CustomerService {
    private LinkedList<Customer> customers= new LinkedList<>();

    @Override
    public void save(Customer customer) {
        if(customer.getId()== 0){
            // create
            customer.setId(customers.size() + 1);
            customers.add(customer);
        }
        else {
            // update
            int size= customers.size();
            for (int i = 0; i < size; i++) {
                if(customer.getId() == customers.get(i).getId()){
                    customers.set(i, customer);
                    break;
                }
            }
        }
    }

    @Override
    public List<Customer> getAll() {
        return null;
    }

    @Override
    public void delete(int id) {

    }

    @Override
    public Optional<Customer> get(int id) {
        return Optional.empty();
    }
}
