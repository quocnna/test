package util;

import model.Product;

import java.util.List;

public class CommonUtil {
    public static FileHelper getFileHelper() {
        return new FileHelper();
    }

    public static int getLastProductId(List<? extends Product> list){
        int lastId = 0;
        if(list.size() > 0){
            lastId = list.get(list.size() - 1).getId();
        }

        return lastId;
    }
}
