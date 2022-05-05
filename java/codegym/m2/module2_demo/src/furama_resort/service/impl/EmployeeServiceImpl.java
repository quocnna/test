package furama_resort.service.impl;

import furama_resort.model.Employee;
import furama_resort.service.EmployeeService;

import java.util.*;
import java.util.stream.Collectors;

public class EmployeeServiceImpl implements EmployeeService {
    private static List<Employee> employees= new ArrayList<>();

    @Override
    public void save(Employee employee) {
        if(employee.getId()> 0){
            int index= employees.indexOf(employee);
            employees.set(index, employee);
        }
        else {
            employee.setId(employees.size()+ 1);
            employees.add(employee);
        }
    }

    @Override
    public List<Employee> getAll() {
        return employees;
    }

    @Override
    public void delete(int id) {
//        int size= employees.size();
//        for (int i = 0; i < size; i++) {
//            if(employees.get(i).getId() == id){
//                employees.remove(i);
//            }
//        }

        employees.removeIf(e-> e.getId()==id);
    }

    @Override
    public Optional<Employee> get(int id) {
        return employees.stream().filter(e-> e.getId()== id).findFirst();

//        int size= employees.size();
//        for (int i = 0; i < size; i++) {
//            if(employees.get(i).getId() == id){
//                return Optional.of(employees.get(i));
//            }
//        }
//        return Optional.empty();
    }

    @Override
    public List<Employee> sort(){
//        Collections.sort(employees, Comparator.comparing(Employee::getFullName));
        Collections.sort(employees);
        return employees;
    }

    @Override
    public List<Employee> search(String name){
//        List<Employee> res= new ArrayList<>();
//
//        int size= employees.size();
//        for (int i = 0; i < size; i++) {
//            if(employees.get(i).getFullName().contains(name))
//            {
//                res.add(employees.get(i));
//            }
//        }

       return employees.stream().filter(e-> e.getFullName().contains(name)).collect(Collectors.toList());

//        return res;
    }
}
