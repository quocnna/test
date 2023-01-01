package service;

import model.Truck;

import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

public class TruckService implements VehicleService<Truck>{
    private ArrayList<Truck> trucks = new ArrayList<>();

    public List<Truck> findAll(){
        return trucks;
    }

    public void create(Truck truck){
        trucks.add(truck);
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
    }

    public List<Truck> search(String driverPlate){
        return trucks.stream().filter(e -> e.getDriverPlate().contains(driverPlate)).collect(Collectors.toList());
    }
}
