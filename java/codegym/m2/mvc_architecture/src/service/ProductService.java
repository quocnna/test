package service;

import model.Product;

import java.util.ArrayList;
import java.util.List;

public class ProductService {
    private List<Product> products= new ArrayList<Product>();

    public List<Product> getAll(){
        return products;
    }

    public void save(Product product){
        if(product.getId() > 0){
            int index= products.indexOf(product);
            products.set(index, product);
        }
        else {
            products.add(product);
        }
    }

    public boolean delete(int id){
        return products.removeIf(e-> e.getId() == id);
    }
}
