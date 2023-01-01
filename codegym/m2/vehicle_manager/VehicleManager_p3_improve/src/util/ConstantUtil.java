package util;

import model.Manufacturer;

public interface ConstantUtil {
    enum TypeOfCar{
        Tourist,
        Coach
    }

    interface FilePath{
        String Car = "src/data/car.csv";
        String Motor = "src/data/motor.csv";
        String Truck = "src/data/truck.csv";
    }
}
