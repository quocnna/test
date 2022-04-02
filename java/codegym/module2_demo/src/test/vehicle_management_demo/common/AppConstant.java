package test.vehicle_management_demo.common;

public interface AppConstant {
    enum TypeCar{
        TOURIST,
        BUS
    }

    interface Path{
        String CAR= "src/test/vehicle_management_demo/data/car.csv";
        String TRUCK= "src/test/vehicle_management_demo/data/truck.csv";
        String MOTOR= "src/test/vehicle_management_demo/data/motor.csv";
        String MANUFACTURE= "src/test/vehicle_management_demo/data/manufacture.csv";
    }

    interface PlateRegex{
        String TOURIST= "\\d{2}A-\\d{3}\\.\\d{2}";
        String BUS= "\\d{2}B-\\d{3}\\.\\d{2}";
        String MOTOR= "\\d{2}-[A-Z][0-9A-Z]-\\d{3}\\.\\d{2}";
        String TRUCK= "\\d{2}C-\\d{3}\\.\\d{2}";
    }
}
