package model;

public class AuthenticPhone extends Phone{
    private int granteeByYear;
    private String granteeCode;

    public AuthenticPhone(int id, String name, Double price, String manufacturer, int granteeByYear, String granteeCode) {
        super(id, name, price, manufacturer);
        this.granteeByYear = granteeByYear;
        this.granteeCode = granteeCode;
    }

    public int getGranteeByYear() {
        return granteeByYear;
    }

    public void setGranteeByYear(int granteeByYear) {
        this.granteeByYear = granteeByYear;
    }

    public String getGranteeCode() {
        return granteeCode;
    }

    public void setGranteeCode(String granteeCode) {
        this.granteeCode = granteeCode;
    }

    @Override
    public String toString() {
        return "AuthenticPhone{" +
                super.toString() +
                ",granteeByYear=" + granteeByYear +
                ", granteeCode='" + granteeCode + '\'' +
                '}';
    }

    @Override
    String toPhone() {
        return String.format("%s,%s,%s,%s,%s,%s", getId(), getName(), getPrice(), getManufacturer(), granteeByYear, granteeCode);
    }
}
