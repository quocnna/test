package test.a.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import test.a.model.Product;

public interface ProductRepository extends JpaRepository<Product, Long> {
}
