package view;

import model.AuthenticPhone;
import model.HandPhone;
import model.Phone;
import service.AuthenticPhoneService;
import service.HandPhoneService;
import service.PhoneService;

import java.util.Scanner;

public class HomeMenu {
    //region fields
    private final static Scanner scanner = new Scanner(System.in);
    private final static PhoneService authenticPhoneService = new AuthenticPhoneService();
    private final static PhoneService handPhoneService = new HandPhoneService();
    //endregion

    public static void main(String[] args) {
        while (true){
            System.out.print("""
                === Phone Menu ===
                1. Create
                2. Update
                3. Delete
                4. Display
                5. Search by name
                6. Sort by price
                7. Exit
                """);

            int choice = Integer.parseInt(input("Enter your choice"));
            switch (choice) {
                case 1 -> create();
                case 2 -> update();
                case 3 -> delete();
                case 4 -> display();
                case 5 -> searchByName();
                case 6 -> sortByPrice();
                case 7 -> System.exit(0);
            }
        }
    }

    //region menu function
    private static void create(){
        save(false);
        System.out.println("Create successfully!!");
    }

    private static void update(){
        save(true);
        System.out.println("Updated successfully!!");
    }

    private static void delete(){
        int choice = choosePhone();
        int id = Integer.parseInt(input("Enter ID you want to delete"));

        if(choice == 1){
            authenticPhoneService.delete(id);
        }
        else{
            handPhoneService.delete(id);
        }

        System.out.println("Deleted successfully");
    }

    private static void display(){
        int choice = choosePhone();

        if(choice == 1){
            authenticPhoneService.findAll().forEach(System.out::println);
        }
        else {
            handPhoneService.findAll().forEach(System.out::println);
        }
    }

    private static void searchByName(){
        int choice = choosePhone();
        String name = input("Enter Name you want to search");

        if(choice == 1){
            authenticPhoneService.findByName(name).forEach(System.out::println);
        }
        else {
            handPhoneService.findByName(name).forEach(System.out::println);
        }
    }

    private static void sortByPrice(){
        int choice = choosePhone();

        if(choice == 1){
            authenticPhoneService.sortByPrice().forEach(System.out::println);
        }
        else {
            handPhoneService.sortByPrice().forEach(System.out::println);
        }
    }
    //endregion

    //region util function
    private static String input(String field){
        System.out.print(field + ":");

        return scanner.nextLine();
    }

    private static int choose() {
        System.out.print("Enter your choice:");

        return Integer.parseInt(scanner.nextLine());
    }

    private static int choosePhone(){
        System.out.print("""
                1. Authentic
                2. Hand
                """);

        return choose();
    }

    private static void save(boolean isEdit) {
        int id = 0;
        int choice = choosePhone();

        if (isEdit) {
            id = Integer.parseInt(input("Enter ID you want to update"));
        }

        String name = input("Name");
        Double price = Double.parseDouble(input("Price"));
        String manufacture = input("Manufacture");

        if (choice == 1) {
            int granteeByYear = Integer.parseInt(input("Grantee By Year"));
            String granteeCode = input("Grantee Code");
            authenticPhoneService.save(new AuthenticPhone(id, name, price, manufacture, granteeByYear, granteeCode));
        } else {
            String country = input("Country");
            String status = input("Status");
            handPhoneService.save(new HandPhone(id, name, price, manufacture, country, status));
        }
    }
    //endregion
}
