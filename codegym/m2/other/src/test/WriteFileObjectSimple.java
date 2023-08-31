package test;

import stratery_pattern.Apple;
import java.io.*;

public class WriteFileObjectSimple {
    private static final File file = new File("src/test/data.bin");

    public static void main(String[] args) throws IOException {
        Apple apple1 = new Apple(1, "red", 1, 1);
        Apple apple2 = new Apple(2, "red", 1, 1);

        writeObject(file, apple1);
        writeObject(file, apple2);

        ObjectInputStream ois = new ObjectInputStream(new FileInputStream(file));
        while (true) {
            try {
                Apple apple = (Apple) readObject(ois);
                System.out.println(apple);
            } catch (EOFException | ClassNotFoundException e) {
                System.out.println("end of file");
            }
        }
    }

    private static void writeObject(File file, Apple apple) throws IOException {
        ObjectOutputStream oos = getOOS(file);
        oos.writeObject(apple);
        oos.close();
    }

    private static Object readObject(ObjectInputStream ois) throws IOException, ClassNotFoundException {
        return ois.readObject();
    }

    private static ObjectOutputStream getOOS(File storageFile) throws IOException {
        if (storageFile.exists()) {
            // this is a workaround so that we can append objects to an existing file
            return new WriteFileObjectSimple.AppendableObjectOutputStream(new FileOutputStream(storageFile, true));
        } else {
            return new ObjectOutputStream(new FileOutputStream(storageFile));
        }
    }

    private static class AppendableObjectOutputStream extends ObjectOutputStream {

        public AppendableObjectOutputStream(OutputStream out) throws IOException {
            super(out);
        }

        @Override
        protected void writeStreamHeader() throws IOException {
            // do not write a header
        }
    }
}
