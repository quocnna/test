package org.example.viblo;


import static org.junit.jupiter.api.Assertions.assertEquals;

public class Test {
    public static void main(String[] args) {
        NewsAgency observable = new NewsAgency();
        NewsChannel observer = new NewsChannel();
        observer.setNews("test1");

        observable.addObserver(observer);
        observable.setNews("news");

        System.out.println(observer.getNews());
        assertEquals(observer.getNews(), "news");
    }
}
