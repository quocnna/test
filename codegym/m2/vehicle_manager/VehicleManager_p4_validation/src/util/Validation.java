package util;

import util.ConstantUtil.DriverPlateRegex;

public class Validation {
    public static boolean isCarPlate(String plate, boolean isTourist){
       return   isTourist ? plate.matches(DriverPlateRegex.CAR_TOURIST) : plate.matches(DriverPlateRegex.CAR_COACH);
    }

    public static boolean isTruck(String plate){
        return plate.matches(DriverPlateRegex.TRUNK);
    }

    public static boolean isMotor(String plate){
        return plate.matches(DriverPlateRegex.MOTOR);
    }
}
