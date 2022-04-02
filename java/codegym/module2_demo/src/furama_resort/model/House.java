package furama_resort.model;

import furama_resort.util.ConstantUtil.*;

public class House extends Facility {
    private RoomType roomType;
    private int numberOfFloor;

    public House(int id, String code, String name, double areaUsed, double price, int maxPeople, RentType rentType, RoomType roomType, int numberOfFloor) {
        super(id, code, name, areaUsed, price, maxPeople, rentType);
        this.roomType = roomType;
        this.numberOfFloor = numberOfFloor;
    }

    public RoomType getRoomType() {
        return roomType;
    }

    public void setRoomType(RoomType roomType) {
        this.roomType = roomType;
    }

    public int getNumberOfFloor() {
        return numberOfFloor;
    }

    public void setNumberOfFloor(int numberOfFloor) {
        this.numberOfFloor = numberOfFloor;
    }


}
