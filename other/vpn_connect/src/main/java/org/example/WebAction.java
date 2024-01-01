package org.example;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.chrome.ChromeDriver;
import org.openqa.selenium.chrome.ChromeOptions;
import org.openqa.selenium.support.ui.ExpectedConditions;
import org.openqa.selenium.support.ui.WebDriverWait;

import java.time.Duration;

public class WebAction {
    private static final Logger LOGGER = LogManager.getLogger(WebAction.class);
    private static WebDriver webDriver;

    private static WebDriverWait wait;

    public static void launchWeb() {
        LOGGER.info("start chrome selenium");
//        String chromeDriverPath = Objects.requireNonNull(WebAction.class.getClassLoader().getResource("chromedriver.exe")).getPath();
        System.setProperty("webdriver.chrome.driver", CommonUtil.getValueProperties("selenium.path"));
        ChromeOptions chromeOptions = new ChromeOptions();
        chromeOptions.addArguments("user-data-dir=" + CommonUtil.getValueProperties("data.chrome"));
        chromeOptions.addArguments("profile-directory="+ CommonUtil.getValueProperties("profile.chrome"));
        chromeOptions.addArguments("--headless=new");
        webDriver = new ChromeDriver(chromeOptions);
        wait = new WebDriverWait(webDriver, Duration.ofMinutes(1));
        webDriver.manage().window().maximize();
        webDriver.manage().deleteAllCookies();
        webDriver.manage().timeouts().pageLoadTimeout(Duration.ofMinutes(1));
        webDriver.manage().timeouts().implicitlyWait(Duration.ofSeconds(40));
    }

    public static void navigate(String url) {
        webDriver.navigate().to(url);
    }

    public static void quit() {
        webDriver.quit();
    }

    public static WebElement getElementForVisibility(String valueToFind) {
        return wait.until(ExpectedConditions.visibilityOfElementLocated(By.xpath(valueToFind)));
    }
}
