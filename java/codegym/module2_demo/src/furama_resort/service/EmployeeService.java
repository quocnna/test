package furama_resort.service;

import furama_resort.model.Employee;

import java.util.List;

public interface EmployeeService extends BaseService<Employee> {
    List<Employee> search(String name);
    List<Employee> sort();
}
