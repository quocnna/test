package util;

import javax.tools.*;
import java.io.File;
import java.io.FileWriter;
import java.lang.reflect.Constructor;
import java.lang.reflect.Field;
import java.net.URL;
import java.net.URLClassLoader;
import java.util.*;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import static util.ConstantUtil.CONFIG_PATH;

public class CommonUtil {
    public static Scanner getScanner() {
        return new Scanner(System.in);
    }

    private static FileHelper fileHelper = new FileHelper();
    public static List<RunTimeClass> rts = mapToClass(CONFIG_PATH);

    public static List<RunTimeClass> mapToClass(String path) {
        List<RunTimeClass> result = new ArrayList<>();
        Map<String, String> parentFields = new LinkedHashMap<>();

        List<String> lines = fileHelper.read(path);

        for (int i = 0; i < lines.size(); i++) {
            String[] tmp = lines.get(i).split(":");
            if (i == 0 && tmp[0].contains("parent")) {
                putFields(parentFields, tmp[1]);
            } else if (tmp[0].contains("search")) {
                FeatureClass.setSearchBy(tmp[1]);
            } else if (tmp[0].contains("sort")) {
                String[] sort = tmp[1].split("-");
                Map<String, String> mapSort = new LinkedHashMap<>();
                mapSort.put(sort[0], sort[1]);
                FeatureClass.setSort(mapSort);
            } else if (tmp[0].contains("validate")) {
                Map<String, String> mapValid = new LinkedHashMap<>();
                String[] valid = tmp[1].split(",");
                for (int j = 0; j < valid.length; j++){
                    String[] v = valid[j].split("-");
                    mapValid.put(v[0],v[1]);
                }

                FeatureClass.setValid(mapValid);
            } else if(tmp[0].contains("menu")){
                FeatureClass.setMenu(tmp[1].split(","));
            }
            else if(tmp[0].contains("path")){
                FeatureClass.setPath(tmp[1]);
            }
            else {
                RunTimeClass runTimeClass = new RunTimeClass();
                runTimeClass.setEntityName(tmp[0]);
                runTimeClass.setFields(putFields(parentFields, tmp[1]));
                runTimeClass.setCls(generateDynamicClass(runTimeClass));
                result.add(runTimeClass);
            }
        }

        return result;
    }

