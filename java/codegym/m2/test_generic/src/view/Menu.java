package view;

import exception.NotFoundException;
import service.GenericService;
import util.CommonUtil;
import util.FeatureClass;
import util.RunTimeClass;

import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.util.List;

public class Menu {
    private static GenericService generalService = new GenericService();
    private static List<RunTimeClass> rtcs = CommonUtil.rts;

    public static void main(String[] args) throws NoSuchFieldException, IllegalAccessException, InvocationTargetException, NoSuchMethodException {
        while (true){
            System.out.println("--- Menu Home---");

            String[] menu = FeatureClass.getMenu();

            for (int i = 0; i < menu.length ; i++) {
                System.out.printf("%s. %s\n", i + 1, capitalize(menu[i]));
            }

            System.out.printf("%s. Exit\n", menu.length + 1);

            int choice = Integer.parseInt(CommonUtil.inputWithoutEmpty("Enter your choice"));


            for (int i = 0; i < menu.length; i++) {
                if(choice == i +1){
                    Method method = Menu.class.getDeclaredMethod(menu[i], null);
                    method.setAccessible(true);
                    method.invoke(null);
                    break;
                }

                if(choice == menu.length + 1){
                    System.exit(0);
                }
            }
        }
    }

    private static void create() throws NoSuchFieldException, IllegalAccessException {
            System.out.println("Choose items to create:" );
            for (int i = 0; i < rtcs.size(); i++){
                String name = rtcs.get(i).getEntityName().replaceAll("(\\p{javaUpperCase})", " $1");
                System.out.printf("%s.%s\n", i+1,  name);
            }

            int choice = Integer.parseInt(CommonUtil.inputWithoutEmpty("Enter your choice"));

            for (int i = 0; i <  rtcs.size(); i++){
                if(choice == i+ 1){
                    Object o = CommonUtil.createInstance(rtcs.get(i), CommonUtil.inputFields( CommonUtil.rts.get(i)));
                    generalService.create(o);
                    System.out.println("Created successfully...");
                }
            }
    }

    private static void delete() throws InvocationTargetException, NoSuchMethodException, IllegalAccessException {
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

    private static void display() throws NoSuchMethodException, InvocationTargetException, IllegalAccessException {
        display(generalService.findAll());
    }

    private static void search() throws InvocationTargetException, NoSuchMethodException, IllegalAccessException {
        String val = CommonUtil.inputWithoutEmpty("Input " +  FeatureClass.getSearchBy() + " to search");
        display(generalService.search(FeatureClass.getSearchBy(), val));
    }

    private static void display(List<Object> objs) throws NoSuchMethodException, InvocationTargetException, IllegalAccessException {
        for (int i = 0; i < objs.size(); i++) {
            Method method = objs.get(i).getClass().getDeclaredMethod("display", null);
            method.invoke(objs.get(i), null);
        }
    }

    public static final String capitalize(String str) {
        if (str == null || str.length() == 0) {return str;}

        return str.substring(0, 1).toUpperCase() + str.substring(1);
    }
}
