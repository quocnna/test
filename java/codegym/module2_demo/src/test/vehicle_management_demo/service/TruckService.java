package test.vehicle_management_demo.service;

import test.vehicle_management_demo.common.AppConstant;
import test.vehicle_management_demo.common.CSVHelper;
import test.vehicle_management_demo.model.Truck;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public class TruckService {
    private List<Truck> trucks;
    private CSVHelper<Truck> csvHelper= new CSVHelper<>();

    public TruckService(){
        trucks= getFromFile();
    }

    public List<Truck> getAll(){
        return trucks;
    }

    public void add(Truck truck){
        trucks.add(truck);
        csvHelper.write(Arrays.asList(truck), true, AppConstant.Path.TRUCK);
    }

    public boolean delete(String numberPlate){
        boolean res= trucks.removeIf(e-> e.getNumberPlate().equals(numberPlate));
        if(res) csvHelper.write(trucks, false, AppConstant.Path.TRUCK);
        return res;
    }

    private List<Truck> getFromFile(){
        List<Truck> res= new ArrayList<>();
        List<String> lines= csvHelper.read(AppConstant.Path.TRUCK);
        if(lines.size()> 0){
            for (String line : lines){
                String[] tmp= line.split(",");
                String numberPlate= tmp[0];
                String manufacture= tmp[1];
                int year= Integer.parseInt(tmp[2]);
                String ower= tmp[3];
                double load= Double.parseDouble(tmp[4]);
                Truck truck= new Truck(numberPlate, manufacture, year, ower, load);
                res.add(truck);
            }
        }
        return res;
    }
}
