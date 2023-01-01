package util;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.ArrayList;
import java.util.List;

public class FileHelper<E> {
    public  void write(String path, List<E> list, boolean isAppend){
        try(BufferedWriter writer = new BufferedWriter(new FileWriter(path, isAppend))) {
            for(E e : list){
                writer.write(e.toString());
                writer.newLine();
            }
        }
        catch (IOException e) {
            e.printStackTrace();
        }
    }

    public List<String> read(String path){
        List<String> result = new ArrayList<>();

        if(Files.exists(Path.of(path))){
            try (BufferedReader reader = new BufferedReader(new FileReader(path))){
                String line;

                while ((line = reader.readLine()) != null){
                    result.add(line);
                }

//                Files.readAllLines(Path.of(path));
            }
            catch (IOException e) {
                e.printStackTrace();
            }
        }

        return result;
    }
}
