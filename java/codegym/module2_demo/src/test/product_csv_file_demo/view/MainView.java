package test.product_csv_file_demo.view;

import test.product_csv_file_demo.exception.NotFoundProductException;
import test.product_csv_file_demo.model.AuthenticProduct;
import test.product_csv_file_demo.model.HandProduct;
import test.product_csv_file_demo.service.AuthenticProductService;
import test.product_csv_file_demo.service.HandProductService;

import java.util.List;
import java.util.Scanner;

public class MainView {
    private static Scanner scanner= new Scanner(System.in);
    private static HandProductService handProductService= new HandProductService();
    private static AuthenticProductService authenticProductService= new AuthenticProductService();

    public static void main(String[] args) {
        displayMenu();
    }

    private static void displayMenu(){
        while (true){
            System.out.println("--- Product Menu: ---\n1. Add new product\n2. Delete a product\n3. Display list product" +
                    "\n4. Search by name\n5. Exit");

            int choose;
            do{
                System.out.printf("Input your choose: ");
                choose= Integer.parseInt(scanner.nextLine());
            }while (choose<=0 || choose>4);

            switch (choose){
                case 1:
                    add();
                    break;
                case 2:
                    delete();
                    break;
                case 3:
                    display();
                    break;
                case 4:
                    search();
                    break;
                case 5:
                    System.exit(0);
            }
        }
    }

    private static void add(){
        int choose= inputYourChoose();

        System.out.printf("Name: ");
        String name= scanner.nextLine();
        System.out.printf("Price: ");
        double price= Double.parseDouble(scanner.nextLine());
        System.out.printf("Manufacture: ");
        String manufacture= scanner.nextLine();

        if(choose==1){
            System.out.printf("Country: ");
            String country= scanner.nextLine();
            System.out.printf("Status: ");
            String status= scanner.nextLine();
            HandProduct handProduct= new HandProduct(0, name, price, manufacture, country, status);
            handProductService.add(handProduct);
        }
        else {
            System.out.printf("Guarantee period by year: ");
            int year= Integer.parseInt(scanner.nextLine());
            AuthenticProduct authenticProduct= new AuthenticProduct(0, name, price, manufacture, year);
            authenticProductService.add(authenticProduct);
        }
        System.out.println("Created successful!");
    }

    private static void delete(){
        int choose= inputYourChoose();
        deleteWithException(choose==1);
    }

    private static void display(){
        int choose= inputYourChoose();
        if(choose==1)displayHandProduct(handProductService.getAll());
        else displayAuthenticdProduct(authenticProductService.getAll());
    }

    private static void search(){
        int choose= inputYourChoose();
        System.out.printf("Input name to search: ");
        String name= scanner.nextLine();
        if(choose==1)displayHandProduct(handProductService.search(name));
        else displayAuthenticdProduct(authenticProductService.search(name));
    }

    private static void displayHandProduct(List<HandProduct> handProducts){
        if(handProducts.size()> 0){
            System.out.printf("%-10s %-20s %-20s %-20s %-20s %-20s%n", "ID", "NAME", "PRICE", "MANUFACTURE", "COUNTRY", "STATUS");
            System.out.println("-----------------------------------------------------------------------------------------------------------");
            for (HandProduct h: handProducts){
                System.out.printf("%-10d %-20s %-20.2f %-20s %-20s %-20s%n", h.getId(), h.getName(), h.getPrice(), h.getManufacture(), h.getCountry(), h.getStatus());
            }
        }
        else System.out.println("Not Found");
    }

    private static void displayAuthenticdProduct(List<AuthenticProduct> authenticProducts){
        if(authenticProducts.size()>0 ){
            System.out.printf("%-10s %-20s %-20s %-20s %-25s%n", "ID", "NAME", "PRICE", "MANUFACTURE", "GUARANTEE PERIOD YEAR");
            System.out.println("------------------------------------------------------------------------------------------------------");
            for (AuthenticProduct a: authenticProducts){
                System.out.printf("%-10d %-20s %-20.2f %-20s %-25s%n", a.getId(), a.getName(), a.getPrice(), a.getManufacture(), a.getGuarenteePeriodByYear());
            }
        }
        else System.out.println("Not Found");
    }

    private static int inputYourChoose(){
        System.out.printf("1. Hand products\n2. Authentic products\n");
        int choose;
        do{
            System.out.printf("Input your choose: ");
            choose= Integer.parseInt(scanner.nextLine());
        }while (choose<=0 || choose>2);

        return choose;
    }

    private static void deleteWithException(boolean isHandProduct){
        if(isHandProduct) displayHandProduct(handProductService.getAll());
        else displayAuthenticdProduct(authenticProductService.getAll());

        boolean check= true;
        do{
            System.out.printf(check? "Input id to delete: ": "Input again correct id from list to delete: ");
            int id= Integer.parseInt(scanner.nextLine());
            System.out.printf("Are you sure to delete (Y/N): ");
            String answer= scanner.nextLine();
            if(answer.equalsIgnoreCase("Y")){
                try{
                    if(isHandProduct) handProductService.delete(id);
                    else authenticProductService.delete(id);
                    System.out.println("Deleted successful!");
                    check= true;
                }
                catch (NotFoundProductException e){
                    e.printStackTrace();
                    check= false;
                }
            }
            else displayMenu();
        }while (!check);
    }
}
