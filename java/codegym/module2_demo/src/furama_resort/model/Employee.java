package furama_resort.model;

import furama_resort.util.ConstantUtil.*;

public class Employee extends Person implements Comparable<Employee> {
    private Degree degree;
    private Position position;
    private double salary;

    public Employee(int id, String code, String fullName, String birthday, Gender gender, String phone, String email, String address, Degree degree, Position position, double salary) {
        super(id, code, fullName, birthday, gender, phone, email, address);
        this.degree = degree;
        this.position = position;
        this.salary = salary;
    }

    public Degree getDegree() {
        return degree;
    }

    public void setDegree(Degree degree) {
        this.degree = degree;
    }

    public Position getPosition() {
        return position;
    }

    public void setPosition(Position position) {
        this.position = position;
    }

    public double getSalary() {
        return salary;
    }

    public void setSalary(double salary) {
        this.salary = salary;
    }

    @Override
    public int compareTo(Employee o) {
        return getFullName().compareTo(o.getFullName());
    }

    @Override
    public String toString() {
        return "Employee{" +
                "id=" + getId() +
                ", code='" + getCode() + '\'' +
                ", fullName='" + getFullName() + '\'' +
                ", birthday='" + getBirthday() + '\'' +
                ", gender=" +  getGender() +
                ", phone='" + getPhone() + '\'' +
                ", email='" + getEmail() + '\'' +
                ", address='" + getAddress() + '\'' +
                "degree=" + degree +
                ", position=" + position +
                ", salary=" + salary +
                '}';
    }

    @Override
    public boolean equals(Object obj) {
        return getId()== ((Employee)obj).getId();
    }
}
