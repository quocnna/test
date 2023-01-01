package service;

import model.Motor;
import model.Truck;

import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

public class MotorService implements VehicleService<Motor>{
    private ArrayList<Motor> motors = new ArrayList<>();

    public List<Motor> findAll(){
        return motors;
    }

    public void create(Motor motor){
        motors.add(motor);
    }

    public boolean findByDriverPlate(String driverPlate){
        for (int i = 0; i < motors.size(); i++) {
//            if(trucks.get(i).getDriverPlate().equals(driverPlate)){
//                return true;
//            }

            return motors.get(i).getDriverPlate().equals(driverPlate);
        }

        return false;
    }

    public void delete(String driverPlate){
        for (int i = 0; i < motors.size(); i++) {
            if(motors.get(i).getDriverPlate().equals(driverPlate)){
                motors.remove(i);
                break;
            }
        }
    }

    public List<Motor> search(String driverPlate){
        return motors.stream().filter(e -> e.getDriverPlate().contains(driverPlate)).collect(Collectors.toList());
    }
}
