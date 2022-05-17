package test;

import java.util.Arrays;

public class FindSecondValue {
    private static int[] a = {23, 9, 84, 17, 5, 22};

    public static void main(String[] args) {
        int res= findSecondValue3();
        System.out.println(res);
    }

/*
    Time Complexity: O(n log n).
*/
    private static int findSecondValue1(){
        if (a.length < 2) {
            System.out.print(" Khong hop le ");
            return Integer.MIN_VALUE;
        }

        Arrays.sort(a);

        for (int i = a.length-2; i > 0; i--) {
            if(a[i] != a[a.length-1]){
                return a[i];
            }
        }

        return 0;
    }

/*
    Time Complexity: O(n).
*/
    private static int findSecondValue2(){
        int largest = a[0];
        for (int i = 1; i < a.length; i++) {
            if(largest < a[i]){
                largest = a[i];
            }
        }

        int tmp = a[0];
        for (int i = 0; i < a.length; i++) {
            if(tmp < a[i] && a[i] != largest){
                tmp = a[i];
            }

//            if (a[i] != largest)
//                tmp = Math.max(tmp, a[i]);
        }

        return tmp;
    }


/*
    Time Complexity: O(n log n).
*/
    private static int findSecondValue3(){
        int i, first, second;
        first = second = Integer.MIN_VALUE;

        for (i = 0; i < a.length; i++) {
            if (a[i] > first) {
                second = first;
                first = a[i];
            }

            if (a[i] < first && a[i] > second) {
                second = a[i];
            }
        }

        return second;
    }
}
