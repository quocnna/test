package test.product_csv_file_demo.service;

import test.product_csv_file_demo.exception.NotFoundProductException;

import java.util.List;

public interface BaseService<T> {
    List<T> getAll();
    void add(T t);
    void delete(int id) throws NotFoundProductException;
    List<T> search(String name);
}
