package service;

import model.Product;

import java.util.ArrayList;
import java.util.List;

public class ProductService {
    private List<Product> products = new ArrayList<>();

    public void create(Product product){
        //region make id increment
        int idIncrement = 1;

        if(products.size()> 0){
            idIncrement= products.get(products.size()-1).getId() + 1;
        }

        product.setId(idIncrement);
        //endregion
        products.add(product);
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
