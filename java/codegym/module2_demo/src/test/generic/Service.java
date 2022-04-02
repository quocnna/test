package test.generic;

import java.util.List;

public class Service<T> {
    void display(List<T> tList){
        tList.forEach(System.out::println);
    }
}
