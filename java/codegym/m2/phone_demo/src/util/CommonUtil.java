package util;

import java.lang.reflect.Constructor;
import java.lang.reflect.Field;
import java.util.*;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import java.util.stream.Collectors;

public class CommonUtil {
    public static Scanner getScanner() {
        return new Scanner(System.in);
    }

    public static List<Field> getAllFields(String className) {
        List<Field> result = new ArrayList<>();

        if (!("Object").equals(className)) {
            try {
                className = "model." + className;
                Class<?> clazz = Class.forName(className);
                if (clazz != null) {
                    result = new ArrayList<>(getAllFields(clazz.getSuperclass().getSimpleName()));
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

            String value = "0";
            if(!"Id".equals(fieldName)){
                do {
                    System.out.printf("Input %s: ", fieldName);
                    value = getScanner().nextLine();
                } while (value.isEmpty());
            }

            result.add(value);
        }
        return result;
    }

    public static Object createInstance(String className, List<String> params) {
        try {
            Class<?> c = Class.forName("model." + className);
            Constructor<?> ctor = c.getConstructors()[1];
            ctor.setAccessible(true);

            Object[] tmp = new Object[params.size()];

            List<Field> fields = getAllFields(className);
            for (int i = 0; i < fields.size(); i++) {
                Class<?> fieldType = fields.get(i).getType();
                switch (fieldType.getCanonicalName()) {
                    case "int":
                        tmp[i] = Integer.valueOf(params.get(i));
                        break;
                    case "java.lang.Double":
                        tmp[i] = Double.valueOf(params.get(i));
                        break;
                    default:
                        tmp[i] = params.get(i);
                }
            }

            return ctor.newInstance(tmp);
        } catch (Exception e) {
            e.printStackTrace();
        }

        return null;
    }
}
