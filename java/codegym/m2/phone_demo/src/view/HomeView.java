package view;

import exception.NotFoundException;
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
        System.out.println("Choose items to create:" );
        for (int i = 0; i < ConstantUtil.CHILD_ENTITY.length; i++){
            System.out.printf("%s. %s\n", i+1, ConstantUtil.CHILD_ENTITY[i]);
        }
        int choose =  getChoice();

        for (int i = 0; i < ConstantUtil.CHILD_ENTITY.length; i++){
            if(choose == i+ 1){
               Object o = CommonUtil.createInstance(ConstantUtil.CHILD_ENTITY[i] , CommonUtil.inputFields(ConstantUtil.CHILD_ENTITY[i]));
               generalService.create((BaseEntity) o);
            }
        }
    }

    private static void display(){
         generalService.findAll().forEach(System.out::println);
    }

    private static void delete(){
        display();
        boolean isExists;
        System.out.print("Enter ID to delete:");
        do{
            try {
                int id = Integer.parseInt(CommonUtil.getScanner().nextLine());
                generalService.delete(id);
                System.out.println("Deleted successfully");
                isExists = false;
            }
            catch (NotFoundException e){
                System.out.print(e.getMessage() + " Please input ID again:");
                isExists = true;
            }
        }while(isExists);
    }

    private static void search(){
        System.out.print("Input name to delete:");
        String name = CommonUtil.getScanner().nextLine();
        generalService.search(name).forEach(System.out::println);
    }

    private static int getChoice() {
        System.out.print("Enter your choice:");
        return Integer.parseInt(CommonUtil.getScanner().nextLine());
    }
}
