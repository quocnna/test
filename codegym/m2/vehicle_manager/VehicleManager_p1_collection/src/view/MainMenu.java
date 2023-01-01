package view;

import model.*;
import service.*;
import util.ConstantUtil;

import java.util.List;
import java.util.Scanner;

public class MainMenu {
    private static Scanner scanner = new Scanner(System.in);
    private static TruckService truckService = new TruckService();
    private static CarService carService = new CarService();
    private static MotorService motorService = new MotorService();

    public static void main(String[] args) {
        while (true) {
            System.out.println("--- Main Menu ---");
            System.out.println("1. Add\n2. Display\n3. Delete\n4. Search\n5. Exit");
            int choice = getChoice();

            switch (choice) {
                case 1 -> add();
                case 2 -> display();
                case 3 -> delete();
                case 4 -> search();
                case 5 -> System.exit(0);
            }
        }
    }

    private static void display() {
        System.out.println("1. Truck\n2. Car\n3. Motor");
        int choice = getChoice();
        switch (choice) {
            case 1 -> {
                List<Truck> trucks = truckService.findAll();
                for (int i = 0; i < trucks.size(); i++) {
                    System.out.println(trucks.get(i));
                }
            }
            case 2 -> {
                List<Car> cars = carService.findAll();
                for (int i = 0; i < cars.size(); i++) {
                    System.out.println(cars.get(i));
                }
            }
            case 3 -> {
                List<Motor> motors = motorService.findAll();
                for (int i = 0; i < motors.size(); i++) {
                    System.out.println(motors.get(i));
                }
            }
        }
    }

    private static void delete() {
        System.out.printf("Enter driver plate to delete:");
        String driverPlate = scanner.nextLine();

        if (truckService.findByDriverPlate(driverPlate)) {
            deleteWithConfirm(truckService, driverPlate);
        } else if (carService.findByDriverPlate(driverPlate)) {
            deleteWithConfirm(carService, driverPlate);
        } else if (motorService.findByDriverPlate(driverPlate)) {
            deleteWithConfirm(motorService, driverPlate);
        } else {
            System.out.println("No exists driver plate");
        }
    }

    private static void search() {
        System.out.printf("Enter driver plate to search:");
        String driverPlate = scanner.nextLine();

        List<Truck> trucks = truckService.search(driverPlate);
        List<Motor> motors = motorService.search(driverPlate);
        List<Car> cars = carService.search(driverPlate);

        if(trucks.size() == 0 && motors.size() == 0 && cars.size() == 0){
            System.out.println("No driver plate exists");
        }
        else {
            trucks.forEach(System.out::println);
            cars.forEach(System.out::println);
            motors.forEach(System.out::println);
        }
    }

    private static void add() {
        System.out.println("1. Truck\n2. Car\n3. Motor");
        int choice = getChoice();

        System.out.printf("Driver plate:");
        String plate = scanner.nextLine();

        System.out.println("--- List Manufacturer ---");
        ManufactureService manufactureService = new ManufactureService();
        List<Manufacturer> manufacturerList = manufactureService.findAll();
        for (int i = 0; i < manufacturerList.size(); i++) {
            System.out.println(manufacturerList.get(i));
        }

        System.out.printf("Enter manufacturer name:");
        String manufacturerName = scanner.nextLine();
        Manufacturer manufacturer = manufactureService.findByName(manufacturerName);

        System.out.printf("Year Of Manufacturer:");
        int yearOfManufacturer = Integer.parseInt(scanner.nextLine());

        System.out.printf("Owner:");
        String owner = scanner.nextLine();

        switch (choice) {
            case 1 -> {
                System.out.printf("Load:");
                Double load = Double.parseDouble(scanner.nextLine());

                Truck truck = new Truck(plate, manufacturer, yearOfManufacturer, owner, load);
                truckService.create(truck);
            }
            case 2 -> {
                System.out.printf("Number of seat:");
                int numberOfSeat = Integer.parseInt(scanner.nextLine());
                System.out.printf("Type of car:");
                String typeOfCar = scanner.nextLine();

                Car car = new Car(plate, manufacturer, yearOfManufacturer, owner, numberOfSeat, ConstantUtil.TypeOfCar.valueOf(typeOfCar));
                carService.create(car);
            }
            case 3 -> {
                System.out.printf("Wattage:");
                Double wattage = Double.parseDouble(scanner.nextLine());

                Motor motor = new Motor(plate, manufacturer, yearOfManufacturer, owner, wattage);
                motorService.create(motor);
            }
        }

        System.out.println("Created successfully!");
    }

    private static int getChoice() {
        System.out.printf("Enter your choice:");
        return Integer.parseInt(scanner.nextLine());
    }

    private static void deleteWithConfirm(VehicleService vehicleService, String driverPlate){
        System.out.println("Are you sure to delete this item:\n1. Yes\n2. No");
        int choice = getChoice();
        if(choice == 1){
            vehicleService.delete(driverPlate);
            System.out.println("Deleted successfully!");
        }
        else {
            main(null);
        }
    }

}
