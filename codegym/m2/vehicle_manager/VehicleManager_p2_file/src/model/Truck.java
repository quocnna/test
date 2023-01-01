package model;

public class Truck extends Vehicle{
    private Double load;

    public Truck(Double load) {
        this.load = load;
    }

    public Truck(String driverPlate, Manufacturer manufacturer, int yearOfManufacturer, String owner, Double load) {
        super(driverPlate, manufacturer, yearOfManufacturer, owner);
        this.load = load;
    }

    public Double getLoad() {
        return load;
    }

    public void setLoad(Double load) {
        this.load = load;
    }

    @Override
    public String toString() {
        return String.format("%s,%s", super.toString(), load);
//        return "Truck{" +
//                "load=" + load +
//                "} " + super.toString();
    }
}
