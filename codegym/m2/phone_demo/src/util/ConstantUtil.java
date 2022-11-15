package util;

public interface ConstantUtil {
    String PATH = "src/data/data.csv";
    String[] CHILD_ENTITY = { "AuthenticPhone", "HandPhone"};

    interface VALIDATION{
        String name = "[ABC]\\w*";
        String price = "\\d{0,4}(\\.\\d{1,2})?";
    }
}
