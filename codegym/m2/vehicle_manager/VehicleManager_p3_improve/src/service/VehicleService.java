package service;

import exception.NotFoundVehicleException;
import model.Car;
import model.Manufacturer;
import model.Motor;
import model.Truck;
import model.Vehicle;
import util.ConstantUtil.TypeOfCar;
import util.ConstantUtil.FilePath;
import util.FileHelper;

import java.util.ArrayList;
import java.util.List;
import java.util.stream.Stream;
import java.util.Collection;

public class VehicleService {
    //region fields
    private List<Truck> trucks;
    private List<Car> cars;
    private List<Motor> motors;

    private FileHelper<Motor> fileMotor = new FileHelper<>();
    private FileHelper<Truck> fileTruck = new FileHelper();
    private FileHelper<Car> fileCar = new FileHelper<>();
    private ManufactureService manufactureService = new ManufactureService();
    //endregion

    //region constructor

    public VehicleService() {
        motors = toMotors();
        cars = toCars();
        trucks = toTrucks();
    }
    //endregion

    //region public methods
    public List<Car> getAllCar() {
        return cars;
    }

    public List<Motor> getAllMotor() {
        return motors;
    }

    public List<Truck> getAllTruck() {
        return trucks;
    }

    public void create(Vehicle vehicle) {
        if (vehicle instanceof Motor m) {
            motors.add(m);
            fileMotor.write(FilePath.Motor, motors, false);
        } else if (vehicle instanceof Car c) {
            cars.add(c);
            fileCar.write(FilePath.Car, cars, false);
        } else {
            trucks.add((Truck) vehicle);
            fileTruck.write(FilePath.Truck, trucks, false);
        }
    }

    public boolean findByDriverPlate(String driverPlate) {
        return Stream.of(cars, motors, trucks).flatMap(Collection::stream).anyMatch(e -> e.getDriverPlate().equals(driverPlate));
    }

    public boolean delete(String driverPlate) throws NotFoundVehicleException {
        if (cars.removeIf(e -> e.getDriverPlate().equals(driverPlate))) {
            fileCar.write(FilePath.Car, cars, false);
            return true;
        }

        if (motors.removeIf(e -> e.getDriverPlate().equals(driverPlate))) {
            fileMotor.write(FilePath.Motor, motors, false);
            return true;
        }

        if (trucks.removeIf(e -> e.getDriverPlate().equals(driverPlate))) {
            fileTruck.write(FilePath.Truck, trucks, false);
            return true;
        }

        throw new NotFoundVehicleException("Driver plate is not exists");
    }

    public List<? extends Vehicle> search(String driverPlate) {
        return Stream.of(cars, motors, trucks).flatMap(Collection::stream).filter(e -> e.getDriverPlate().contains(driverPlate)).toList();
    }
    //endregion

    //region private methods
    private List<Car> toCars() {
        List<Car> res = new ArrayList<>();
        List<String> lines = fileCar.read(FilePath.Car);

        for (String line : lines) {
            if (!line.isEmpty()) {
                String[] tmp = line.split(",");
                String driverPlate = tmp[0];
                Manufacturer manufacturer = manufactureService.findByName(tmp[1]).orElseGet(() -> new Manufacturer("Unknown"));
                int year = Integer.parseInt(tmp[2]);
                String owner = tmp[3];
                int numberSeat = Integer.parseInt(tmp[4]);
                String typeCar = tmp[5];
                Car car = new Car(driverPlate, manufacturer, year, owner, numberSeat, TypeOfCar.valueOf(typeCar));
                res.add(car);
            }
        }

        return res;
    }

    private List<Truck> toTrucks() {
        List<Truck> res = new ArrayList<>();
        List<String> lines = fileCar.read(FilePath.Truck);

        for (String line : lines) {
            if (!line.isEmpty()) {
                String[] tmp = line.split(",");
                String driverPlate = tmp[0];
                Manufacturer manufacturer = manufactureService.findByName(tmp[1]).orElseGet(() -> new Manufacturer("Unknown"));
                int year = Integer.parseInt(tmp[2]);
                String owner = tmp[3];
                double load = Double.parseDouble(tmp[4]);
                Truck truck = new Truck(driverPlate, manufacturer, year, owner, load);
                res.add(truck);
            }
        }

        return res;
    }

    private List<Motor> toMotors() {
        List<Motor> res = new ArrayList<>();
        List<String> lines = fileCar.read(FilePath.Motor);

        for (String line : lines) {
            if (!line.isEmpty()) {
                String[] tmp = line.split(",");
                String numberPlate = tmp[0];
                Manufacturer manufacturer = manufactureService.findByName(tmp[1]).orElseGet(() -> new Manufacturer("Unknown"));
                int year = Integer.parseInt(tmp[2]);
                String owner = tmp[3];
                double wattage = Double.parseDouble(tmp[4]);
                Motor motor = new Motor(numberPlate, manufacturer, year, owner, wattage);
                res.add(motor);
            }
        }

        return res;
    }
    //endregion
}
