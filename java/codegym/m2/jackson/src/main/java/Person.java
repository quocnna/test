import com.fasterxml.jackson.annotation.JsonPropertyOrder;

@JsonPropertyOrder({ "name", "age", "validated" })
public class Person {
    public String name;
    public int age;
    public boolean validated;



    public Person(String name, int age, boolean validated) {
        this.name = name;
        this.age = age;
        this.validated = validated;
    }
}
