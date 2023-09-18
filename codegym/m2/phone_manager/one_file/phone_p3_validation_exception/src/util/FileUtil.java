package util;

import model.Phone;

import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.Collections;
import java.util.List;

public class FileUtil {
    public static void write(List<Phone> phoneList, String path, boolean isAppend){
        try(BufferedWriter writer = new BufferedWriter(new FileWriter(path, isAppend))){
            for(Phone phone : phoneList){
                writer.write(phone.toPhone());
                writer.newLine();
            }
        }
        catch (IOException e){
            System.out.println(e.getMessage());
        }
    }

    public static List<String> read(String path){
        try{
            return Files.readAllLines(Path.of(path));
        }
        catch (IOException e) {
            System.out.println(e.getMessage());
        }

        return Collections.emptyList();
    }
}