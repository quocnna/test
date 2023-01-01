package service;

import model.Car;
import model.Manufacturer;
import util.ConstantUtil;
import util.ConstantUtil.TypeOfCar;
import util.FileHelper;

import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

public class CarService implements VehicleService<Car>{
    private ArrayList<Car> cars = new ArrayList<>();
    private FileHelper fileHelper= new FileHelper();

    private ManufactureService manufactureService = new ManufactureService();

    public CarService() {
        cars = mapToObject();
    }

    public List<Car> findAll(){
        return cars;
    }

    public void create(Car car){
        cars.add(car);
        // write file
        fileHelper.write(ConstantUtil.FilePath.Car, cars,false);
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

        fileHelper.write(ConstantUtil.FilePath.Car, cars,false);
    }

    public List<Car> search(String driverPlate){
        return cars.stream().filter(e -> e.getDriverPlate().contains(driverPlate)).collect(Collectors.toList());
    }

    private ArrayList<Car> mapToObject() {
        ArrayList<Car> result = new ArrayList<>();

        List<String> lines = fileHelper.read(ConstantUtil.FilePath.Car);
        for (String line : lines){
            if(!line.isEmpty()){
                String[] tmp = line.split(",");
                String driverPlate = tmp[0];
                Manufacturer manufacturer = manufactureService.findByName(tmp[1]);
                int yearOfManufacturer = Integer.parseInt(tmp[2]);
                String owner = tmp[3];
                int numberOfSeat = Integer.parseInt(tmp[4]);
                TypeOfCar typeOfCar = TypeOfCar.valueOf(tmp[5]);
                Car car = new Car(driverPlate, manufacturer, yearOfManufacturer, owner, numberOfSeat, typeOfCar);
                result.add(car);
            }
        }

        return result;
    }
}
