package furama_resort.model;

import furama_resort.util.ConstantUtil;
import furama_resort.util.ConstantUtil.TypeCustomer;

public class Customer extends Person {
    private TypeCustomer typeCustomer;

    public Customer(int id, String code, String fullName, String birthday, ConstantUtil.Gender gender, String phone, String email, String address, TypeCustomer typeCustomer) {
        super(id, code, fullName, birthday, gender, phone, email, address);
        this.typeCustomer = typeCustomer;
    }

    public TypeCustomer getTypeCustomer() {
        return typeCustomer;
    }

    public void setTypeCustomer(TypeCustomer typeCustomer) {
        this.typeCustomer = typeCustomer;
    }
}
