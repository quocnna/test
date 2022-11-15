package stratery_pattern;

import java.util.List;

public class Test {
    private static AppleService appleService = new AppleService();

    public static void main(String[] args) {
//       List<Apple> apples = appleService.search(new ApplePredicate() {
//           @Override
//           public boolean test(Apple apple) {
//               return apple.getColor().equals("blue");
//           }
//       });

//        List<Apple> apples = appleService.search(e -> e.getColor().equals("blue"));

        List<Apple> apples = appleService.search1(e -> e.getColor().equals("blue"));


        apples.forEach(System.out::println);
    }
}
