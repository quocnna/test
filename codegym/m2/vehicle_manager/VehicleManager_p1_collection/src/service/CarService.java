package service;

import model.Car;
import model.Truck;

import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

public class CarService implements VehicleService<Car>{
    private ArrayList<Car> cars = new ArrayList<>();

    public List<Car> findAll(){
        return cars;
    }

    public void create(Car car){
        cars.add(car);
    }

    public boolean findByDriverPlate(String driverPlate){
        for (int i = 0; i < cars.size(); i++) {
//            if(trucks.get(i).getDriverPlate().equals(driverPlate)){
//                return true;
//            }

            return cars.get(i).getDriverPlate().equals(driverPlate);
        }

        return false;
    }

    public void delete(String driverPlate){
        for (int i = 0; i < cars.size(); i++) {
            if(cars.get(i).getDriverPlate().equals(driverPlate)){
                cars.remove(i);
                break;
            }
        }
    }

    public List<Car> search(String driverPlate){
        return cars.stream().filter(e -> e.getDriverPlate().contains(driverPlate)).collect(Collectors.toList());
    }
}
