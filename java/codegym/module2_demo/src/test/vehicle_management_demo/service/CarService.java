package test.vehicle_management_demo.service;

import test.vehicle_management_demo.common.AppConstant.*;
import test.vehicle_management_demo.common.CSVHelper;
import test.vehicle_management_demo.model.Car;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public class CarService {
    private List<Car> cars;
    private CSVHelper<Car> csvHelper= new CSVHelper<>();

    public CarService(){
        cars= getFromFile();
    }

    public List<Car> getAll(){
        return cars;
    }

    public void add(Car car){
        cars.add(car);
        csvHelper.write(Arrays.asList(car), true, Path.CAR);
    }

    public boolean delete(String numberPlate){
        boolean res= cars.removeIf(e-> e.getNumberPlate().equals(numberPlate));
        if(res) csvHelper.write(cars, false, Path.CAR);
        return res;
    }

    private List<Car> getFromFile(){
        List<Car> res= new ArrayList<>();
        List<String> lines= csvHelper.read(Path.CAR);
        if(lines.size()> 0){
            for (String line : lines){
                String[] tmp= line.split(",");
                String numberPlate= tmp[0];
                String manufacture= tmp[1];
                int year= Integer.parseInt(tmp[2]);
                String ower= tmp[3];
                int numberSeat= Integer.parseInt(tmp[4]);
                TypeCar typeCar= TypeCar.valueOf(tmp[5]);
                Car car= new Car(numberPlate, manufacture, year, ower, numberSeat, typeCar);
                res.add(car);
            }
        }

        return res;
    }
}
