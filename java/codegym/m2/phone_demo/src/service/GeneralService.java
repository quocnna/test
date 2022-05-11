package service;

import exception.NotFoundException;
import model.BaseEntity;
import util.CommonUtil;
import util.ConstantUtil;
import util.FileHelper;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.stream.Collectors;

public class GeneralService {
    private List<BaseEntity> list;
    private FileHelper fileHelper = new FileHelper();

    public GeneralService() {
        list = mapToObject();
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

    public void delete(int id) throws NotFoundException {
        if(!list.removeIf(e-> e.getId() == id)){
            throw new NotFoundException("ID " + id + " could not found");
        }
    }

    public List search(String name) {
        return list.stream().filter(e-> e.getName().equals(name)).collect(Collectors.toList());
    }

    private List mapToObject() {
        List result = new ArrayList();

        try {
            List<String> lines = fileHelper.read(ConstantUtil.PATH);
            for (String line : lines) {
                String[] tmp = line.split(",");

                List<String> values = Arrays.stream(tmp).collect(Collectors.toList());
                values.remove(0);
                result.add(CommonUtil.createInstance(tmp[0], values));
            }
        } catch (Exception e) {
            try {
                Files.deleteIfExists(Paths.get(ConstantUtil.PATH));
            } catch (IOException ioException) {
                ioException.printStackTrace();
            }
        }

        return result;
    }
}
