package util;

import java.util.Scanner;

public class ValidationUtil {
    public static Scanner getScanner() {
        return new Scanner(System.in);
    }

    public static String inputWithOutEmpty(String field){
        String res = null;

        do {
            System.out.printf(res != null ? "This %s cannot empty. Please input again:" : field + ":", field);
            res = getScanner().nextLine();
        }while (res.isBlank());

        return res;
    }

    public static String inputName(){
        String res = inputWithOutEmpty("Name");
        while (!res.matches("^DT.+")){
            res = inputWithOutEmpty("Name have to start by DT. Please input again");
        }

        return res;
    }

    public static Double inputPriceGreaterThan50() {
        String res = inputWithOutEmpty("Price");
        while (!isDouble(res) || Double.parseDouble(res) <= 50){
            res = inputWithOutEmpty("This price have to greater than 50. Please input again");
        }

        return Double.parseDouble(res);
    }

    private static boolean isDouble(String val){
        if(val == null){
            return false;
        }

        try {
            Double.parseDouble(val);
            return true;
        }
        catch (NumberFormatException e){
            return false;
        }
    }
}
