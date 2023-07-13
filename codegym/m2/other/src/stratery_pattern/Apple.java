package stratery_pattern;

public class Apple {
    private int id;
    private String color;
    private double price;
    private double weight;

    public Apple(int id, String color, double price, double weight) {
        this.id = id;
        this.color = color;
        this.price = price;
        this.weight = weight;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getColor() {
        return color;
    }

    public void setColor(String color) {
        this.color = color;
    }

    public double getPrice() {
        return price;
    }

    public void setPrice(double price) {
        this.price = price;
    }

    public double getWeight() {
        return weight;
    }

    public void setWeight(double weight) {
        this.weight = weight;
    }

//    @Override
//    public String toString() {
//        return "Apple{" +
//                "id=" + id +
//                ", color='" + color + '\'' +
//                ", price=" + price +
//                ", weight=" + weight +
//                '}';
//    }
}
