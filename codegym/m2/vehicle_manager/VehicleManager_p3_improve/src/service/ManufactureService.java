package service;

import model.Manufacturer;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

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

    public Optional<Manufacturer> findByName(String name){
        return manufacturerList.stream().filter(e -> e.getName().equalsIgnoreCase(name)).findFirst();
    }
}
