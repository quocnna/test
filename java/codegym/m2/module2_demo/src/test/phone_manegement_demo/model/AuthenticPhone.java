package test.phone_manegement_demo.model;

import test.phone_manegement_demo.util.AppConstant.*;

public class AuthenticPhone extends  Phone{
    private int guaranteePeriodByYear;
    private ScopeGuarantee scopeGuarantee;

    public AuthenticPhone(int id, String name, double price, int quantity, String manufacture) {
        super(id, name, price, quantity, manufacture);
    }

    public AuthenticPhone(int id, String name, double price, int quantity, String manufacture, int guaranteePeriodByYear, ScopeGuarantee scopeGuarantee) {
        super(id, name, price, quantity, manufacture);
        this.guaranteePeriodByYear = guaranteePeriodByYear;
        this.scopeGuarantee = scopeGuarantee;
    }

    public int getGuaranteePeriodByYear() {
        return guaranteePeriodByYear;
    }

    public void setGuaranteePeriodByYear(int guaranteePeriodByYear) {
        this.guaranteePeriodByYear = guaranteePeriodByYear;
    }

    public ScopeGuarantee getScopeGuarantee() {
        return scopeGuarantee;
    }

    public void setScopeGuarantee(ScopeGuarantee scopeGuarantee) {
        this.scopeGuarantee = scopeGuarantee;
    }
}
