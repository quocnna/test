package service;

import model.Manufacturer;
import model.Motor;
import model.Truck;
import util.ConstantUtil;
import util.FileHelper;

import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

public class MotorService implements VehicleService<Motor>{
    private ArrayList<Motor> motors = new ArrayList<>();
    private FileHelper fileHelper = new FileHelper();
    private ManufactureService manufactureService = new ManufactureService();

    public MotorService(){
        motors = mapToObject();
    }

    public List<Motor> findAll(){
        return motors;
    }

    public void create(Motor motor){
        motors.add(motor);
        // write file
        fileHelper.write(ConstantUtil.FilePath.Motor, motors,false);
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

        fileHelper.write(ConstantUtil.FilePath.Motor, motors,false);
    }

    public List<Motor> search(String driverPlate){
        return motors.stream().filter(e -> e.getDriverPlate().contains(driverPlate)).collect(Collectors.toList());
    }

    private ArrayList<Motor> mapToObject() {
        ArrayList<Motor> result = new ArrayList<>();

        List<String> lines = fileHelper.read(ConstantUtil.FilePath.Motor);
        for (String line : lines){
            if(!line.isEmpty()){
                String[] tmp = line.split(",");
                String driverPlate = tmp[0];
                Manufacturer manufacturer = manufactureService.findByName(tmp[1]);
                int yearOfManufacturer = Integer.parseInt(tmp[2]);
                String owner = tmp[3];
                Double wattage = Double.parseDouble(tmp[4]);
                Motor motor = new Motor(driverPlate, manufacturer, yearOfManufacturer, owner, wattage);
                result.add(motor);
            }
        }

        return result;
    }
}
