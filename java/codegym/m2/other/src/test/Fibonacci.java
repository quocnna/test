package test;

public class Fibonacci {
    public static void main(String[] args) {
        int num = 10;
        for (int i = 0; i < num; i++) {
            System.out.println(fibonacci(i));
//            System.out.println(fibonacciWithRecursive(i));
//            System.out.println(isFibonacci(i));
        }
    }

    private static int fibonacci(int n){
        if(n == 0 || n == 1){
            return n;
        }

        int f0 = 0;
        int f1 = 1;
        int fn = 0;

        for(int i = 2; i <= n; i++){
            fn = f0 + f1;
            f0 = f1;
            f1 = fn;
        }

        return fn;
    }

    private static int fibonacciWithRecursive(int n){
        if(n == 0 || n == 1){
            return n;
        }

        return fibonacciWithRecursive(n - 2) + fibonacciWithRecursive(n - 1);
    }

    private static boolean isFibonacci(int n){
        return fibonacci(n) == n;
    }
}
