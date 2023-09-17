package service;

import model.AuthenticPhone;
import model.Phone;
import util.CommonUtil;
import util.FileUtil;
import util.ConstantUtil.PATH;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;
import java.util.stream.Collectors;

public class AuthenticPhoneService implements PhoneService {
    private List<Phone> authenticPhoneList = new ArrayList<>();

    public AuthenticPhoneService(){
        authenticPhoneList = mapFiletoPhoneList();
    }

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

        FileUtil.write(authenticPhoneList, PATH.AUTHENTIC, false);
    }

    @Override
    public void delete(int id) {
        authenticPhoneList.removeIf(e -> e.getId() == id);
        FileUtil.write(authenticPhoneList, PATH.AUTHENTIC, false);
    }

    @Override
    public List<Phone> findByName(String name) {
        return authenticPhoneList.stream().filter(e -> e.getName().contains(name)).collect(Collectors.toList());
    }

    @Override
    public List<Phone> sortByPrice() {
        return authenticPhoneList.stream().sorted(Comparator.comparingDouble(Phone::getPrice)).collect(Collectors.toList());
    }

    private List<Phone> mapFiletoPhoneList(){
        List<Phone> phones = new ArrayList<>();

        List<String> lines = FileUtil.read(PATH.AUTHENTIC);
        for (String line : lines){
            String[] tmp = line.split(",");
            int id = Integer.parseInt(tmp[0]);
            String name = tmp[1];
            Double price = Double.parseDouble(tmp[2]);
            String manufacturer = tmp[3];
            int granteeByYear = Integer.parseInt(tmp[4]);
            String granteeCode = tmp[5];
            phones.add(new AuthenticPhone(id, name, price, manufacturer, granteeByYear, granteeCode));
        }

        return phones;
    }
}
