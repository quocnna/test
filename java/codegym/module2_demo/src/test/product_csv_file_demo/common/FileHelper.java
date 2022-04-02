package test.product_csv_file_demo.common;

import java.io.*;
import java.util.ArrayList;
import java.util.List;

public class FileHelper<T> {
    public List<String> read(String path){
        List<String> res= new ArrayList<>();

        createFileIfNotExists(path);
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

    public void write(String path, List<T> ts, boolean isAppend){
        createFileIfNotExists(path);
        try(BufferedWriter bufferedWriter= new BufferedWriter(new FileWriter(path, isAppend))) {
            for (T t: ts){
                bufferedWriter.write(t.toString());
                bufferedWriter.newLine();
            }
        }
        catch (IOException e){
            e.printStackTrace();
        }
    }

    private void createFileIfNotExists(String path){
        try {
            File file= new File(path);
            if(!file.exists()) file.createNewFile();
        }
        catch (IOException e){
            e.printStackTrace();
        }
    }
}
