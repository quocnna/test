package service;

import model.BaseEntity;
import util.ConstantUtil;
import util.FileHelper;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.stream.Collectors;

public class GeneralService {
    private List<BaseEntity> list = new ArrayList<>();
    private FileHelper fileHelper = new FileHelper();

    public GeneralService() {
        list = Collections.emptyList();
    }

    public void create(BaseEntity baseEntity) {
        //region get last id
        int lastId = 0;

        if (list.size() > 0) {
            lastId = list.get(list.size() - 1).getId();
        }
        //endregion

        baseEntity.setId(lastId + 1);

        list.add(baseEntity);
        fileHelper.write(ConstantUtil.PATH, list, false);
    }

    public List findAll() {
        return list;
    }

    public void delete(int id) {
        if(!list.removeIf(e-> e.getId() == id)){
            System.out.println("a");
        }
    }

    public List search(String name) {
        return list.stream().filter(e-> e.getName().equals(name)).collect(Collectors.toList());
    }
}
