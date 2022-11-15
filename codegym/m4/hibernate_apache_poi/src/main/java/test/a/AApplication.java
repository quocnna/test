package test.a;

import org.apache.poi.ss.usermodel.Sheet;
import org.apache.poi.ss.usermodel.Workbook;
import org.apache.poi.ss.usermodel.WorkbookFactory;
import org.apache.poi.xssf.streaming.SXSSFWorkbook;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import test.a.model.Category;
import test.a.model.Product;
import test.a.repository.CategoryRepository;
import test.a.repository.ProductRepository;

import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.util.List;

@SpringBootApplication
@Controller
public class AApplication {
    @Autowired
    private CategoryRepository categoryRepository;
    @Autowired
    private ProductRepository productRepository;

    public static void main(String[] args) {
        SpringApplication.run(AApplication.class, args);
    }

    @GetMapping("a")
    public String a() throws IOException {
        List<Category> categories = categoryRepository.findAll();
        List<Product> products = productRepository.findAll();

        FileInputStream fis = new FileInputStream("D:\\ATTP 25.7.2022 - TT.xlsx");
        // Workbook wb = new SXSSFWorkbook();
        Workbook wb = WorkbookFactory.create(fis);
        Sheet sh = wb.getSheet("Sheet1");
        int rowNum = sh.getLastRowNum();
        return "a";
    }
}
