package service;

import java.util.List;

public interface ProductService<T> {
    void create(T t);
    List findAll();
    void delete(int id);
    List searchByName(String name);
}
