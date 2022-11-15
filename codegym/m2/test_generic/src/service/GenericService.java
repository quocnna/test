package service;

import exception.NotFoundException;
import util.CommonUtil;
import util.ConstantUtil;
import util.FeatureClass;
import util.FileHelper;
import util.RunTimeClass;

import java.lang.reflect.Field;
import java.util.*;
import java.util.stream.Collectors;

public class GenericService {
    private FileHelper fileHelper = new FileHelper();
    private List<Object> list;

    public GenericService() {
        list = mapToObject();
    }

    public void create(Object o) throws NoSuchFieldException, IllegalAccessException {
        //region get last id
        int lastId = 0;

        if (list.size() > 0) {
            Object oLast = list.get(list.size() - 1);
            lastId = (Integer)getValueByField(oLast, "id");
        }
        //endregion

        Field f = o.getClass().getDeclaredField("id");
        f.setAccessible(true);
        f.set(o, lastId + 1);
        list.add(o);
        fileHelper.write(FeatureClass.getPath(), list, false);
    }

    public void update(Object o) {
        Object t = getValueByField(o, "id");

        for (int i = 0; i < list.size(); i++) {
            Object s = getValueByField(list.get(i), "id");

            if (t.equals(s)) {
                list.set(i, o);
                break;
            }
        }

        fileHelper.write(FeatureClass.getPath(), list, false);
    }

    public List findAll() {
        return list;
    }

    public void delete(int id) throws NotFoundException {
        for (int i = 0; i < list.size(); i++) {
            Object s = getValueByField(list.get(i), "id");
            if (s.equals(id)) {
                list.remove(i);
                fileHelper.write(FeatureClass.getPath(), list, false);
                return;
            }
        }

        throw new NotFoundException("ID " + id + " could not found");
    }

    public List search(String field, String val){
        List<Object> result = new ArrayList<>();

        for (int i = 0; i < list.size(); i++) {
            Object s = getValueByField(list.get(i), field);
            String sVal = (String) s;
            if (sVal.contains(val)) {
                result.add(list.get(i));
            }
        }

        return result;
    }

    public void sort(String field, boolean isDESC){
        Collections.sort(list, (o1, o2) -> {
            double f1 = (Double) getValueByField(o1, field);
            double f2 = (Double) getValueByField(o2, field);

            return isDESC ? (int)(f2 -f1) : (int)(f1 -f2);
        });
    }

    private Object getValueByField(Object o, String field) {
        Object result = null;

        try {
            Class<?> clazz = o.getClass();
            Field f = clazz.getDeclaredField(field);
            f.setAccessible(true);
            result = f.get(o);
        } catch (Exception e) {
            e.printStackTrace();
        }

        return result;
    }

    private List mapToObject() {
        List result = new ArrayList();

        try {
            if(FeatureClass.getPath() != null){
                List<String> lines = fileHelper.read(FeatureClass.getPath());
                for (String line : lines) {
                    String[] tmp = line.split(",");

                    List<String> values = Arrays.stream(tmp).collect(Collectors.toList());
                    values.remove(0);
                    Optional<RunTimeClass> runTimeClass = CommonUtil.rts.stream().filter(e -> e.getEntityName().equals(tmp[0])).findFirst();
                    if(runTimeClass.isPresent()){
                        result.add(CommonUtil.createInstance(runTimeClass.get(), values));
                    }
                }
            }
        } catch (Exception e) {
            e.printStackTrace();
        }

        return result;
    }
}
