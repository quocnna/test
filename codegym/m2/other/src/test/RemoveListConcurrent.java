package test;

import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;

public class RemoveListConcurrent {
    public static void main(String[] args) {
        List<String> res = new ArrayList<String>();
        res.add("a");
        res.add("b");
        res.add("c");

        /*// error
        for (String s : res){
            res.remove(s);
        }*/

        /*// wrong because res.size() changed
        for (int i = 0; i < res.size(); i++) {
            res.remove(i);
        }*/

        /*// error IndexOutOfBoundsException: Index 2 out of bounds for length 1
        int size = res.size();
        for (int i = 0; i < size; i++) {
            res.remove(i);
        }*/

        // ok
        /*for (int i = res.size() -1; i >=0; i--) {
            res.remove(i);
        }*/

        /*// ok
        Iterator<String> iterator = res.iterator();
        while (iterator.hasNext()){
//            String s = iterator.next();
//            System.out.println(s);
//            iterator.remove();

            if(iterator.next().equals("a")){
                iterator.remove();
            }
        }*/

        res.removeIf(e -> e.equals("a"));

        /*for (int i = 0; i < myArray.size(); ) {
            String text = myArray.get(i);
            if (someCondition(text))
                myArray.remove(i);
            else
                i++;
        }*/


        /*List<Object> l = ...

        List<Object> iterationList = ImmutableList.copyOf(l);

        for (Object curr : iterationList) {
            if (condition(curr)) {
                l.remove(curr);
            }
        }*/


        System.out.println("end"+ res.size());
    }
}
