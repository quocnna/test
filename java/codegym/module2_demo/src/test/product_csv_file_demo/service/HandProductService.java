package test.product_csv_file_demo.service;

import test.product_csv_file_demo.common.FileHelper;
import test.product_csv_file_demo.exception.NotFoundProductException;
import test.product_csv_file_demo.model.HandProduct;
import test.product_csv_file_demo.common.AppConstant.*;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.stream.Collectors;

public class HandProductService implements BaseService<HandProduct> {
    private FileHelper<HandProduct> fileHelper= new FileHelper<>();
    private List<HandProduct> handProducts= getFromFile();

    @Override
    public List<HandProduct> getAll() {
        return handProducts;
    }

    @Override
    public void add(HandProduct handProduct) {
        int id= handProducts.size()> 0? handProducts.get(handProducts.size() -1).getId() +1: 1;
        handProduct.setId(id);
        handProducts.add(handProduct);
        fileHelper.write(Path.handProduct, Arrays.asList(handProduct), true);
    }

    @Override
    public void delete(int id) throws NotFoundProductException {
        if(!handProducts.removeIf(e-> e.getId()== id))
            throw new NotFoundProductException("This ID is not exists");
        fileHelper.write(Path.handProduct, handProducts, false);
    }

    @Override
    public List<HandProduct> search(String name) {
        return handProducts.stream().filter(e-> e.getName().contains(name)).collect(Collectors.toList());
    }

    private List<HandProduct> getFromFile(){
        List<HandProduct> res= new ArrayList<>();

        List<String> lines= fileHelper.read(Path.handProduct);
        if(lines.size()> 0){
            for (String line: lines){
                String[] tmp= line.split(",");
                int id= Integer.parseInt(tmp[0]);
                String name= tmp[1];
                double price= Double.parseDouble(tmp[2]);
                String manufacture= tmp[3];
                String country= tmp[4];
                String status= tmp[5];
                HandProduct handProduct= new HandProduct(id, name, price, manufacture, country, status);
                res.add(handProduct);
            }
        }

        return res;
    }
}
