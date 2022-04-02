package test.generic;

import java.util.List;

public class StudentService {
    void display(List<Student> studentList){
        studentList.forEach(System.out::println);
    }
}
