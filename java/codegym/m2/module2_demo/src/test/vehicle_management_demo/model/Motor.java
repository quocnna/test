package test.vehicle_management_demo.model;

public class Motor extends Vehicle {
    private int wattage;

    public Motor(String numberPlate, String manufacture, int year, String ower, int wattage) {
        super(numberPlate, manufacture, year, ower);
        this.wattage = wattage;
    }

    public int getWattage() {
        return wattage;
    }

    public void setWattage(int wattage) {
        this.wattage = wattage;
    }

    @Override
    public String toString() {
        return String.format("%s,%s,%s,%s,%s", getNumberPlate(), getManufacture(), getYear(), getOwer(), wattage);
    }
}
