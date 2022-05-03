package cg.crud_template_basic.repository;

import cg.crud_template_basic.model.Category;
import org.springframework.data.jpa.repository.JpaRepository;

public interface CategoryRepository extends JpaRepository<Category, Integer> {
}
