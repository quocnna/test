package org.example.viblo.core1;

import static org.junit.jupiter.api.Assertions.assertEquals;

public class Test {
    public static void main(String[] args) {
        PCLNewsAgency observable = new PCLNewsAgency();
        PCLNewsChannel observer = new PCLNewsChannel();

        observable.addPropertyChangeListener(observer);
        observable.setNews("news");

        assertEquals(observer.getNews(), "news");

    }
}
