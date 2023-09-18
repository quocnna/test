package service;

import model.Phone;
import util.CommonUtil;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;
import java.util.stream.Collectors;

public class HandPhoneService implements PhoneService{
    private List<Phone> handPhoneList = new ArrayList<>();

    @Override
    public List<Phone> findAll() {
        return handPhoneList;
    }

    @Override
    public void save(Phone handPhone) {
        if(handPhone.getId() > 0){
            int index = handPhoneList.indexOf(handPhone);
            handPhoneList.set(index, handPhone);
        }
        else{
            handPhone.setId(CommonUtil.getLastId(handPhoneList) + 1);
            handPhoneList.add(handPhone);
        }
    }

    @Override
    public void delete(int id) {
        handPhoneList.removeIf(e -> e.getId() == id);
    }

    @Override
    public List<Phone> findByName(String name) {
        return handPhoneList.stream().filter(e -> e.getName().contains(name)).collect(Collectors.toList());
    }

    @Override
    public List<Phone> sortByPrice() {
        return handPhoneList.stream().sorted(Comparator.comparingDouble(Phone::getPrice)).collect(Collectors.toList());
    }
}
