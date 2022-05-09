package service;

import model.HandProduct;
import util.CommonUtil;
import util.ConstantUtil;

import java.util.ArrayList;
import java.util.List;

public class HandService implements ProductService<HandProduct>{
    private List<HandProduct> handProducts= new ArrayList<>();

    public HandService() {
        handProducts = mapToHandProducts();
    }

    @Override
    public void create(HandProduct handProduct) {
        int lastId = CommonUtil.getLastProductId(handProducts);
        handProduct.setId(++lastId);
        handProducts.add(handProduct);

        CommonUtil.getFileHelper().write(ConstantUtil.PATH.HAND, handProducts, false);
    }

    @Override
    public List findAll() {
        return handProducts;
    }

    @Override
    public void delete(int id) {
        if(handProducts.removeIf(e -> e.getId() == id)){
            CommonUtil.getFileHelper().write(ConstantUtil.PATH.HAND, handProducts, false);
        }
    }

    @Override
    public List searchByName(String name) {
        return null;
    }

    private List mapToHandProducts(){
        List result = new ArrayList();

        List<String> lines = CommonUtil.getFileHelper().read(ConstantUtil.PATH.HAND);
        for(String line : lines){
            String tmp[] = line.split(",");
            int id = Integer.parseInt(tmp[0]);
            String name = tmp[1];
            Double price = Double.valueOf(tmp[2]);
            String manufacturer = tmp[3];
            String country = tmp[4];
            String status = tmp[5];
            result.add(new HandProduct(id, name, price, manufacturer, country, status));
        }

        return result;
    }
}
