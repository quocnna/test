package util;

import model.Product;

import java.io.*;
import java.util.ArrayList;
import java.util.List;

public class FileHelper {
    public List<String> read(String path){
        List result = new ArrayList<>();

        File file = new File(path);
        try {
            if(!file.exists()){
                file.createNewFile();
            }

            BufferedReader reader = new BufferedReader(new FileReader(path));
            String line;
            while ((line = reader.readLine()) != null){
                if(!line.isEmpty()){
                    result.add(line);
                }
            }
            reader.close();
        }
        catch (IOException e){
            e.printStackTrace();
        }

        return result;
    }

    public void write(String path, List<Product> products, boolean isAppend){
        try(BufferedWriter bufferedWriter= new BufferedWriter(new FileWriter(path, isAppend))) {
            for (Product p : products) {
                bufferedWriter.write(p.toString());
                bufferedWriter.newLine();
            }
        }
        catch(IOException e){
            e.printStackTrace();
        }
    }
}
