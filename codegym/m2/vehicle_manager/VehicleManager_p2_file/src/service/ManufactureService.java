package service;

import model.Manufacturer;

import java.util.ArrayList;
import java.util.List;

public class ManufactureService {
    private ArrayList<Manufacturer> manufacturerList = new ArrayList<>();

    public ManufactureService(){
        manufacturerList.add(new Manufacturer("HD", "Honda", "Japan"));
        manufacturerList.add(new Manufacturer("KI", "KIA", "Korea"));
        manufacturerList.add(new Manufacturer("AU", "Audi", "Germany"));
    }

    public List<Manufacturer> findAll(){
        return manufacturerList;
    }

    public Manufacturer findByName(String name){
        for (int i = 0; i < manufacturerList.size(); i++) {
            if(manufacturerList.get(i).getName().equals(name)) {
                return manufacturerList.get(i);
            }
        }

        return null;
    }
}
