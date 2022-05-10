package util;

import java.lang.reflect.Constructor;
import java.lang.reflect.Field;
import java.util.*;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import java.util.stream.Collectors;

public class CommonUtil {
    public static Scanner getScanner(){
        return new Scanner(System.in);
    }

    public static List<Field> getAllFields(String className) {
        List<Field> result = new ArrayList<>();

        if(!className.equals("java.lang.Object")){
            try {
                if(!className.contains("model.")){
                    className = "model."+ className;
                }

                Class<?> clazz = Class.forName(className);
                if (clazz != null) {
                    result = new ArrayList<>(getAllFields(clazz.getSuperclass().getName()));
                    List<Field> filteredFields = Arrays.stream(clazz.getDeclaredFields())
                            .collect(Collectors.toList());
                    result.addAll(filteredFields);
                }
            } catch (ClassNotFoundException e) {
                e.printStackTrace();
            }
        }

        return result;
    }

    public static List<String> inputFields(String className) {
        List result = new ArrayList();

        List<Field> fields = getAllFields(className);
        for (int i = 0; i < fields.size(); i++) {
            String fieldName = fields.get(i).getName();
            Pattern pattern = Pattern.compile("(^.)");
            Matcher matcher = pattern.matcher(fieldName);
            if (matcher.find()) {
                String tmp = fieldName.replaceAll("(\\p{javaUpperCase})", " $1");
                String firstChar = matcher.group(1);
                fieldName = tmp.replaceAll("(^.)", firstChar.toUpperCase());
            }

            String value = "";
            do {
                System.out.printf("Input %s: ", fieldName);
                value = getScanner().nextLine();
            } while (value.isEmpty());


//            if (cls.equals(Villa.class) || cls.equals(House.class) || cls.equals(Room.class)) {
//                do {
//                    System.out.printf("Input %s: ", fieldName);
//                    value = scanner.nextLine();
//                } while (!Validation.ValidateService(fieldName, value));
//            } else {
//                do {
//                    System.out.printf("Input %s: ", fieldName);
//                    value = scanner.nextLine();
//                } while (!Validation.ValidateCustomer(fieldName, value));
//            }

            result.add(value);
        }
        return result;
    }

    public static Object createInstance(String className, List<String> params){
        try{
            Class<?> c = Class.forName("model."+ className);
            Constructor<?> ctor = c.getConstructors()[0];
            Object object=ctor.newInstance(new Object[]{"ContstractorArgs"});
//            c.getDeclaredMethods()[0].invoke(object,Object... MethodArgs);
        }
        catch(Exception e){

        }



        return null;
    }
}
