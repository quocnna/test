package model;

public class BaseEntity {
    private int id;
    private String name;
    private String phone;
    private Double balance;
    private String branch;

    public BaseEntity() {
    }

    public BaseEntity(int id, String name, String phone, Double balance, String branch) {
        this.id = id;
        this.name = name;
        this.phone = phone;
        this.balance = balance;
        this.branch = branch;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getPhone() {
        return phone;
    }

    public void setPhone(String phone) {
        this.phone = phone;
    }

    public Double getBalance() {
        return balance;
    }

    public void setBalance(Double balance) {
        this.balance = balance;
    }

    public String getBranch() {
        return branch;
    }

    public void setBranch(String branch) {
        this.branch = branch;
    }

    @Override
    public String toString() {
        return String.format("%s,%s,%s,%s,%s", id, name, phone, balance, branch);
    }
}
