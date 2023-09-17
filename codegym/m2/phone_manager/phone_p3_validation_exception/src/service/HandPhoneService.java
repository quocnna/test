package service;

import exception.NotFoundPhoneException;
import model.HandPhone;
import model.Phone;
import util.CommonUtil;
import util.ConstantUtil;
import util.FileUtil;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;
import java.util.stream.Collectors;

public class HandPhoneService implements PhoneService{
    private List<Phone> handPhoneList = new ArrayList<>();

    public HandPhoneService(){
        handPhoneList = mapFileToPhoneList();
    }

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

        FileUtil.write(handPhoneList, ConstantUtil.PATH.HAND, false);
    }

    @Override
    public void delete(int id) {
        if(handPhoneList.removeIf(e -> e.getId() == id)){
            FileUtil.write(handPhoneList, ConstantUtil.PATH.HAND, false);
            return;
        }

        throw new NotFoundPhoneException("This phone ID not found");
    }

    @Override
    public List<Phone> findByName(String name) {
        return handPhoneList.stream().filter(e -> e.getName().contains(name)).collect(Collectors.toList());
    }

    @Override
    public List<Phone> sortByPrice() {
        return handPhoneList.stream().sorted(Comparator.comparingDouble(Phone::getPrice)).collect(Collectors.toList());
    }

    private List<Phone> mapFileToPhoneList(){
        List<Phone> phones = new ArrayList<>();

        List<String> lines = FileUtil.read(ConstantUtil.PATH.HAND);
        for (String line : lines){
            String[] tmp = line.split(",");
            int id = Integer.parseInt(tmp[0]);
            String name = tmp[1];
            Double price = Double.parseDouble(tmp[2]);
            String manufacturer = tmp[3];
            String country = tmp[4];
            String status = tmp[5];
            phones.add(new HandPhone(id, name, price, manufacturer, country, status));
        }

        return phones;
    }
}
