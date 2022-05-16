package test.vehicle_management_demo.model;

public class Truck extends Vehicle {
    private double load;

    public Truck(String numberPlate, String manufacture, int year, String ower, double load) {
        super(numberPlate, manufacture, year, ower);
        this.load = load;
    }

    public double getLoad() {
        return load;
    }

    public void setLoad(double load) {
        this.load = load;
    }

    @Override
    public String toString() {
        return  String.format("%s,%s,%s,%s,%s", getNumberPlate(), getManufacture(), getYear(), getOwer(), load);
    }
}
