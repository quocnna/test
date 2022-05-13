import java.io.File;
import java.util.*;

public class Main {
    public static void main(String[] args) {
        int year = 2022;
        System.out.println(year % 4 == 0 ? (year % 100 == 0 ? false : true) : true);

        String kq = year % 4 == 0 ? year % 100 == 0 ? "b" : "aa"
                : "cddd";

//        System.out.println(year % 4 == 0 ?);


//        System.out.println(year % 4 == 0 ? year % 100 == 0 ? year % 400 == 0 : true : false);

        System.out.println(year % 4 == 0 ? (year % 100 == 0 ? year % 400 == 0 : true) : false);
    }
}
