package test;

import java.util.Scanner;

public class Prime100 {
    public static void main(String[] args) {
        Scanner scanner= new Scanner(System.in);
        System.out.println("Input how many prime: ");
        int num= scanner.nextInt();
        int count= 0;
        int tmp= 1;
        do{
            tmp++;

            boolean res= true;
            for (int i = 2; i < tmp; i++) {
                if(tmp%i== 0){
                    res= false;
                    break;
                }
            }

            if(res){
                System.out.println(tmp);
                count++;
            }

//            if(isPrime(tmp)){
//                System.out.println(tmp);
//                count++;
//            }
        }while (num>count);
    }

    private static boolean isPrime(int val){
        boolean res= true;
        for (int i = 2; i < val; i++) {
            if(val%i==0)
                return false;
        }
        return res;
    }

//    public static void main(String[] args) {
//        int i=1;
//        do{
//            i++;
//            if(checkPrimeNumber(i))
//                System.out.println(i);
//        }while (i<100);
//    }
//    private static boolean checkPrimeNumber(int a){
//        for (int i = 2; i < a; i++)
//            if(a%i==0)
//                return false;
//        return true;
//    }
}