    public static Object createInstance(RunTimeClass runTimeClass, List<String> params) {
        try {
            Constructor<?> ctor = runTimeClass.getCls().getConstructors()[0];
            ctor.setAccessible(true);

            Object[] tmp = new Object[runTimeClass.getFields().size()];

            List listKeys = new ArrayList<>(runTimeClass.getFields().values());
            for (int i = 0; i < runTimeClass.getFields().size(); i++) {
                switch (listKeys.get(i).toString()) {
                    case "int":
                        tmp[i] = Integer.valueOf(params.get(i));
                        break;
                    case "double":
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

    public static List<String> inputFields(RunTimeClass runTimeClass) {
        List result = new ArrayList();

        runTimeClass.getFields().keySet().forEach(e -> {

            Pattern pattern = Pattern.compile("(^.)");
            Matcher matcher = pattern.matcher(e);
            if (matcher.find()) {
                String tmp = e.replaceAll("(\\p{javaUpperCase})", " $1");
                String firstChar = matcher.group(1);
                e = tmp.replaceAll("(^.)", firstChar.toUpperCase());
            }

            String value = "0";
            if (!"Id".equals(e)) {
                value = inputWithoutEmpty(e);

                if (FeatureClass.getValid() != null) {
                    for (Map.Entry<String, String> entry : FeatureClass.getValid().entrySet()) {
                        if (entry.getKey().equals(e.toLowerCase())) {
                            String regex = entry.getValue();

                            if(regex.equals("number")){
                                while (!isNumber(value)) {
                                    System.out.print(e + " is number " + regex + ". Please input again:");
                                    value = getScanner().nextLine();
                                }
                                break;
                            }
                            else{
                                while (!value.matches(regex)) {
                                    System.out.print(e + " is invalid format " + regex + ". Please input again:");
                                    value = getScanner().nextLine();
                                }
                                break;
                            }
                        }
                    }
                }
            }

            result.add(value);


        });

        return result;
    }

    private static Map<String, String> putFields(Map<String, String> parentFields, String val) {
        Map<String, String> result = new LinkedHashMap<>();

        String[] fields = val.split(",");
        if (fields.length > 0) {
            for (int j = 0; j < fields.length; j++) {
                String[] tmp = fields[j].split("-");

                if (parentFields.size() > 0 && j == 0) {
                    result.putAll(parentFields);
                    result.put(tmp[0], tmp[1]);
                } else {
                    if (result.size() > 0) {
                        result.put(tmp[0], tmp[1]);
                        continue;
                    }

                    parentFields.put(tmp[0], tmp[1]);
                }
            }
        }

        return result;
    }

    //region private method support build dynamic class
    private static Class<?> generateDynamicClass(RunTimeClass runTimeClass) {
        try {
            File sourceFile = File.createTempFile(runTimeClass.getEntityName(), ".java");
            sourceFile.deleteOnExit();

            String classname = sourceFile.getName().split("\\.")[0];

            StringBuilder declareField = new StringBuilder();
            for (Map.Entry<String, String> entry : runTimeClass.getFields().entrySet()) {
                declareField.append(generateField(entry.getKey(), entry.getValue()));
            }

            String sourceCode = "public class " + classname + String.format("{\n %s %s %s %s}", declareField, generateConstructor(classname, runTimeClass.getFields()), generateToString(runTimeClass), generateDisplay(runTimeClass));

            FileWriter writer = new FileWriter(sourceFile);
            writer.write(sourceCode);
            writer.close();

            JavaCompiler compiler = ToolProvider.getSystemJavaCompiler();
            StandardJavaFileManager fileManager = compiler.getStandardFileManager(null, null, null);
            File parentDirectory = sourceFile.getParentFile();
            fileManager.setLocation(StandardLocation.CLASS_OUTPUT, Arrays.asList(parentDirectory));
            Iterable<? extends JavaFileObject> compilationUnits = fileManager.getJavaFileObjectsFromFiles(Arrays.asList(sourceFile));
            compiler.getTask(null, fileManager, null, null, null, compilationUnits).call();
            fileManager.close();

            URLClassLoader classLoader = URLClassLoader.newInstance(new URL[]{parentDirectory.toURI().toURL()});

            return classLoader.loadClass(classname);
        } catch (Exception e) {
            e.printStackTrace();
        }

        return null;
    }

    private static String generateField(String fieldName, String dataType) {
        String tmp = "private DataType FieldName;\n" +
                "\n" +
                "    public DataType getFieldName() {\n" +
                "        return FieldName;\n" +
                "    }\n" +
                "\n" +
                "    public void setFieldName(DataType fieldName) {\n" +
                "        FieldName = fieldName;\n" +
                "    }\n";

        return tmp.replace("FieldName", fieldName).replace("DataType", dataType);
    }

    private static String generateConstructor(String className, Map<String, String> fields) {
        String params = "";
        String body = "";
        for (Map.Entry<String, String> entry : fields.entrySet()) {
            String key = entry.getKey();
            String val = entry.getValue();
            params += val + " " + key + ",";
            body += "this." + key + "=" + key + ";";
        }

        params = params.substring(0, params.length() - 1);
        String res = String.format("public %s(%s){%s}\n", className, params, body);
        return res;
    }

    private static String generateToString(RunTimeClass runTimeClass) {
//        String head = "@Override\n public String toString() { return \"" + runTimeClass.getEntityName() + "{\" +\n";
//        String body = "";
//        for (Map.Entry<String, String> entry : runTimeClass.getFields().entrySet()) {
//            String key = entry.getKey();
//            body += "\", " + key + "=\"" + String.format(" + %s +\n", key);
//        }
//        String res = head + body.replaceFirst(", ", "") + "'}';}";
//
//        return res;

        String head = "@Override\n public String toString() { return \"" + runTimeClass.getEntityName() + ",\"";
        String body = "";
        for (Map.Entry<String, String> entry : runTimeClass.getFields().entrySet()) {
            String key = entry.getKey();
            body += "+" + key + "+\",\"";
        }

        String res = head + body.substring(0, body.length() - 4) + ";}";
        return res;
    }
    //endregion

    private static String generateDisplay(RunTimeClass runTimeClass) {
        String head = "\n public void display() { System.out.println( \"" + runTimeClass.getEntityName() + "{\" +\n";
        String body = "";
        for (Map.Entry<String, String> entry : runTimeClass.getFields().entrySet()) {
            String key = entry.getKey();
            body += "\", " + key + "=\"" + String.format(" + %s +\n", key);
        }
        String res = head + body.replaceFirst(", ", "") + "'}');}";

        return res;
    }

    public static String inputWithoutEmpty(String fieldName) {
        String value = "0";
        do {
            System.out.print(value.isEmpty() ? fieldName + " cannot be empty. Please input again: " : fieldName + ": ");
            value = getScanner().nextLine();
        } while (value.isEmpty());

        return value;
    }

    public static boolean isNumber(String val){
        try {
            Integer.parseInt(val);
            return true;
        }
        catch (NumberFormatException e){
            return false;
        }
    }
}
