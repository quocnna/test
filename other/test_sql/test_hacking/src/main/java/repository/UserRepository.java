package repository;

import conf.DBConnection;

import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;

public class UserRepository {
    private final String SELECT_BY_USER_AND_PASSWORD = "select * from username where username = '1' and password = '2'";
//    private final String SELECT_BY_USER_AND_PASSWORD = "select * from username where username = ? and password = ?";

    public boolean find(String user, String pass) {
        try (
            Statement st = DBConnection.getConnection().createStatement();
                ) {
            String query = SELECT_BY_USER_AND_PASSWORD.replace("1", user).replace("2", pass);

//                PreparedStatement st = DBConnection.getConnection().prepareStatement(SELECT_BY_USER_AND_PASSWORD);
//        ) {
//            st.setString(1,user);
//            st.setString(2,pass);
//            String query = SELECT_BY_USER_AND_PASSWORD.replace("1", user).replace("2", pass);
            System.out.println(query);
            return st.executeQuery(query).next();

/*            boolean b1 = st.executeQuery("select * from username where username = 'a' and password = '' or '1' = '1'").next();
            boolean b2 = st.executeQuery().next();

            return st.executeQuery().next();*/

        } catch (SQLException e) {
            e.printStackTrace();
        }

        return false;
    }
}
