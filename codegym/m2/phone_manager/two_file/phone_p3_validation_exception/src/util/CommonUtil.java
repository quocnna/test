package util;

import model.Phone;

import java.util.List;

public class CommonUtil {
    public static int getLastId(List<Phone> phoneList){
        int lastId = 0;
        if(!phoneList.isEmpty()){
            lastId = phoneList.get(phoneList.size() - 1).getId();
        }

        return lastId;
    }
}
