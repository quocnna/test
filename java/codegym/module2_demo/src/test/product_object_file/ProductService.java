package test.product_object_file;

import java.io.*;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.stream.Collectors;

public class ProductService {
    private static final String path= "src/test.product_object_file/product.bin";

    private static List<Product> products= new ArrayList<>();

    public ProductService(){
        products= getAll();
    }

    public void add(Product product){
        try {
            product.setId(products.size() +1);
            products.add(product);
            ObjectOutputStream  out = new ObjectOutputStream(new FileOutputStream(path));
            out.writeObject(products);
        }
       catch (IOException e){
            e.printStackTrace();
       }
    }

    public List<Product> getAll(){
        List<Product> res= new ArrayList<>();
        try{
            File file= new File(path);
            if(!file.exists())
                file.createNewFile();

            if(file.length()> 0){
                ObjectInputStream input = new ObjectInputStream(new FileInputStream(path));
                res =(List<Product>) input.readObject();
            }
        }
        catch (IOException | ClassNotFoundException e){
            e.printStackTrace();
        }

        return res;
    }

    public List<Product>  searchByName(String name){
        List<Product> res= new ArrayList<>();
        int size= products.size();
        for (int i = 0; i < size; i++) {
            if(products.get(i).getName().contains(name))
                res.add(products.get(i));
        }
        return res;
    }

    public List<Product> searchFromPrice(double price){
        return products.stream().filter(e-> e.getPrice() > price).collect(Collectors.toList());
    }

    public void update(Product product){
        try {
            int index= products.indexOf(product);
            products.set(index, product);
            ObjectOutputStream  out = new ObjectOutputStream(new FileOutputStream(path));
            out.writeObject(products);
        }
        catch (IOException e){
            e.printStackTrace();
        }
    }

    public List<Product> sort(){
        Collections.sort(products);
        return products;
    }
}
