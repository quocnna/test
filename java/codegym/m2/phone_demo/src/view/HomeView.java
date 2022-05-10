package view;

import model.BaseEntity;
import service.GeneralService;
import util.CommonUtil;
import util.ConstantUtil;

import java.lang.reflect.Field;
import java.util.HashMap;
import java.util.List;
import java.util.Scanner;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class HomeView {
    private static GeneralService generalService = new GeneralService();
    private static Scanner scanner = new Scanner(System.in);

    public static void main(String[] args) {
        while (true){
            System.out.println("--- Menu Home---");
            System.out.println("1. Create");
            System.out.println("2. Display");
            System.out.println("3. Delete");
            System.out.println("4. Search");
            System.out.println("5. Exit");

            int choice = getChoice();

            switch (choice) {
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

    private static void create(){
        System.out.println("Choose items to create:");
        for (int i = 0; i < ConstantUtil.CHILD_ENTITY.length; i++){
            System.out.printf("%s. %s\n", i+1, ConstantUtil.CHILD_ENTITY[i]);
        }

        int choose = getChoice();

        for (int i = 0; i < ConstantUtil.CHILD_ENTITY.length; i++){
            if(choose == i+ 1){
//                HashMap<String, String>  a= CommonUtil.inputFields(ConstantUtil.CHILD_ENTITY[i]);
                CommonUtil.createInstance(ConstantUtil.CHILD_ENTITY[i] , CommonUtil.inputFields(ConstantUtil.CHILD_ENTITY[i]));
            }
        }



//
//        System.out.print("Name:");
//        String name = scanner.nextLine();
//        System.out.print("Price:");
//        double price = Double.parseDouble(scanner.nextLine());
//        System.out.print("Manufacturer:");
//        String manufacturer = scanner.nextLine();


//
//        Product product;
//        if (choose == 1) {
//            System.out.print("Grantee by year:");
//            int granteeByYear = Integer.parseInt(scanner.nextLine());
//            product = new AuthenticProduct(0, name, price, manufacturer, granteeByYear);
//
//        } else {
//            System.out.print("Country:");
//            String country = scanner.nextLine();
//            System.out.print("Status:");
//            String status = scanner.nextLine();
//            product = new HandProduct(0, name, price, manufacturer, country, status);
//
//        }
//
//        productService.create(product);
//        System.out.println("Created successfully");
    }

    private static void display(){

    }

    private static void delete(){
        display();
        System.out.print("Enter id to delete:");
        int id = Integer.parseInt(scanner.nextLine());
        generalService.delete(id);
        System.out.println("Deleted successfully");
    }

    private static void search(){

    }

    private static int getChoice() {
        System.out.print("Enter your choice:");
        return Integer.parseInt(scanner.nextLine());
    }
}
