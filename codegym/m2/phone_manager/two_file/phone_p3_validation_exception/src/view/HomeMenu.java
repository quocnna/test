package view;

import exception.NotFoundPhoneException;
import model.AuthenticPhone;
import model.HandPhone;
import service.AuthenticPhoneService;
import service.HandPhoneService;
import service.PhoneService;
import util.ValidationUtil;

public class HomeMenu {
    //region fields
    private final static PhoneService authenticPhoneService = new AuthenticPhoneService();
    private final static PhoneService handPhoneService = new HandPhoneService();
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

            int choice = Integer.parseInt(ValidationUtil.inputWithOutEmpty("Enter your choice"));
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
        int choice = choosePhone();

        System.out.print("Enter ID you want to delete:");
        boolean isExists;

        do {
            try {
                int id = Integer.parseInt(ValidationUtil.getScanner().nextLine());
                if (choice == 1) {
                    authenticPhoneService.delete(id);
                } else if (choice == 2) {
                    handPhoneService.delete(id);
                }

                isExists = true;
                System.out.println("Deleted successfully");
            } catch (NumberFormatException | NotFoundPhoneException e) {
                System.err.print(e.getMessage() + "\nPlease input ID again:");
                isExists = false;
            }
        } while (!isExists);
    }

    private static void display() {
        authenticPhoneService.findAll().forEach(System.out::println);

        handPhoneService.findAll().forEach(System.out::println);
    }

    private static void searchByName() {
        int choice = choosePhone();
        String name = ValidationUtil.inputWithOutEmpty("Enter Name you want to search");

        if (choice == 1) {
            authenticPhoneService.findByName(name).forEach(System.out::println);
        } else if (choice == 2) {
            handPhoneService.findByName(name).forEach(System.out::println);
        }
    }

    private static void sortByPrice() {
        int choice = choosePhone();

        if (choice == 1) {
            authenticPhoneService.sortByPrice().forEach(System.out::println);
        } else if (choice == 2) {
            handPhoneService.sortByPrice().forEach(System.out::println);
        }
    }
    //endregion

    //region util function
    private static int choose() {
        System.out.print("Enter your choice:");

        return Integer.parseInt(ValidationUtil.getScanner().nextLine());
    }

    private static int choosePhone() {
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
            id = Integer.parseInt(ValidationUtil.inputWithOutEmpty("Enter ID you want to update"));
        }

        String name = ValidationUtil.inputName();
        Double price = ValidationUtil.inputPriceGreaterThan50();
        String manufacture = ValidationUtil.inputWithOutEmpty("Manufacture");

        if (choice == 1) {
            int granteeByYear = Integer.parseInt(ValidationUtil.inputWithOutEmpty("Grantee By Year"));
            String granteeCode = ValidationUtil.inputWithOutEmpty("Grantee Code");
            authenticPhoneService.save(new AuthenticPhone(id, name, price, manufacture, granteeByYear, granteeCode));
        } else {
            String country = ValidationUtil.inputWithOutEmpty("Country");
            String status = ValidationUtil.inputWithOutEmpty("Status");
            handPhoneService.save(new HandPhone(id, name, price, manufacture, country, status));
        }
    }
    //endregion
}
