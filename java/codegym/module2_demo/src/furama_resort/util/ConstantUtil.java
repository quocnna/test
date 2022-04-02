package furama_resort.util;

public interface ConstantUtil {
    enum RentType{
        YEAR,
        MONTH,
        DAY,
        HOUR
    }

    enum RoomType {
        STANDARD,
        SUPERIOR,
        DELUXE,
        SUITE,
        PRESIDENTIAL
    }

    enum Gender{
        MALE,
        FEMALE,
        UNKNOW
    }

    enum TypeCustomer{
        DIAMOND,
        PLATINIUM,
        GOLC,
        SILVER,
        MEMBER
    }

    enum Degree{
        INTERMEDIATE,
        ASSOCIATE,
        BACHERLOR,
        MASTER,
        DOCTER,
        PROFESSOR
    }

    enum Position{
        Receptionist,
        ATTENDANT,
        SUPERVISOR,
        SECURITY,
        HR,
        SALES,
        DIRECTOR,
        CHEF,
        MANAGER
    }
}
