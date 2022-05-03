package cg.crud_template_basic.repository;

import cg.crud_template_basic.model.Product;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

import java.util.List;

public interface ProductRepository extends JpaRepository<Product, Integer> {
    Page<Product> findByNameContaining(String name, Pageable pageable);

    Page<Product> findByPriceGreaterThanEqual(double price, Pageable pageable);

    Page<Product> findByManufacturerContaining(String manufacturer, Pageable pageable);

    @Query(value = "select * from product where exp_date between :from and :to"
            , countQuery = "select count(*) from product where exp_date between :from and :to", nativeQuery= true)
    Page<Product> findByEXPDate(String from, String to, Pageable pageable);

    @Query(value="select p.* from product p join category c on p.category_id = c.id where c.name like ?1"
            , countQuery = "select count(p.id) from product p join category c on p.category_id = c.id where c.name like ?1", nativeQuery = true)
    Page<Product> findByCategoryName(String categoryName, Pageable pageable);

    @Query(value = "select p.* from product p\n" +
            "join category c on p.category_id= c.id\n" +
            "where p.name like ?1 or p.price like ?1 or p.exp_date like ?1 or p.manufacturer like ?1 or c.name like ?1"
            , countQuery= "select p.* from product p\n" +
            "join category c on p.category_id= c.id\n" +
            "where p.name like ?1 or p.price like ?1 or p.exp_date like ?1 or p.manufacturer like ?1 or c.name like ?1", nativeQuery= true)
    Page<Product> findAllByValue(String val, Pageable pageable);
}
