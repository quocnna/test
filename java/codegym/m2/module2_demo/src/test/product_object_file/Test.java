package test.product_object_file;

import java.util.List;
import java.util.Scanner;

public class Test {
    private static Scanner scanner= new Scanner(System.in);
    private static ProductService productService= new ProductService();

    public static void main(String[] args) {
        menu();
    }

    private static void menu(){
        int choice;
        do{
            System.out.println("1. Create Product");
            System.out.println("2. Display Product");
            System.out.println("3. Search By Name");
            System.out.println("4. Search From Price");
            System.out.println("5. Edit Product");
            System.out.println("6. Sort By Price");
            System.out.print("Choice a number: ");
            choice= Integer.parseInt(scanner.nextLine());
        }while (choice <=0 || choice>6);

        switch (choice){
            case 1:
                create();
                break;
            case 2:
//                productService.getAll().forEach(System.out::println);
              List<Product> products=   productService.getAll();
                int size= products.size();
                for (int i = 0; i < size; i++) {
                    System.out.println(products.get(i));
                }
                break;
            case 3:
                searchByName();
                break;
            case 4:
                searchFromPrice();
                break;
            case 5:
                update();
                break;
            case 6:
                sort();
                break;
        }

        backToMainMenu();
    }

    private static void sort(){
        productService.sort().forEach(System.out::println);

    }

    private static void update(){
        productService.getAll().forEach(System.out::println);
        System.out.print("Choice id to edit: ");
        int choice = Integer.parseInt(scanner.nextLine());
        System.out.print("Code to edit: ");
        String code= scanner.nextLine();
        System.out.print("Name to edit: ");
        String name= scanner.nextLine();
        System.out.print("Band to edit: ");
        String brand= scanner.nextLine();
        System.out.print("Price to edit: ");
        double price= Double.parseDouble(scanner.nextLine());
        System.out.print("Description to edit: ");
        String des= scanner.nextLine();

        Product product= new Product(choice, code, name, brand, price, des);
        productService.update(product);
        System.out.println("Update successful");
    }

    private static void searchFromPrice(){
        System.out.print("Input price to search: ");
        double price= Double.parseDouble(scanner.nextLine());
        List<Product> products= productService.searchFromPrice(price);
        if(products.size()> 0){
            products.forEach(System.out::println);
        }
        else {
            System.out.println("Not found");
        }
    }

    private static void searchByName(){
        System.out.print("Input name to search: ");
        String name= scanner.nextLine();
        List<Product> products= productService.searchByName(name);
        if(products.size()> 0){
            products.forEach(System.out::println);
        }
        else {
            System.out.println("Not found");
        }
    }

    private static void backToMainMenu(){
        System.out.println("Do you back to main menu (Y/N): ");
        String answer= scanner.nextLine();
        if(answer.equalsIgnoreCase("y")) menu();
        else System.exit(0);
    }

    private static void create(){
        System.out.print("Code: ");
        String code= scanner.nextLine();
        System.out.print("Name: ");
        String name= scanner.nextLine();
        System.out.print("Brand: ");
        String brand= scanner.nextLine();
        System.out.print("Price: ");
        double price= Double.parseDouble(scanner.nextLine());
        System.out.print("Descrition: ");
        String des= scanner.nextLine();

        Product product= new Product(code, name, brand, price, des);
        productService.add(product);
        System.out.println("Create successful");
    }
}
