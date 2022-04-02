package test.vehicle_management_demo.common;

import java.io.*;
import java.util.ArrayList;
import java.util.List;

public class CSVHelper<T> {
    public List<String> read(String path){
        List<String> res= new ArrayList<>();

        createFileIfNotExist(path);
        try(BufferedReader reader= new BufferedReader(new FileReader(path))) {
            String line;
            while ((line= reader.readLine())!= null){
                res.add(line);
            }
        }
        catch (IOException e){
            e.printStackTrace();
        }
        return res;
    }

    public void write(List<T> tList, boolean isAppend, String path){
        createFileIfNotExist(path);
        try(BufferedWriter writer= new BufferedWriter(new FileWriter(path, isAppend))){
            for (T t: tList) {
                writer.write(t.toString());
                writer.newLine();
            }
        }
        catch (IOException e){
            e.printStackTrace();
        }
    }

    private void createFileIfNotExist(String path) {
        try {
            File file= new File(path);
            if(!file.exists())
                file.createNewFile();
        }catch (IOException e) {
            e.printStackTrace();
        }
    }
}
