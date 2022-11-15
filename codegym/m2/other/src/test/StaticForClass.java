package test;

// cannot mark static class for top level
public class StaticForClass {
    static class child{
      private int a;
    }

    public static void main(String[] args) {
        System.out.println("a");
    }
}
