import model.AuthenticProduct;
import model.HandProduct;
import model.Product;
import service.AuthenticService;
import service.HandService;
import service.ProductService;

import java.util.List;
import java.util.Scanner;

public class ProductView {
    private static Scanner scanner = new Scanner(System.in);
    private static ProductService authenticService = new AuthenticService();
    private static ProductService handService = new HandService();

    public static void main(String[] args) {
        while (true) {
            System.out.println("--- Product Menu---");
            System.out.println("1. Create");
            System.out.println("2. Display");
            System.out.println("3. Delete");
            System.out.println("4. Search");
            System.out.println("5. Exit");

            int choose = yourChoice();

            switch (choose) {
                case 1:
                    create();
                    break;
                case 2:
                    display();
                    break;
                case 3:
                    delete();
                    break;
                case 4:
                    search();
                    break;
                case 5:
                    System.exit(0);
            }
        }
    }

    private static void search() {
        int choose = choiceProductType();
        if(choose == 1) {
            System.out.print("Enter name to search:");
            String name = scanner.nextLine();
            List<AuthenticProduct> products= authenticService.searchByName(name);
            displayAuthenticProducts(products);
        }
        else {
            System.out.print("Enter name to search:");
            String name = scanner.nextLine();
            List<HandProduct> products= handService.searchByName(name);
            displayHandProducts(products);
        }
    }

    private static void display() {
        int choose = choiceProductType();
        if (choose == 1) {
            displayAuthenticProducts(authenticService.findAll());
        } else {
            displayHandProducts(handService.findAll());
        }
    }

    private static void delete() {
        int choose = choiceProductType();
        if (choose == 1) {
            displayAuthenticProducts(authenticService.findAll());
            System.out.print("Enter id to delete:");
            int id = Integer.parseInt(scanner.nextLine());
            authenticService.delete(id);
        } else {
            displayHandProducts(handService.findAll());
            System.out.print("Enter id to delete:");
            int id = Integer.parseInt(scanner.nextLine());
            handService.delete(id);
        }
        System.out.println("Deleted successfully");
    }

    private static void create() {
        int choose = choiceProductType();

        System.out.print("Name:");
        String name = scanner.nextLine();
        System.out.print("Price:");
        double price = Double.parseDouble(scanner.nextLine());
        System.out.print("Manufacturer:");
        String manufacturer = scanner.nextLine();

        Product product;
        if (choose == 1) {
            System.out.print("Grantee by year:");
            int granteeByYear = Integer.parseInt(scanner.nextLine());
            product = new AuthenticProduct(0, name, price, manufacturer, granteeByYear);
            authenticService.create(product);

        } else {
            System.out.print("Country:");
            String country = scanner.nextLine();
            System.out.print("Status:");
            String status = scanner.nextLine();
            product = new HandProduct(0, name, price, manufacturer, country, status);
            handService.create(product);
        }

        System.out.println("Created successfully");
    }

    private static int yourChoice() {
        System.out.print("Enter your choice:");
        return Integer.parseInt(scanner.nextLine());
    }

    private static int choiceProductType() {
        System.out.println("Choice product type:");
        System.out.println("1. Authentic");
        System.out.println("2. Hand");
        return yourChoice();
    }

    private static void displayAuthenticProducts(List<AuthenticProduct> authenticProducts) {
        for (AuthenticProduct p : authenticProducts) {
            System.out.printf("Authentic Product: id= %s, name =%s, price =%s, manufacturer = %s, granteeByYear = %s\n"
                    , p.getId(), p.getName(), p.getPrice(), p.getManufacturer(), p.getGranteeByYear());
        }
    }

    private static void displayHandProducts(List<HandProduct> handProducts) {
        for (HandProduct p : handProducts) {
            System.out.printf("Hand Product: id= %s, name =%s, price =%s, manufacturer = %s, country = %s, status = %s\n"
                    , p.getId(), p.getName(), p.getPrice(), p.getManufacturer(), p.getCountry(), p.getStatus());
        }
    }
}
