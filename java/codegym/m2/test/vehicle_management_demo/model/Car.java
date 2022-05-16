package test.vehicle_management_demo.model;

import test.vehicle_management_demo.common.AppConstant.*;

public class Car extends Vehicle {
    private int numberSeat;
    private TypeCar typeCar;

    public Car(String numberPlate, String manufacture, int year, String ower, int numberSeat, TypeCar typeCar) {
        super(numberPlate, manufacture, year, ower);
        this.numberSeat = numberSeat;
        this.typeCar = typeCar;
    }

    public int getNumberSeat() {
        return numberSeat;
    }

    public void setNumberSeat(int numberSeat) {
        this.numberSeat = numberSeat;
    }

    public TypeCar getTypeCar() {
        return typeCar;
    }

    public void setTypeCar(TypeCar typeCar) {
        this.typeCar = typeCar;
    }

    @Override
    public String toString() {
        return  String.format("%s,%s,%s,%s,%s,%s", getNumberPlate(), getManufacture(), getYear(), getOwer(), numberSeat, typeCar.toString());
    }
}
