package read_json;

import org.json.JSONArray;
import org.json.JSONObject;

import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.Iterator;

public class ReadJsonToClass {
    public static void main(String[] args) {
//        String lines = Files.readString(Paths.get("src/test.json"));
//        JSONObject obj = new JSONObject(lines);
//        Iterator<String> s = obj.keys();
//        if(s.hasNext()){
//            String entity = s.next();
//            System.out.println(entity);
//            int l = obj.getJSONObject(entity).length();
//            for(int i = 0; i < l; i++){
//                JSONObject o = obj.getJSONObject(entity);
//                JSONArray arr = o.names();
//                String field = arr.get(i).toString();
//                String dataType = o.getString(field);
//                System.out.println(field + ":" + dataType);
//            }
//        }
    }


    private void generateClass(){
//        File sourceFile = File.createTempFile("Hello", ".java");
//        sourceFile.deleteOnExit();
//
//        // generate the source code, using the source filename as the class name
//        String classname = sourceFile.getName().split("\\.")[0];
//        String sourceCode = "public class " + classname + "{ public void hello() { System.out.print(\"Hello world\");}}";
//
//        // write the source code into the source file
//        FileWriter writer = new FileWriter(sourceFile);
//        writer.write(sourceCode);
//        writer.close();
//
//
//
//        JavaCompiler compiler = ToolProvider.getSystemJavaCompiler();
//        StandardJavaFileManager fileManager = compiler.getStandardFileManager(null, null, null);
//        File parentDirectory = sourceFile.getParentFile();
//        fileManager.setLocation(StandardLocation.CLASS_OUTPUT, Arrays.asList(parentDirectory));
//        Iterable<? extends JavaFileObject> compilationUnits = fileManager.getJavaFileObjectsFromFiles(Arrays.asList(sourceFile));
//        compiler.getTask(null, fileManager, null, null, null, compilationUnits).call();
//        fileManager.close();
//
//
//        // load the compiled class
//        URLClassLoader classLoader = URLClassLoader.newInstance(new URL[] { parentDirectory.toURI().toURL() });
//        Class<?> helloClass = classLoader.loadClass(classname);
//
//        // call a method on the loaded class
//        Method helloMethod = helloClass.getDeclaredMethod("hello");
//        helloMethod.invoke(helloClass.newInstance());
    }
}
