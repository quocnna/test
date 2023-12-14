package cg.m3.cs_customer_mvn.repository;


import cg.m3.cs_customer_mvn.model.Customer;

import java.sql.*;
import java.util.ArrayList;
import java.util.List;

public class CustomerRepository {
    private static final String URL = "jdbc:mysql://localhost:3306/cs_customer";
    private static final String USER = "root";
    private static final String PASS = "12345";
    public static final String FINDALL = "select * from customer join customer_type on customer_type_id = customer_type.id";
    public static final String UPDATE = "update customer set name = ?, birthday = ?, gender = ?, card = ?, phone = ?" +
            ", email = ?, customer_type_id = ? where id = ?";
    public static final String INSERT = "insert into customer (name, birthday, gender, card, phone, email, customer_type_id)" +
            "values (?,?,?,?,?,?,?)";
    public static final String DELETE = "delete from customer where id = ?";

    public List<Customer> findAll() {
        ArrayList<Customer> customers = new ArrayList<>();

        try (
                Connection conn = getConnection();
                ResultSet resultSet = conn.prepareStatement(FINDALL).executeQuery()
        ) {
            while (resultSet.next()) {
                int id = resultSet.getInt(1);
                String name = resultSet.getString(2);
                Date birthday = resultSet.getDate(3);
                boolean gender = resultSet.getBoolean(4);
                String card = resultSet.getString(5);
                String phone = resultSet.getString(6);
                String email = resultSet.getString(7);
                String address = resultSet.getString(8);
                int customerTypeId = resultSet.getInt(9);
                String customerTypeName = resultSet.getString(10);
                customers.add(new Customer(id, name, birthday, gender, card, phone, email, address, customerTypeId, customerTypeName));
            }
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }

        return customers;
    }

    public boolean save(Customer customer) {
        try (
                Connection conn = getConnection();
                PreparedStatement preparedStatement = conn.prepareStatement(customer.id() > 0 ? UPDATE : INSERT);
        ) {
            preparedStatement.setString(1, customer.name());
            preparedStatement.setDate(2, new Date(customer.birthday().getTime()));
            preparedStatement.setBoolean(3, customer.gender());
            preparedStatement.setString(4, customer.card());
            preparedStatement.setString(5, customer.phone());
            preparedStatement.setString(6, customer.email());
            preparedStatement.setInt(7, customer.customerTypeId());

            if (customer.id() > 0) {
                preparedStatement.setInt(8, customer.id());
            }

            return preparedStatement.executeUpdate() > 0;
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    public boolean delete(int id) {
        try (
                Connection connection = getConnection();
                PreparedStatement statement = connection.prepareStatement(DELETE);
        ) {
            statement.setInt(1, id);
            return statement.executeUpdate() > 0;
        } catch (Exception e) {
            throw new RuntimeException(e);
        }
    }

    private Connection getConnection() {
        try {
            Class.forName("com.mysql.cj.jdbc.Driver");
            return DriverManager.getConnection(URL, USER, PASS);
        } catch (Exception e) {
            throw new RuntimeException(e);
        }
    }
}
