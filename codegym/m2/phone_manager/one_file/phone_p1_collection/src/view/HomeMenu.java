package view;

import model.AuthenticPhone;
import model.HandPhone;
import model.Phone;
import service.PhoneService;

import java.util.Scanner;

public class HomeMenu {
    //region fields
    private final static Scanner scanner = new Scanner(System.in);
    private final static PhoneService phoneService = new PhoneService();
    //endregion

    public static void main(String[] args) {
        while (true) {
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
    private static void create() {
        save(false);
        System.out.println("Create successfully!!");
    }

    private static void update() {
        save(true);
        System.out.println("Updated successfully!!");
    }

    private static void delete() {
        int id = Integer.parseInt(input("Enter ID you want to delete"));
        phoneService.delete(id);
        System.out.println("Deleted successfully");
    }

    private static void display() {
        phoneService.findAll().forEach(System.out::println);
    }

    private static void searchByName() {
        String name = input("Enter Name you want to search");
        phoneService.findByName(name).forEach(System.out::println);
    }

    private static void sortByPrice() {
        phoneService.sortByPrice().forEach(System.out::println);
    }
    //endregion

    //region util function
    private static String input(String field) {
        System.out.print(field + ":");

        return scanner.nextLine();
    }

    private static void save(boolean isEdit) {
        int id = 0;
        int choice;

        if (isEdit) {
            id = Integer.parseInt(input("Enter ID you want to update"));
            choice = phoneService.isAuthentic(id) ? 1 : 2;
        } else {
            System.out.print("""
                    1. Authentic
                    2. Hand
                    """);
            choice = Integer.parseInt(input("Enter your choice"));
        }

        String name = input("Name");
        Double price = Double.parseDouble(input("Price"));
        String manufacture = input("Manufacture");

        Phone phone;
        if (choice == 1) {
            int granteeByYear = Integer.parseInt(input("Grantee By Year"));
            String granteeCode = input("Grantee Code");
            phone = new AuthenticPhone(id, name, price, manufacture, granteeByYear, granteeCode);
        } else {
            String country = input("Country");
            String status = input("Status");
            phone = new HandPhone(id, name, price, manufacture, country, status);
        }

        phoneService.save(phone);
    }
    //endregion
}