package test.product_csv_file_demo.model;

public class HandProduct extends Product{
    private String country;
    private String status;

    public HandProduct(int id, String name, double price, String manufacture, String country, String status) {
        super(id, name, price, manufacture);
        this.country = country;
        this.status = status;
    }

    public String getCountry() {
        return country;
    }

    public String getStatus() {
        return status;
    }

    @Override
    public String toString() {
        return String.format("%s,%s,%s,%s,%s,%s", getId(), getName(), getPrice(), getManufacture(), country, status);
    }
}
