package util;

import java.lang.reflect.Constructor;
import java.lang.reflect.Field;
import java.lang.reflect.Modifier;
import java.util.*;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import java.util.stream.Collectors;

public class CommonUtil {
    private static Set<Map<String, String>> validations = getValidation();

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
            if (!"Id".equals(fieldName)) {
                value = inputWithoutEmpty(value, fieldName);

                if (!validations.isEmpty()) {
                    for (Map<String, String> v : validations) {
                        if (v.containsKey(fieldName.toLowerCase())) {
                            String regex = v.get(fieldName.toLowerCase());
                            while (!value.matches(regex)) {
                                System.out.print(fieldName + " is invalid format " + regex + ". Please input again:");
                                value = getScanner().nextLine();
                            }
                            break;
                        }
                    }
                }
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

    private static Set<Map<String, String>> getValidation() {
        Set<Map<String, String>> result = new HashSet<Map<String, String>>();
        Class<ConstantUtil.VALIDATION> valid = ConstantUtil.VALIDATION.class;

        for (Field f : valid.getDeclaredFields()) {
            int mod = f.getModifiers();
            if (Modifier.isStatic(mod) && Modifier.isPublic(mod) && Modifier.isFinal(mod)) {
                try {
                    Map<String, String> map = new HashMap<String, String>();
                    map.put(f.getName(), f.get(null).toString());
                    result.add(map);
                } catch (IllegalAccessException e) {
                    e.printStackTrace();
                }
            }
        }

        return result;
    }

    private static String inputWithoutEmpty(String value, String fieldName) {
        do {
            System.out.print(value.isEmpty() ? fieldName + " cannot be empty. Please input again:" : "Input " + fieldName + " :");
            value = getScanner().nextLine();
        } while (value.isEmpty());

        return value;
    }
}
