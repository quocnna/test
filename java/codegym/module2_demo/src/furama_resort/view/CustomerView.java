package furama_resort.view;

import furama_resort.controller.CustomerController;
import furama_resort.model.Customer;
import furama_resort.util.CommonUtil;
import furama_resort.util.ConstantUtil;

public class CustomerView {
    private static CustomerController customerController= new CustomerController();

    static void customerManager(){
        System.out.println("--- Customer View: ---" +
                "1. Display list customer\n" +
                "2. Add new customer\n" +
                "3. Edit customer\n" +
                "4. Return main menu\n");

        int choice= 0;
        do{
            System.out.print("Input your choice: ");
            choice= Integer.parseInt(CommonUtil.getScanner().nextLine());
        }while (choice<=0 || choice> 4);

        switch (choice){
            case 1:
                display();
                break;
            case 2:
                create();
                break;
            case 3:
                edit();
                break;
            case 4:
                HomeView.displayMainMenu();
        }
    }

    private static void display(){

    }

    private static void create(){
//        id, String code, String fullName, String birthday, ConstantUtil.Gender gender, String phone, String email, String address, TypeCustomer typeCustomer
        System.out.printf("Code: ");
        String code= CommonUtil.getScanner().nextLine();
        System.out.printf("FullName: ");
        String fullName= CommonUtil.getScanner().nextLine();
        System.out.printf("Birthday: ");
        String birthday= CommonUtil.getScanner().nextLine();
        Customer customer= new Customer(0, code, fullName, birthday, ConstantUtil.Gender.MALE, "123", "email", "address", ConstantUtil.TypeCustomer.DIAMOND);
        customerController.save(customer);
    }

    private static void edit(){
        System.out.printf("Input customer id to edit: ");
        int id= Integer.parseInt(CommonUtil.getScanner().nextLine());
        System.out.printf("Code: ");
        String code= CommonUtil.getScanner().nextLine();
        System.out.printf("FullName: ");
        String fullName= CommonUtil.getScanner().nextLine();
        System.out.printf("Birthday: ");
        String birthday= CommonUtil.getScanner().nextLine();
        Customer customer= new Customer(id, code, fullName, birthday, ConstantUtil.Gender.MALE, "123", "email", "address", ConstantUtil.TypeCustomer.DIAMOND);
        customerController.save(customer);
    }
}
