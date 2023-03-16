public class Main {
    public static void main(String[] args) {
        String s = "hello";
        setA(s);
        System.out.println(s);
    }

    private static void setA(String a){
        a = "world";
    }
}
