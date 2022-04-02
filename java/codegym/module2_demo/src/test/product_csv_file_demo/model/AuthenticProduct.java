package test.product_csv_file_demo.model;

public class AuthenticProduct extends Product{
    private int guarenteePeriodByYear;

    public AuthenticProduct(int id, String name, double price, String manufacture, int guarenteePeriodByYear) {
        super(id, name, price, manufacture);
        this.guarenteePeriodByYear = guarenteePeriodByYear;
    }

    public int getGuarenteePeriodByYear() {
        return guarenteePeriodByYear;
    }

    @Override
    public String toString() {
        return String.format("%s,%s,%s,%s,%s", getId(), getName(), getPrice(), getManufacture(), guarenteePeriodByYear);
    }
}
