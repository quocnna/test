package service;

import model.Product;
import util.ConstantUtil;
import util.FileHelper;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public class ProductService {
    private List<Product> products = new ArrayList<>();
    private FileHelper fileHelper = new FileHelper();

    public void create(Product product){
        //region get last id
        int lastId = 0;

        if(products.size()> 0){
            lastId= products.get(products.size()-1).getId();
        }

        product.setId(lastId + 1);
        //endregion
        products.add(product);
        fileHelper.write(ConstantUtil.PRODUCT_PATH, Collections.singletonList(product), true);
    }

    public List findAll(){
        return products;
    }

    public void delete(int id){
        for (int i = 0; i < products.size(); i++) {
            if(products.get(i).getId() == id){
                products.remove(i);
                return;
            }
        }
    }

    public List searchByName(String name){
        List result = new ArrayList();
        for (int i = 0; i < products.size(); i++) {
            if(products.get(i).getName().contains(name)){
                result.add(products.get(i));
            }
        }

        return  result;
    }
}
