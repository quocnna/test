package model;

public class Motor extends Vehicle{
    private Double wattage;

    public Motor(Double wattage) {
        this.wattage = wattage;
    }

    public Motor(String driverPlate, Manufacturer manufacturer, int yearOfManufacturer, String owner, Double wattage) {
        super(driverPlate, manufacturer, yearOfManufacturer, owner);
        this.wattage = wattage;
    }

    public Double getWattage() {
        return wattage;
    }

    public void setWattage(Double wattage) {
        this.wattage = wattage;
    }

    @Override
    public String toString() {
        return String.format("%s,%s", super.toString(), wattage);
//        return "Motor{" +
//                "wattage=" + wattage +
//                "} " + super.toString();
    }


}
