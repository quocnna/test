package service;

import exception.NotFoundPhoneException;
import model.AuthenticPhone;
import model.HandPhone;
import model.Phone;
import util.FileUtil;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;
import java.util.stream.Collectors;

public class PhoneService {
    private static final String PATH_PHONE = "src/data/phone.csv";
    private List<Phone> phoneList = new ArrayList<>();

    public PhoneService(){
        phoneList = mapFiletoPhoneList();
    }

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

        FileUtil.write(phoneList, PATH_PHONE, false);
    }

    public void delete(int id){
        if(phoneList.removeIf(e -> e.getId() == id)){
            FileUtil.write(phoneList, PATH_PHONE, false);
            return;
        }

        throw new NotFoundPhoneException("This phone ID not found");
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

    private List<Phone> mapFiletoPhoneList(){
        List<Phone> phones = new ArrayList<>();

        List<String> lines = FileUtil.read(PATH_PHONE);
        for (String line : lines){
            String[] tmp = line.split(",");
            int id = Integer.parseInt(tmp[1]);
            String name = tmp[2];
            Double price = Double.parseDouble(tmp[3]);
            String manufacturer = tmp[4];

            if(tmp[0].equals("Authentic")){
                int granteeByYear = Integer.parseInt(tmp[5]);
                String granteeCode = tmp[6];
                phones.add(new AuthenticPhone(id, name, price, manufacturer, granteeByYear, granteeCode));
            }
            else {
                String country = tmp[5];
                String status = tmp[6];
                phones.add(new HandPhone(id, name, price, manufacturer, country, status));
            }
        }

        return phones;
    }
}
