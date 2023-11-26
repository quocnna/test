package service;

import model.Product;

import java.util.List;

public interface ProductPredicate {
    boolean test(Product product);
}
