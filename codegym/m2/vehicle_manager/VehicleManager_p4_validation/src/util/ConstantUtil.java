package util;

import model.Manufacturer;

public interface ConstantUtil {
    enum TypeOfCar{
        TOURIST,
        COACH
    }

    interface FilePath{
        String Car = "src/data/car.csv";
        String Motor = "src/data/motor.csv";
        String Truck = "src/data/truck.csv";
    }

    interface DriverPlateRegex{
        String CAR_COACH= "\\d{2}B-\\d{3}.\\d{2}";
        String CAR_TOURIST= "\\d{2}A-\\d{3}.\\d{2}";
        String MOTOR= "\\d{2}-[A-Z]\\w-\\d{3}.\\d{2}";
        String TRUNK= "\\d{2}\\w-\\d{3}.\\d{2}";
    }
}
