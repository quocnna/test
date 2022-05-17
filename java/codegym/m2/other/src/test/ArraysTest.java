package test;

import java.util.Arrays;

public class ArraysTest {
    private static int[] a = {23, 9, 84, 17, 5, 22};
    private static int[] b = {1, 2, 3, 4, 5};

    public static void main(String[] args) {
//        remove(2);
//
//        for (int i = 0; i < a.length - 1; i++) {
//            System.out.println(a[i]);
//        }

//        add(3, 62);

        combine(a, b);
    }

    private static void remove(int index) {
        for (int i = 0, j = 0; i < a.length; i++) {
            if (i != index) {
                a[j++] = a[i];
            }
        }
    }

    private static void add(int index, int value) {
        int[] b = Arrays.copyOf(a, a.length +1);

        for (int i = a.length; i > index; i--) {
                b[i] = a[i-1];
        }

        b[index] = value;

        a= b;
    }

    private static int[] combine(int[] a, int[] b){
        int[] c= new int[a.length + b.length];

        for (int i = 0; i < a.length; i++) {
            c[i] = a[i];
        }

        for (int i = 0; i < b.length; i++) {
            c[a.length + i] = b[i];
        }

        return c;
    }
}
