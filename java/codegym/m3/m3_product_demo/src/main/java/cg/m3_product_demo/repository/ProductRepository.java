package cg.m3_product_demo.repository;

import cg.model.Product;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

public class ProductRepository {
    private final String SELECT_ALL = "select * from product p join category c on p.category_id = c.id";
    private final String SELECT_BY_ID = "select * from product p join category c on p.category_id = c.id where p.id = ?";

    private Connection getConnection() {
        try {
            Class.forName("com.mysql.jdbc.Driver");
            return DriverManager.getConnection("jdbc:mysql://localhost:3306/m3_product", "root", "40forever");
        }
        catch (Exception e){
            e.printStackTrace();
        }

        return null;
    }

    public int create(Product product) {
        return 0;
    }

    public List findAll() {
        List result = new ArrayList();

        try(  PreparedStatement statement = getConnection().prepareStatement(SELECT_ALL)) {
            ResultSet rs = statement.executeQuery();
            while(rs.next()) {
                int id = rs.getInt(1);
                String name = rs.getString(2);
                Double price = rs.getDouble(3);
                int quantity = rs.getInt(4);
                String color = rs.getString(5);
                String description = rs.getString(6);
                int categoryId= rs.getInt(7);
                String categoryName = rs.getString(9);
                result.add(new Product(id, name, price, quantity, color, description, categoryId, categoryName));
            }
        }
        catch (Exception e){
            e.printStackTrace();
        }

        return result;
    }

    public Product findById(int id){
        try(PreparedStatement statement = getConnection().prepareStatement(SELECT_BY_ID)){
            statement.setInt(1, id);
            ResultSet rs = statement.executeQuery();
            while (rs.next()){
                String name = rs.getString(2);
                Double price = rs.getDouble(3);
                int quantity = rs.getInt(4);
                String color = rs.getString(5);
                String description = rs.getString(6);
                int categoryId= rs.getInt(7);
                String categoryName = rs.getString(9);
                return new Product(id, name, price, quantity, color, description, categoryId, categoryName);
            }
        }
        catch (Exception e) {
            e.printStackTrace();
        }

        return null;
    }

    public int delete(int id) {
        return 0;
    }

    public List searchByName(String name) {
        return null;
    }
}
