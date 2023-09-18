package service;

import model.AuthenticPhone;
import model.Phone;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;
import java.util.stream.Collectors;

public class PhoneService {
    private List<Phone> phoneList = new ArrayList<>();

    public List<Phone> findAll(){
        return phoneList;
    }

    public void save(Phone phone){
        if(phone.getId() > 0){
            int index = phoneList.indexOf(phone);
            phoneList.set(index, phone);
        }
        else {
            int lastId = 0;
            if(!phoneList.isEmpty()){
                lastId = phoneList.get(phoneList.size() - 1).getId();
            }
            phone.setId(lastId + 1);
            phoneList.add(phone);
        }
    }

    public void delete(int id){
        phoneList.removeIf(e -> e.getId() == id);
    }

    public List<Phone> findByName(String name) {
        return phoneList.stream().filter(e -> e.getName().contains(name)).collect(Collectors.toList());
    }

    public List<Phone> sortByPrice() {
        return phoneList.stream().sorted(Comparator.comparingDouble(Phone::getPrice)).collect(Collectors.toList());
    }

    public boolean isAuthentic(int id){
        return phoneList.stream().filter(e -> e.getId() == id).findFirst().get() instanceof AuthenticPhone;
    }
}
