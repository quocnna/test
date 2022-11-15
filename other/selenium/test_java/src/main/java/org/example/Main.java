package org.example;

import org.openqa.selenium.By;
import org.openqa.selenium.Dimension;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.Keys;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.chrome.ChromeDriver;

public class Main {
    public static void main(String[] args) throws InterruptedException {

        System.setProperty("webdriver.chrome.driver", "chromedriver.exe");
        WebDriver driver = new ChromeDriver();
//        driver.manage().window().maximize();
//        driver.get("http://google.com");
//        driver.manage().window().maximize();
//        System.out.println("Hello world!");
//        WebElement txtSearch = driver.findElement(By.name("q"));
//        txtSearch.sendKeys("devopsify vietnam");
//        txtSearch.sendKeys(Keys.ENTER);
//        System.out.println(driver.getTitle());

        JavascriptExecutor js = (JavascriptExecutor) driver;
        driver.get("https://shopee.vn/S%E1%BB%AFa-t%E1%BA%AFm-LIFEBUOY-800g-v%E1%BB%9Bi-Ion-b%E1%BA%A1c-c%C3%B9ng-%C4%91%E1%BB%81-kh%C3%A1ng-da-i.111138057.2855276371");
        driver.manage().window().setSize(new Dimension(1936, 1056));


//        js.executeScript("alert('Successfully Logged In');");
        WebElement button = driver.findElement(By.xpath("//*[@id=\"main\"]/div/div[2]/div[1]/div/div/div/div[2]/div[3]/div/div[5]/div/div/button[2]"));
        Thread.sleep(2000);
//        js.executeScript("document.getElementsByClassName('_1MGNbJ _1eS5m1')[0].setAttribute('value','123456');");

        js.executeScript("arguments[0].click();", button);
        Thread.sleep(2000);
//        js.executeScript("alert('Welcome To Anh Tester - Automation Testing');");
        driver.findElement(By.xpath("//div/input")).sendKeys(Keys.CONTROL + "a");
        driver.findElement(By.xpath("//div/input")).sendKeys(Keys.DELETE);

//        driver.findElement(By.xpath("//div/input")).click();
        driver.findElement(By.xpath("//div/input")).sendKeys("25");


//
//        WebElement element = driver.findElement(By.xpath( "//div[text()='Client_1_name']"));
        driver.findElement(By.xpath("//body/div/div/div[2]/div/div/div")).click();
    }
}