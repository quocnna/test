package model;

public class SavingAccount extends BaseEntity {
    private String code;
    private String status;

    public SavingAccount(String code, String status) {
        this.code = code;
        this.status = status;
    }

    public SavingAccount(int id, String name, String phone, Double balance, String branch, String code, String status) {
        super(id, name, phone, balance, branch);
        this.code = code;
        this.status = status;
    }

    public String getCode() {
        return code;
    }

    public void setCode(String code) {
        this.code = code;
    }

    public String getStatus() {
        return status;
    }

    public void setStatus(String status) {
        this.status = status;
    }

    @Override
    public String toString() {
        return String.format("%s,%s,%s,%s", getClass().getSimpleName(), super.toString(), code, status);
    }
}
