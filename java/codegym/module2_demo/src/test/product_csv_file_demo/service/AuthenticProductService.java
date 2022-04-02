package test.product_csv_file_demo.service;

import test.product_csv_file_demo.common.AppConstant;
import test.product_csv_file_demo.common.FileHelper;
import test.product_csv_file_demo.model.AuthenticProduct;

import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

public class AuthenticProductService implements BaseService<AuthenticProduct> {
    private FileHelper<AuthenticProduct> fileHelper= new FileHelper<>();
    private List<AuthenticProduct> authenticProducts= getFromFile();

    @Override
    public List<AuthenticProduct> getAll() {
        return authenticProducts;
    }

    @Override
    public void add(AuthenticProduct authenticProduct) {
        int id= authenticProducts.size()>0 ? authenticProducts.get(authenticProducts.size() -1).getId()+ 1: 1;
        authenticProduct.setId(id);
        authenticProducts.add(authenticProduct);
    }

    @Override
    public void delete(int id) {
        authenticProducts.removeIf(e-> e.getId()== id);
    }

    @Override
    public List<AuthenticProduct> search(String name) {
        return authenticProducts.stream().filter(e-> e.getName().contains(name)).collect(Collectors.toList());
    }

    private List<AuthenticProduct> getFromFile(){
        List<AuthenticProduct> res= new ArrayList<>();

        List<String> lines= fileHelper.read(AppConstant.Path.authenticProduct);
        if(lines.size()> 0){
            for (String line: lines){
                String[] tmp= line.split(",");
                int id= Integer.parseInt(tmp[0]);
                String name= tmp[1];
                double price= Double.parseDouble(tmp[2]);
                String manufacture= tmp[3];
                int year=  Integer.parseInt(tmp[4]);
                AuthenticProduct authenticProduct= new AuthenticProduct(id, name, price, manufacture, year);
                res.add(authenticProduct);
            }
        }

        return res;
    }
}
