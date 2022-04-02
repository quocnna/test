package test.vehicle_management_demo.service;

import test.vehicle_management_demo.common.AppConstant;
import test.vehicle_management_demo.common.CSVHelper;
import test.vehicle_management_demo.model.Motor;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public class MotorService {
    private List<Motor> motors;
    private CSVHelper<Motor> csvHelper= new CSVHelper<>();

    public MotorService(){
        motors= getFromFile();
    }

    public List<Motor> getAll(){
        return motors;
    }

    public void add(Motor motor){
        motors.add(motor);
        csvHelper.write(Arrays.asList(motor), true, AppConstant.Path.MOTOR);
    }

    public boolean delete(String numberPlate){
        boolean res= motors.removeIf(e-> e.getNumberPlate().equals(numberPlate));
        if(res) csvHelper.write(motors, false, AppConstant.Path.CAR);
        return res;
    }

    private List<Motor> getFromFile(){
        List<Motor> res= new ArrayList<>();
        List<String> lines= csvHelper.read(AppConstant.Path.MOTOR);
        if(lines.size()> 0){
            for (String line : lines){
                String[] tmp= line.split(",");
                String numberPlate= tmp[0];
                String manufacture= tmp[1];
                int year= Integer.parseInt(tmp[2]);
                String owner= tmp[3];
                int wattage= Integer.parseInt(tmp[4]);
                Motor motor= new Motor(numberPlate, manufacture, year, owner, wattage);
                res.add(motor);
            }
        }
        return res;
    }
}
