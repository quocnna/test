package service;

import model.Phone;

import java.util.List;

public interface PhoneService{
    List<Phone> findAll();
    void save(Phone phone);
    void delete(int id);
    List<Phone> findByName(String name);
    List<Phone> sortByPrice();
}
