package cg.crud_template_basic.repository;

import cg.crud_template_basic.model.Product;
import org.springframework.data.jpa.repository.JpaRepository;

public interface ProductRepository extends JpaRepository<Product, Integer> {
}
