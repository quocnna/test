package test.product_object_file;

import java.io.Serializable;
import java.util.Objects;

public class Product implements Serializable, Comparable<Product> {
        private int id;
    private String code;
    private String name;
    private String brand;
    private double price;
    private String desciption;

    public Product(int id, String code, String name, String brand, double price, String desciption) {
        this.id = id;
        this.code = code;
        this.name = name;
        this.brand = brand;
        this.price = price;
        this.desciption = desciption;
    }

    public Product(String code, String name, String brand, double price, String desciption) {
        this.code = code;
        this.name = name;
        this.brand = brand;
        this.price = price;
        this.desciption = desciption;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getCode() {
        return code;
    }

    public void setCode(String code) {
        this.code = code;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getBrand() {
        return brand;
    }

    public void setBrand(String brand) {
        this.brand = brand;
    }

    public double getPrice() {
        return price;
    }

    public void setPrice(double price) {
        this.price = price;
    }

    public String getDesciption() {
        return desciption;
    }

    public void setDesciption(String desciption) {
        this.desciption = desciption;
    }

    @Override
    public boolean equals(Object o) {
        return id== ((Product)o).id;
    }

    @Override
    public int hashCode() {
        return Objects.hash(getId());
    }

    @Override
    public String toString() {
        return "Product{" +
                "id=" + id +
                ", code='" + code + '\'' +
                ", name='" + name + '\'' +
                ", brand='" + brand + '\'' +
                ", price=" + price +
                ", desciption='" + desciption + '\'' +
                '}';
    }

    @Override
    public int compareTo(Product o) {
        return (int)(price - o.getPrice());
    }
}
