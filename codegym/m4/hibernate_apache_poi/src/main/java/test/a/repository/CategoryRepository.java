package test.a.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import test.a.model.Category;

public interface CategoryRepository extends JpaRepository<Category, Long> {
}
