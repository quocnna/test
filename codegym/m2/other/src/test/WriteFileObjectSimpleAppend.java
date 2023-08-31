package test;

import java.io.*;
import java.util.HashMap;
import java.util.Map;

public class WriteFileObjectSimpleAppend {
    public static void main(String[] args) throws Exception {

        File storageFile = new File("src/test/test.bin");
        storageFile.delete();

        write(storageFile, getO1());
        write(storageFile, getO2());
//        write(storageFile, getO2());

        ObjectInputStream ois = new ObjectInputStream(new FileInputStream(storageFile));
        read(ois, getO1());
        read(ois, getO2());
//        read(ois, getO2());
    }

    private static void write(File storageFile, Map<String, String> o) throws IOException {
        ObjectOutputStream oos = getOOS(storageFile);
        oos.writeObject(o);
        oos.close();
    }

    private static void read(ObjectInputStream ois, Map<String, String> expected) throws ClassNotFoundException, IOException {
        Object actual = ois.readObject();
        assertEquals(expected, actual);
    }

    private static void assertEquals(Object o1, Object o2) {
        if (!o1.equals(o2)) {
            throw new AssertionError("\n expected: " + o1 + "\n actual:   " + o2);
        }
    }

    private static Map<String, String> getO1() {
        Map<String, String> nvps = new HashMap<String, String>();
        nvps.put("timestamp", "1326382770000");
        nvps.put("length", "246");
        return nvps;
    }

    private static Map<String, String> getO2() {
        Map<String, String> nvps = new HashMap<String, String>();
        nvps.put("timestamp", "0");
        nvps.put("length", "0");
        return nvps;
    }

    private static ObjectOutputStream getOOS(File storageFile) throws IOException {
        if (storageFile.exists()) {
            // this is a workaround so that we can append objects to an existing file
            return new AppendableObjectOutputStream(new FileOutputStream(storageFile, true));
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
