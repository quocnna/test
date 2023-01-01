package model;

public abstract class Vehicle {
    private String driverPlate;
    private Manufacturer manufacturer;
    private int yearOfManufacturer;
    private String owner;

    public Vehicle() {
    }

    public Vehicle(String driverPlate, Manufacturer manufacturer, int yearOfManufacturer, String owner) {
        this.driverPlate = driverPlate;
        this.manufacturer = manufacturer;
        this.yearOfManufacturer = yearOfManufacturer;
        this.owner = owner;
    }

    public String getDriverPlate() {
        return driverPlate;
    }

    public void setDriverPlate(String driverPlate) {
        this.driverPlate = driverPlate;
    }

    public Manufacturer getManufacturer() {
        return manufacturer;
    }

    public void setManufacturer(Manufacturer manufacturer) {
        this.manufacturer = manufacturer;
    }

    public int getYearOfManufacturer() {
        return yearOfManufacturer;
    }

    public void setYearOfManufacturer(int yearOfManufacturer) {
        this.yearOfManufacturer = yearOfManufacturer;
    }

    public String getOwner() {
        return owner;
    }

    public void setOwner(String owner) {
        this.owner = owner;
    }

    @Override
    public String toString() {
        return String.format("%s,%s,%s,%s", driverPlate, manufacturer.getName(), yearOfManufacturer, owner);
    }
}
