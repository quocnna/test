package view;

import exception.NotFoundVehicleException;
import model.*;
import service.*;
import util.ConstantUtil.TypeOfCar;
import util.Validation;

import java.util.Arrays;
import java.util.List;
import java.util.Scanner;

import static util.CommonUtil.getChoice;
import static util.CommonUtil.inputToDouble;
import static util.CommonUtil.inputToEnum;
import static util.CommonUtil.inputToInteger;
import static util.CommonUtil.inputWithOutEmpty;

public class MainMenu {
    //region fields
    private static Scanner scanner = new Scanner(System.in);
    private static VehicleService vehicleService = new VehicleService();
    //endregion

    //region menu feature
    public static void main(String[] args) {
        while (true) {
            System.out.println("--- Main Menu ---");
            System.out.println("1. Add\n2. Display\n3. Delete\n4. Search\n5. Exit");
            int choice = getChoice(1,5);

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
        int choice = getChoice(1,3);

        switch (choice) {
            case 1 -> displayTruck(vehicleService.getAllTruck());
            case 2 -> displayCar(vehicleService.getAllCar());
            case 3 -> displayMotor(vehicleService.getAllMotor());
        }
    }

    private static void delete() {
        boolean check = false;

        do {
            String driverPlate = inputWithOutEmpty(check ? "Enter driver plate again" : "Enter driver plate to delete");

            try {
                //region delete with confirm
                //                if(vehicleService.findByDriverPlate(driverPlate)){
//                    System.out.println("Are you sure to delete:\n1. Yes\n2. No");
//                    int choice = getChoice();
//
//                    if(choice == 1){
//                        vehicleService.delete(driverPlate);
//                        System.out.println("Deleted successfully");
//                    }
//                    else {
//                        main(null);
//                    }
//                }
//                else {
//                    throw new NotFoundVehicleException("Driver plate is not exists");
//                }
                //endregion

                vehicleService.delete(driverPlate);
                System.out.println("Deleted successfully");
                check = false;
            }
            catch (NotFoundVehicleException e){
                System.out.println(e.getMessage());
                check = true;
            }
        }while (check);
    }

    private static void search() {
        String driverPlate = inputWithOutEmpty("Enter driver plate to search");

        List<? extends Vehicle> vehicles = vehicleService.search(driverPlate);

        if(vehicles.size() == 0) {
            System.out.println("No driver plate exists");
        }
        else {
            vehicles.forEach(e -> {
                if(e instanceof Car c){
                    displayCar(List.of(c));
                }
                else if(e instanceof Motor m){
                    displayMotor(List.of(m));
                }
                else {
                    displayTruck(List.of((Truck)e));
                }
            });
        }
    }

    private static void add() {
        System.out.println("1. Truck\n2. Car\n3. Motor");
        int choice = getChoice(1,3);

        //region validate driver plate
        String plate = "";
        switch (choice){
            case 1 -> {
                plate = getPlateWithoutExists();

                while (!Validation.isTruck(plate)){
                    plate = inputWithOutEmpty("Invalid format. Please input again");
                }
            }
            case 3 -> {
                plate = getPlateWithoutExists();

                while (!Validation.isMotor(plate)){
                    plate = inputWithOutEmpty("Invalid format. Please input again");
                }
            }
        }
        //endregion


        //region validation manufacturer
        System.out.println("--- List Manufacturer ---");
        ManufactureService manufactureService = new ManufactureService();
        List<Manufacturer> manufacturerList = manufactureService.findAll();
        for (int i = 0; i < manufacturerList.size(); i++) {
            System.out.println(manufacturerList.get(i));
        }

        String manufacturerName = inputWithOutEmpty("Enter manufacturer name");
        Manufacturer manufacturer = manufactureService.findByName(manufacturerName).orElseGet(() -> new Manufacturer("Unknown"));

        int yearOfManufacturer = inputToInteger("Year Of Manufacturer");
        //endregion

        String owner = inputWithOutEmpty("Owner");

        Vehicle vehicle = null;

        switch (choice) {
            case 1 -> {
                Double load = inputToDouble("Load");
                vehicle = new Truck(plate, manufacturer, yearOfManufacturer, owner, load);
            }
            case 2 -> {
                int numberOfSeat = inputToInteger("Number of seat");

                TypeOfCar typeOfCar = inputToEnum(TypeOfCar.class, "Type of car");

                plate = getPlateWithoutExists();
                while (!Validation.isCarPlate(plate, typeOfCar.name().equalsIgnoreCase("TOURIST") )){
                    plate = inputWithOutEmpty("Invalid format. Please input again");
                }

                vehicle = new Car(plate, manufacturer, yearOfManufacturer, owner, numberOfSeat, typeOfCar);
            }
            case 3 -> {
                Double wattage = inputToDouble("Wattage");
                vehicle = new Motor(plate, manufacturer, yearOfManufacturer, owner, wattage);
            }
        }

        vehicleService.create(vehicle);
        System.out.println("Created successfully!");
    }
    //endregion

    //region private function

    private static String getPlateWithoutExists(){
        String plate = null;
        do{
            plate = inputWithOutEmpty(plate != null ? "Driver plate exists. Please input again:" : "Driver plate");
        }while (vehicleService.findByDriverPlate(plate));

        return plate;
    }

    private static void displayTruck(List<Truck> trucks){

        if(trucks.size() > 0){
            System.out.printf("%-20s %-20s %-30s %-20s %20s %n", "DRIVER PLATE", "MANUFACTURER", "YEAR OF MANUFACTURER", "OWNER", "LOAD");
            System.out.println("-------------------------------------------------------------------------------------------------------------------------");

            for(Truck t: trucks){
                System.out.printf("%-20s %-20s %-30s %-20s %20s %n", t.getDriverPlate(), t.getManufacturer().getName(), t.getYearOfManufacturer(), t.getOwner(), t.getLoad());
            }
        }
        else {
            System.out.println("Empty list...");
        }
    }

    private static void displayMotor(List<Motor> motors){
        if(motors.size() > 0){
            System.out.printf("%-20s %-20s %-30s %-20s %20s %n", "DRIVER PLATE", "MANUFACTURER", "YEAR OF MANUFACTURER", "OWNER", "WATTAGE");
            System.out.println("------------------------------------------------------------------------------------------------------------------------");

            for(Motor t: motors){
                System.out.printf("%-20s %-20s %-30s %-20s %20s %n", t.getDriverPlate(), t.getManufacturer().getName(), t.getYearOfManufacturer(), t.getOwner(), t.getWattage());
            }
        }
        else {
            System.out.println("Empty list...");
        }
    }

    private static void displayCar(List<Car> cars){
        if(cars.size() > 0){
            System.out.printf("%-20s %-20s %-30s %-20s %25s %25s %n", "DRIVER PLATE", "MANUFACTURER", "YEAR OF MANUFACTURER", "OWNER", "NUMBER OF SEAT", "TYPE OF CAR");
            System.out.println("-------------------------------------------------------------------------------------------------------------------------------------------------");

            for(Car t: cars){
                System.out.printf("%-20s %-20s %-30s %-20s %25s %25s %n", t.getDriverPlate(), t.getManufacturer().getName(), t.getYearOfManufacturer(), t.getOwner(), t.getNumberOfSeat(), t.getTypeOfCar().name());
            }
        }
        else{
            System.out.println("Empty list...");
        }
    }
    //endregion
}
