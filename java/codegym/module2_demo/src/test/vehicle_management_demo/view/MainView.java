package test.vehicle_management_demo.view;

import test.vehicle_management_demo.common.AppConstant.*;
import test.vehicle_management_demo.common.CSVHelper;
import test.vehicle_management_demo.exception.NotFoundVehicleException;
import test.vehicle_management_demo.model.Car;
import test.vehicle_management_demo.model.Motor;
import test.vehicle_management_demo.model.Truck;
import test.vehicle_management_demo.service.CarService;
import test.vehicle_management_demo.service.MotorService;
import test.vehicle_management_demo.service.TruckService;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Scanner;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class MainView {
    private static Scanner scanner = new Scanner(System.in);
    private static CarService carService = new CarService();
    private static MotorService motorService = new MotorService();
    private static TruckService truckService = new TruckService();
    private static CSVHelper<String> csvHelper= new CSVHelper<>();
    private static Pattern pattern;
    private static Matcher matcher;

    public static void main(String[] args) {
        displayMainMenu();
    }

    private static void displayMainMenu(){
        System.out.println("--- Vehicle Management ---");
        System.out.println("1. Add new\n2. Display\n3. Delete\n4. Exit");

        int choice = 0;
        do {
            System.out.printf(choice == 0 ? "Input your choose: " : "Just input value from 1 to 4: ");
            choice = Integer.parseInt(scanner.nextLine());
        } while (choice <= 0 || choice > 4);

        switch (choice) {
            case 1:
                add();
                break;
            case 2:
                display();
                break;
            case 3:
                delete();
                break;
            case 4:
                System.exit(0);
                break;
        }

        backToMainMenu();
    }
    private static void add() {
        System.out.println("1. Add truck\n2. Add car\n3. Add motor");

        int choice = 0;
        do {
            System.out.printf(choice == 0 ? "Input your choose: " : "Just input value from 1 to 3: ");
            choice = Integer.parseInt(scanner.nextLine());
        } while (choice <= 0 || choice > 3);

        switch (choice) {
            case 1:
                boolean isTruckPlate;
                String truckPlate= "";
                do{
                    System.out.printf(truckPlate.isEmpty()? "Number plate: ": "Input number plate with format XXC-XXX.XX (C:0-9): ");
                    truckPlate = scanner.nextLine();
                    isTruckPlate= checkTruckPlate(truckPlate);
                }while (!isTruckPlate);
                List<String> tmpTruck= inputCommon();

                System.out.printf("Load: ");
                double load = Double.parseDouble(scanner.nextLine());
                Truck truck = new Truck(truckPlate, tmpTruck.get(0), Integer.parseInt(tmpTruck.get(1)), tmpTruck.get(2), load);
                truckService.add(truck);
                break;
            case 2:
                boolean isTypeCar;
                String typeCar= "";
                do{
                    System.out.printf(typeCar.isEmpty()? "Type Car: ": "Just input type car is BUS or TOURIST: ");
                    typeCar = scanner.nextLine();
                    isTypeCar= checkTypeCar(typeCar);
                }while (!isTypeCar);


                boolean isCarPlate;
                String carPlate;
                do{
                    boolean tmp= typeCar.equalsIgnoreCase("BUS");
                    System.out.printf(tmp? "Number BUS plate: ": "Number TOURIST plate: ");
                    carPlate = scanner.nextLine();
                    isCarPlate= checkCarPlate(carPlate, tmp);
                }while (!isCarPlate);
                List<String> tmpCar= inputCommon();
                System.out.printf("Number seat: ");
                int numberSeat = Integer.parseInt(scanner.nextLine());
                Car car = new Car(carPlate, tmpCar.get(0), Integer.parseInt(tmpCar.get(1)), tmpCar.get(2), numberSeat, TypeCar.valueOf(typeCar.toUpperCase()));
                carService.add(car);
                break;
            case 3:
                boolean isMotorPlate;
                String motorPlate= "";
                do{
                    System.out.printf(motorPlate.isEmpty()? "Number plate: ": "Input number plate with format XX-YZ-XXX.XX: ");
                    motorPlate = scanner.nextLine();
                    isMotorPlate= checkMotorPlate(motorPlate);
                }while (!isMotorPlate);
                List<String> tmpMotor= inputCommon();
                System.out.printf("Wattage: ");
                int wattage = Integer.parseInt(scanner.nextLine());
                Motor motor = new Motor(motorPlate, tmpMotor.get(0), Integer.parseInt(tmpMotor.get(1)), tmpMotor.get(2), wattage);
                motorService.add(motor);
                break;
        }

        System.out.println("Created successful");
    }

    private static void delete() {
        boolean tmp = false;
        String numberPlate= "";
        do {
            try {
                System.out.printf(numberPlate.isEmpty()? "Input number plate to delete: ": "Input again number plate to delete: ");
                numberPlate = scanner.nextLine();
                System.out.printf("Do you sure to delete (Y/N): ");
                String anwser= scanner.nextLine();
                if(anwser.equalsIgnoreCase("Y")){
                    tmp = carService.delete(numberPlate);
                    if (!tmp) tmp = motorService.delete(numberPlate);
                    if (!tmp) tmp = truckService.delete(numberPlate);
                    if (!tmp) throw new NotFoundVehicleException("Number plate is not exists");
                    System.out.println("Deleted successful");
                }
                else {
                    tmp= true;
                }

            } catch (NotFoundVehicleException e) {
                e.printStackTrace();
            }
        } while (!tmp);
    }

    private static void display() {
        System.out.println("1. Display truck\n2. Display car\n3. Display motor");
        int choice;
        do {
            System.out.printf("Input your choose: ");
            choice = Integer.parseInt(scanner.nextLine());
        } while (choice <= 0 || choice > 3);

        switch (choice) {
            case 1:
                List<Truck> trucks = truckService.getAll();
                System.out.println("----- Display trucks -----");
                System.out.println("--------------------------------------------------------------------------------------------");
                System.out.printf("%-20s %-20s %-10s %-20s %-20s%n", "NUMBER PLATE", "MANUFACTURE", "YEAR", "OWNER", "LOAD");
                System.out.println("--------------------------------------------------------------------------------------------");
                for (Truck truck : trucks) {
                    System.out.format("%-20s %-20s %-10d %-20s %-20.1f%n", truck.getNumberPlate(), truck.getManufacture(), truck.getYear(), truck.getOwer(), truck.getLoad());
                }
                System.out.println("--------------------------------------------------------------------------------------------");
                break;
            case 2:
                List<Car> cars = carService.getAll();
                System.out.println("----- Display car -----");
                System.out.println("----------------------------------------------------------------------------------------------------------");
                System.out.printf("%-20s %-20s %-10s %-20s %-20s %-20s%n", "NUMBER PLATE", "MANUFACTURE", "YEAR", "OWNER", "NUMBER SEAT", "TYPE CAR");
                System.out.println("----------------------------------------------------------------------------------------------------------");
                for (Car car : cars) {
                    System.out.format("%-20s %-20s %-10d %-20s %-20d %-20s%n", car.getNumberPlate(), car.getManufacture(), car.getYear(), car.getOwer(), car.getNumberSeat(), car.getTypeCar());
                }
                System.out.println("----------------------------------------------------------------------------------------------------------");
                break;
            case 3:
                List<Motor> motors = motorService.getAll();
                System.out.println("----- Display motor -----");
                System.out.println("--------------------------------------------------------------------------------------------");
                System.out.printf("%-20s %-20s %-10s %-20s %-20s%n", "NUMBER PLATE", "MANUFACTURE", "YEAR", "OWNER", "WATTAGE");
                System.out.println("--------------------------------------------------------------------------------------------");
                for (Motor motor : motors) {
                    System.out.format("%-20s %-20s %-10d %-20s %-20d%n", motor.getNumberPlate(), motor.getManufacture(), motor.getYear(), motor.getOwer(), motor.getWattage());
                }
                System.out.println("--------------------------------------------------------------------------------------------");
                break;
        }
    }

    private static void backToMainMenu(){
        System.out.printf("Do you back to main menu (Y/N): ");
        String answer= scanner.nextLine();
        if(answer.equalsIgnoreCase("Y")) displayMainMenu();
        else System.exit(0);
    }

    // or input first
    private static List<String> inputCommon(){
        List<String> res= new ArrayList<>();
        List<String> manufactures= csvHelper.read(Path.MANUFACTURE);
        List<String> nameManufactures= new ArrayList<>();
        System.out.println("--- List Manufacture: ---");
        System.out.println("-------------------------------------------------------");
        System.out.printf("%-20s %-20s %-20s%n", "ID", "Name", "Country");
        System.out.println("-------------------------------------------------------");
        for (String st: manufactures){
            String[] tmp= st.split(",");
            nameManufactures.add(tmp[1]);
            System.out.printf("%-20s %-20s %-20s%n", tmp[0], tmp[1], tmp[2]);
        }

        boolean isManufacture;
        String manufacture= "";
        do{
            System.out.printf(manufacture.isEmpty()? "Choice manufacture name from list: ": "Please choice manufacture name again: ");
            manufacture = scanner.nextLine();
            isManufacture= nameManufactures.contains(manufacture);
        }while (!isManufacture);

        System.out.printf("Year: ");
        int year = Integer.parseInt(scanner.nextLine());
        System.out.printf("Owner: ");
        String owner = scanner.nextLine();
        res.add(manufacture);
        res.add(String.valueOf(year));
        res.add(owner);
        return res;
    }

    private static boolean checkTruckPlate(String plate){
        if(plate.isEmpty())
            return false;

        if(truckService.getAll().stream().anyMatch(e-> e.getNumberPlate().equals(pattern)))
            return false;

        pattern= Pattern.compile(PlateRegex.TRUCK);
        matcher= pattern.matcher(plate);
        return matcher.matches();
    }

    private static boolean checkCarPlate(String plate, boolean isBUS){
        if(plate.isEmpty())
            return false;

        if(carService.getAll().stream().anyMatch(e-> e.getNumberPlate().equals(pattern)))
            return false;

        pattern= Pattern.compile(isBUS? PlateRegex.BUS: PlateRegex.TOURIST);
        matcher= pattern.matcher(plate);
        return matcher.matches();
    }

    private static boolean checkMotorPlate(String plate){
        if(plate.isEmpty())
            return false;

        if(motorService.getAll().stream().anyMatch(e-> e.getNumberPlate().equals(pattern)))
            return false;

        pattern= Pattern.compile(PlateRegex.MOTOR);
        matcher= pattern.matcher(plate);
        return matcher.matches();
    }

    private static boolean checkTypeCar(String typeCar){
        if(typeCar.isEmpty())
            return false;

        return Arrays.stream(TypeCar.values()).anyMatch(e-> e.name().equalsIgnoreCase(typeCar));
    }
}
