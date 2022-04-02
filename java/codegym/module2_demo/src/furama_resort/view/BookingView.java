package furama_resort.view;

import furama_resort.util.CommonUtil;

public class BookingView {
    static void bookingManeger(){
        System.out.println("1. Display list booking\n" +
                "2. Add new booking\n" +
                "3. Create new constracts\n" +
                "4. Display list contracts\n" +
                "5. Edit contracts\n" +
                "6. Return main menu\n");

        int choice= 0;
        do{
            System.out.print("Input your choice: ");
            choice= Integer.parseInt(CommonUtil.getScanner().nextLine());
        }while (choice<=0 || choice> 6);

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

    }

    private static void edit(){

    }
}
