package org.example.viblo.core;

public class Test {
    public static void main(String[] args) {
        ONewsAgency observable = new ONewsAgency();
        ONewsChannel observer1 = new ONewsChannel();
        ONewsChannel observer2 = new ONewsChannel();
        observer1.setNews("o1");
        observer2.setNews("o2");

        observable.addObserver(observer1);
        observable.addObserver(observer2);
        observable.setNews("news");

        System.out.println(observer1.getNews());
        System.out.println(observer2.getNews());
        System.out.println("==========");

        observable.deleteObserver(observer2);
        observable.setNews("new1");
        System.out.println(observer1.getNews());
        System.out.println(observer2.getNews());
    }
}
