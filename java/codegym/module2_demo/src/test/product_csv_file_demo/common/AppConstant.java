package test.product_csv_file_demo.common;

public interface AppConstant {
    interface Path{
        String handProduct= "src/test/product_csv_file_demo/data/handProduct.csv";
        String authenticProduct= "src/test/product_csv_file_demo/data/authenticProduct.csv";
    }

    interface Regex{
        String handProductName= "\\w{5}";
        String authenticProductName= "\\w{3}";
    }
}
