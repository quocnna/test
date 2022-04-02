package furama_resort.controller;

import furama_resort.model.Employee;
import furama_resort.service.EmployeeService;
import furama_resort.service.impl.EmployeeServiceImpl;

import java.util.List;

public class EmployeeController {
    private EmployeeService employeeService= new EmployeeServiceImpl();

    public void save(Employee employee){
        employeeService.save(employee);
    }

    public List<Employee> getAll(){
        return employeeService.getAll();
    }

    public List<Employee> search(String name){
        return employeeService.search(name);
    }

    public List<Employee> sort(){
        return employeeService.sort();
    }
}
