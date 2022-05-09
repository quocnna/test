package service;

import model.AuthenticProduct;
import util.CommonUtil;
import util.ConstantUtil;

import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

public class AuthenticService implements ProductService<AuthenticProduct>{
    private List<AuthenticProduct> authenticProducts= new ArrayList<>();

    public AuthenticService() {
        authenticProducts = mapToAuthenticProducts();
    }

    public void create(AuthenticProduct authentic){
        int lastId = CommonUtil.getLastProductId(authenticProducts);
        authentic.setId(++lastId);
        authenticProducts.add(authentic);

        CommonUtil.getFileHelper().write(ConstantUtil.PATH.AUTHENTIC, authenticProducts, false);
    }

    public List findAll() {
        return authenticProducts;
    }

    public void delete(int id){
        if(authenticProducts.removeIf(e -> e.getId() == id)){
            CommonUtil.getFileHelper().write(ConstantUtil.PATH.AUTHENTIC, authenticProducts, false);
        }
    }

    @Override
    public List searchByName(String name) {
        return authenticProducts.stream().filter(e -> e.getName().equals(name)).collect(Collectors.toList());
    }

    private List mapToAuthenticProducts(){
        List result = new ArrayList();

        List<String> lines = CommonUtil.getFileHelper().read(ConstantUtil.PATH.AUTHENTIC);
        for(String line : lines){
            String tmp[] = line.split(",");
            int id = Integer.parseInt(tmp[0]);
            String name = tmp[1];
            Double price = Double.valueOf(tmp[2]);
            String manufacturer = tmp[3];
            int granteeByYear = Integer.parseInt(tmp[4]);
            result.add(new AuthenticProduct(id, name, price, manufacturer, granteeByYear));
        }

        return result;
    }
}
