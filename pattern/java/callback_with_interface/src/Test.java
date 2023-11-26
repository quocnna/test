import model.Product;
import service.ProductPredicate;
import service.ProductService;

public class Test {
    private final static ProductService productService = new ProductService();

    public static void main(String[] args) {
//        productService.filterGreenApple().forEach(System.out::println);

        productService.search(new ProductPredicate() {
            @Override
            public boolean test(Product product) {
                return product.getColor().equals("gold") && product.getWeight() == 15;
            }
        }).forEach(System.out::println);


        System.out.println("---");

        productService.search(e -> e.getName().equals("lg") && e.getColor().equals("blue")).forEach(System.out::println);
    }
}
