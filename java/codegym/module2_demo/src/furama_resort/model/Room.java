package furama_resort.model;

import furama_resort.util.ConstantUtil;

public class Room extends Facility {
    private String ServiceApp;

    public Room(int id, String code, String name, double areaUsed, double price, int maxPeople, ConstantUtil.RentType rentType, String serviceApp) {
        super(id, code, name, areaUsed, price, maxPeople, rentType);
        ServiceApp = serviceApp;
    }

    public String getServiceApp() {
        return ServiceApp;
    }

    public void setServiceApp(String serviceApp) {
        ServiceApp = serviceApp;
    }
}
