package exception_demo;

import java.io.FileNotFoundException;
import java.io.IOException;

public class ExceptionInterviewQuestion_01 {
    public static void main(String[] args) {
        try {
            multiple();
        } catch (IOException ex) {
            ex.printStackTrace();
        }
    }

    public static void multiple() throws IOException, FileNotFoundException {
        System.out.println("Inside multiple");
    }

}
