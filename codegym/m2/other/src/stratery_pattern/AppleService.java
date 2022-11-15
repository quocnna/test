package stratery_pattern;

import java.util.ArrayList;
import java.util.List;
import java.util.function.Predicate;

public class AppleService {
    private List<Apple> apples = new ArrayList<>();

    public AppleService() {
        apples.add(new Apple(1,"blue",11,22));
        apples.add(new Apple(2,"y",3,44));
        apples.add(new Apple(3,"red",4,7));
        apples.add(new Apple(4,"b",5,8));
        apples.add(new Apple(5,"w",6,9));
    }


    public List<Apple> search(ApplePredicate applePredicate) {
        List<Apple> result = new ArrayList<>();

        for (int i = 0; i < apples.size(); i++) {
            if(applePredicate.test(apples.get(i))){
                result.add(apples.get(i));
            }
        }

        return result;
    }

    public List<Apple> search1(Predicate<Apple> predicateapplePredicate) {
        List<Apple> result = new ArrayList<>();

        for (int i = 0; i < apples.size(); i++) {
            if(predicateapplePredicate.test(apples.get(i))){
                result.add(apples.get(i));
            }
        }

        return result;
    }
}
