package test.vehicle_management_demo.model;

public abstract class Vehicle {
    private String numberPlate;
    private String manufacture;
    private int year;
    private String ower;

    public Vehicle(String numberPlate, String manufacture, int year, String ower) {
        this.numberPlate = numberPlate;
        this.manufacture = manufacture;
        this.year = year;
        this.ower = ower;
    }

    public String getNumberPlate() {
        return numberPlate;
    }

    public void setNumberPlate(String numberPlate) {
        this.numberPlate = numberPlate;
    }

    public String getManufacture() {
        return manufacture;
    }

    public void setManufacture(String manufacture) {
        this.manufacture = manufacture;
    }

    public int getYear() {
        return year;
    }

    public void setYear(int year) {
        this.year = year;
    }

    public String getOwer() {
        return ower;
    }

    public void setOwer(String ower) {
        this.ower = ower;
    }
}
