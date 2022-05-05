package test.phone_manegement_demo.model;

import test.phone_manegement_demo.util.AppConstant.*;

public class HandPhone extends Phone {
    private String country;
    private Status status;

    public HandPhone(int id, String name, double price, int quantity, String manufacture) {
        super(id, name, price, quantity, manufacture);
    }

    public HandPhone(int id, String name, double price, int quantity, String manufacture, String country, Status status) {
        super(id, name, price, quantity, manufacture);
        this.country = country;
        this.status = status;
    }

    public String getCountry() {
        return country;
    }

    public void setCountry(String country) {
        this.country = country;
    }

    public Status getStatus() {
        return status;
    }

    public void setStatus(Status status) {
        this.status = status;
    }
}
