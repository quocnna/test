package furama_resort.view;

import furama_resort.util.CommonUtil;

public class PromotionView {
    static void promotionManager(){
        System.out.println("1. Display list customers use service\n" +
                "2. Display list customers get voucher\n" +
                "4. Return main menu\n");

        int choice= 0;
        do{
            System.out.print("Input your choice: ");
            choice= Integer.parseInt(CommonUtil.getScanner().nextLine());
        }while (choice<=0 || choice> 4);

        switch (choice){
            case 1:
//                display();
                break;
            case 2:
//                create();
                break;
            case 3:
//                edit();
                break;
            case 4:
                HomeView.displayMainMenu();
        }
    }
}
