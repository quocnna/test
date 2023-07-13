package test;

import stratery_pattern.Apple;
import sun.misc.Unsafe;

public class MemoryAddress {
    public static void main(String[] args) {
        int a = 10;
        System.out.println(Integer.toBinaryString(a));

        Apple apple = new Apple(1, "y", 11, 11);
        System.out.println(apple);
        System.out.println(apple.hashCode());
    }


}
