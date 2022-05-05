package test.vehicle_management_demo.service;

import test.vehicle_management_demo.common.AppConstant.*;
import test.vehicle_management_demo.common.CSVHelper;
import test.vehicle_management_demo.exception.NotFoundVehicleException;
import test.vehicle_management_demo.model.Car;
import test.vehicle_management_demo.model.Motor;
import test.vehicle_management_demo.model.Truck;
import test.vehicle_management_demo.model.Vehicle;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public class VehicleService {
    private CarService carService= new CarService();
    private MotorService motorService= new MotorService();
    private TruckService truckService= new TruckService();

    public void delete(String numberPlate) throws NotFoundVehicleException {
        boolean tmp= carService.delete(numberPlate);
        if(!tmp) tmp= motorService.delete(numberPlate);
        if(!tmp) tmp= truckService.delete(numberPlate);
        if(!tmp) throw new NotFoundVehicleException("a");
    }
//    private List<Vehicle> vehicels= new ArrayList<>();
//    CSVHelper<Car> carCSVHelper= new CSVHelper<>();
//    CSVHelper<Motor> motorCSVHelper= new CSVHelper<>();
//    CSVHelper<Truck> truckCSVHelper= new CSVHelper<>();
//
//    public List<Vehicle> getAll(){
//        return vehicels;
//    }
//
//    public void add(Vehicle vehicel){
//        vehicels.add(vehicel);
//        if(vehicel instanceof Car){
//            carCSVHelper.write(Arrays.asList((Car) vehicel), true, Path.CAR);
//        }
//        else if(vehicel instanceof Motor){
//            motorCSVHelper.write(Arrays.asList((Motor) vehicel), true, Path.MOTOR);
//        }
//        else {
//            truckCSVHelper.write(Arrays.asList((Truck) vehicel), true, Path.TRUCK);
//        }
//    }
//
//    public void delete(String numberPlate){
//        vehicels.removeIf(e-> e.getNumberPlate().equals(numberPlate));
//        if(vehicel instanceof Car){
//            carCSVHelper.write(Arrays.asList((Car) vehicel), true, Path.CAR);
//        }
//        else if(vehicel instanceof Motor){
//            motorCSVHelper.write(Arrays.asList((Motor) vehicel), true, Path.MOTOR);
//        }
//        else {
//            truckCSVHelper.write(Arrays.asList((Truck) vehicel), true, Path.TRUCK);
//        }
//    }
}
