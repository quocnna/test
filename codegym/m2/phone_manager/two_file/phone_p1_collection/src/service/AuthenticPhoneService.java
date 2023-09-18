package service;

import model.Phone;
import util.CommonUtil;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;
import java.util.stream.Collectors;

public class AuthenticPhoneService implements PhoneService {
    public List<Phone> authenticPhoneList = new ArrayList<>();

    @Override
    public List<Phone> findAll() {
        return authenticPhoneList;
    }

    @Override
    public void save(Phone authenticPhone) {
        if(authenticPhone.getId() > 0){
            int index = authenticPhoneList.indexOf(authenticPhone);
            authenticPhoneList.set(index, authenticPhone);
        }
        else{
            authenticPhone.setId(CommonUtil.getLastId(authenticPhoneList) + 1);
            authenticPhoneList.add(authenticPhone);
        }
    }

    @Override
    public void delete(int id) {
        authenticPhoneList.removeIf(e -> e.getId() == id);
    }

    @Override
    public List<Phone> findByName(String name) {
        return authenticPhoneList.stream().filter(e -> e.getName().contains(name)).collect(Collectors.toList());
    }

    @Override
    public List<Phone> sortByPrice() {
        return authenticPhoneList.stream().sorted(Comparator.comparingDouble(Phone::getPrice)).collect(Collectors.toList());
    }
}
