package service;

import model.Manufacturer;
import model.Truck;
import util.ConstantUtil;
import util.FileHelper;

import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

public class TruckService implements VehicleService<Truck>{
    private ArrayList<Truck> trucks = new ArrayList<>();
    private FileHelper fileHelper = new FileHelper();
    private ManufactureService manufactureService = new ManufactureService();

    public TruckService(){
        trucks = mapToObject();
    }

    public List<Truck> findAll(){
        return trucks;
    }

    public void create(Truck truck){
        trucks.add(truck);
        // write file
        fileHelper.write(ConstantUtil.FilePath.Truck, trucks,false);
    }

    public boolean findByDriverPlate(String driverPlate){
        for (int i = 0; i < trucks.size(); i++) {
//            if(trucks.get(i).getDriverPlate().equals(driverPlate)){
//                return true;
//            }

            return trucks.get(i).getDriverPlate().equals(driverPlate);
        }

        return false;
    }

    public void delete(String driverPlate){
        for (int i = 0; i < trucks.size(); i++) {
            if(trucks.get(i).getDriverPlate().equals(driverPlate)){
                trucks.remove(i);
                break;
            }
        }

        fileHelper.write(ConstantUtil.FilePath.Truck, trucks,false);
    }

    public List<Truck> search(String driverPlate){
        return trucks.stream().filter(e -> e.getDriverPlate().contains(driverPlate)).collect(Collectors.toList());
    }

    private ArrayList<Truck> mapToObject() {
        ArrayList<Truck> result = new ArrayList<>();

        List<String> lines = fileHelper.read(ConstantUtil.FilePath.Truck);
        for (String line : lines){
            if(!line.isEmpty()){
                String[] tmp = line.split(",");
                String driverPlate = tmp[0];
                Manufacturer manufacturer = manufactureService.findByName(tmp[1]);
                int yearOfManufacturer = Integer.parseInt(tmp[2]);
                String owner = tmp[3];
                Double load = Double.parseDouble(tmp[4]);
                Truck truck = new Truck(driverPlate, manufacturer, yearOfManufacturer, owner, load);
                result.add(truck);
            }
        }

        return result;
    }
}
