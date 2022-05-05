package furama_resort.view;

import furama_resort.util.CommonUtil;

public class HomeView {
    public static void main(String[] args) {
        displayMainMenu();
    }

    static void displayMainMenu(){
//        EmployeeView.employeeManger();
        CustomerView.customerManager();

//        System.out.println("Welcome to Furama Resort.\n" +
//                "1. Employee Management\n" +
//                "2. Customer Management\n" +
//                "3. Facility Management \n" +
//                "4. Booking Management\n" +
//                "5. Promotion Management\n" +
//                "6. Exit");
//
//        int choice= 0;
//        do{
//            System.out.print(choice==0? "Input your choice: ": "Please input value from 1 to 6: ");
//            choice= Integer.parseInt(CommonUtil.getScanner().nextLine());
//        }while (choice<=0 || choice >6);
//
//        switch (choice){
//            case 1:
//                EmployeeView.employeeManger();
//                break;
//            case 2:
//                CustomerView.customerManager();
//                break;
//            case 3:
//                FacilityView.facilityManger();
//                break;
//            case 4:
//                BookingView.bookingManeger();
//                break;
//            case 5:
//                PromotionView.promotionManager();
//                break;
//            case 6:
//                System.exit(0);
//                break;
//        }

        backToMainMenu();
    }

    static private void backToMainMenu(){
        System.out.print("Do you to back main menu (Y/N): ");
        String anwser= CommonUtil.getScanner().nextLine();
        if(anwser.equalsIgnoreCase("Y")) displayMainMenu();
        else System.exit(0);
    }
}
