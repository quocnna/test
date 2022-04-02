package furama_resort.service;

import java.util.List;
import java.util.Optional;

public interface BaseService<T> {
    void save(T t);
    List<T> getAll();
    void delete(int id);
    Optional<T> get(int id);
}
