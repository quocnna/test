package cg.crud_template_basic.util;


import java.time.LocalDate;

public class CommonUtil {
    public static double parseDouble(String val){
        try {
            return Double.parseDouble(val);
        }catch(Exception e){
            return 0;
        }
    }

    public static boolean parseLocalDate(String val){
        try {
            LocalDate.parse(val);
            return true;
        }
        catch (Exception e){
            return false;
        }
    }
}
