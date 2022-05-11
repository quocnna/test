package view;

import model.BaseEntity;
import service.GeneralService;
import util.CommonUtil;
import util.ConstantUtil;

import java.util.List;

public class HomeView {
    private static GeneralService generalService = new GeneralService();

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
        int choose =  getChoiceChild("create");

        for (int i = 0; i < ConstantUtil.CHILD_ENTITY.length; i++){
            if(choose == i+ 1){
               Object o = CommonUtil.createInstance(ConstantUtil.CHILD_ENTITY[i] , CommonUtil.inputFields(ConstantUtil.CHILD_ENTITY[i]));
               generalService.create((BaseEntity) o);
            }
        }
    }

    private static void display(){
//        int choose =  getChoiceChild("display");
        List<BaseEntity> list = generalService.findAll();
        for (int i = 0; i < list.size(); i++){
            System.out.println(list.get(i));
        }
//        for (int i = 0; i < ConstantUtil.CHILD_ENTITY.length; i++){
//            if(choose == i+ 1){
//                Object o = CommonUtil.createInstance(ConstantUtil.CHILD_ENTITY[i] , CommonUtil.inputFields(ConstantUtil.CHILD_ENTITY[i]));
//                generalService.create((BaseEntity) o);
//            }
//        }
    }

    private static void delete(){
        display();
        System.out.print("Enter id to delete:");
        int id = Integer.parseInt(CommonUtil.getScanner().nextLine());
        generalService.delete(id);
        System.out.println("Deleted successfully");
    }

    private static void search(){

    }

    private static int getChoice() {
        System.out.print("Enter your choice:");
        return Integer.parseInt(CommonUtil.getScanner().nextLine());
    }

    private static int getChoiceChild(String action){
        System.out.println("Choose items to :" + action);
        for (int i = 0; i < ConstantUtil.CHILD_ENTITY.length; i++){
            System.out.printf("%s. %s\n", i+1, ConstantUtil.CHILD_ENTITY[i]);
        }

        return getChoice();
    }
}
