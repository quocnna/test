package util;

import java.io.*;
import java.util.ArrayList;
import java.util.List;

public class FileHelper<T> {
    public List<String> read(String path){
        List result = new ArrayList<>();

        createFileIfNotExist(path);
        try(BufferedReader reader = new BufferedReader(new FileReader(path))) {
            String line;
            while ((line = reader.readLine()) != null){
                if(!line.isEmpty()){
                    result.add(line);

                }
            }
        }
        catch(IOException e) {
            e.printStackTrace();
        }

        return result;
    }

    public void write(String path, List<T> list, boolean isAppend){
        createFileIfNotExist(path);
        try(BufferedWriter bufferedWriter= new BufferedWriter(new FileWriter(path, isAppend))) {
            for (T t : list) {
                bufferedWriter.write(t.toString());
                bufferedWriter.newLine();
            }
        }
        catch(IOException e){
            e.printStackTrace();
        }
    }

    private void createFileIfNotExist(String path){
        File file = new File(path);
        if(!file.exists()){
            try {
                file.createNewFile();
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
    }
}
