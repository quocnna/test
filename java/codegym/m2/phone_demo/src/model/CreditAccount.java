package model;

public class CreditAccount extends BaseEntity{
    private String nummberCard;
    private String pin;

    public CreditAccount(String nummberCard, String pin) {
        this.nummberCard = nummberCard;
        this.pin = pin;
    }

    public CreditAccount(int id, String name, String phone, Double balance, String branch, String nummberCard, String pin) {
        super(id, name, phone, balance, branch);
        this.nummberCard = nummberCard;
        this.pin = pin;
    }

    public String getNummberCard() {
        return nummberCard;
    }

    public void setNummberCard(String nummberCard) {
        this.nummberCard = nummberCard;
    }

    public String getPin() {
        return pin;
    }

    public void setPin(String pin) {
        this.pin = pin;
    }

    @Override
    public String toString() {
        return String.format("%s,%s,%s,%s", getClass().getSimpleName(), super.toString(), nummberCard, pin);
    }
}
