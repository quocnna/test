package test.generic;

import java.util.ArrayList;
import java.util.List;

public class Test {
    public static void main(String[] args) {
        List<Student> students= new ArrayList<>();
        students.add(new Student(1,"Quoc", 38));
        students.add(new Student(2,"Dung", 3));
        students.add(new Student(3,"Toan", 25));
        StudentService studentService= new StudentService();
        studentService.display(students);

        List<Product> products= new ArrayList<>();
        products.add(new Product(1,"Nokia", 200, "test"));
        products.add(new Product(2,"Apple", 500, "test"));
        products.add(new Product(3,"Samsung", 400, "test"));
        ProductService productService= new ProductService();
        productService.display(products);

        Service<Product> p= new Service<>();
        p.display(products);
    }
}
