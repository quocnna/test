package org.example;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.firefox.FirefoxDriver;
import org.openqa.selenium.firefox.FirefoxOptions;
import org.openqa.selenium.firefox.FirefoxProfile;
import org.openqa.selenium.firefox.ProfilesIni;
import org.openqa.selenium.support.ui.ExpectedConditions;
import org.openqa.selenium.support.ui.WebDriverWait;

import java.io.File;
import java.time.Duration;

public class WebAction {
    private static final Logger LOGGER = LogManager.getLogger(WebAction.class);
    private static WebDriver webDriver;

    private static WebDriverWait wait;

    public static void launchWeb() {
/*        LOGGER.info("start chrome selenium");
        System.setProperty("webdriver.gecko.driver", CommonUtil.getValueProperties("selenium.path"));
        ProfilesIni profile = new ProfilesIni();
        FirefoxProfile fqProfile = profile.getProfile("qprofile");
        FirefoxOptions firefoxOptions = new FirefoxOptions();
        firefoxOptions.setProfile(fqProfile);
//        firefoxOptions.addArguments("-headless");
        webDriver = new FirefoxDriver(firefoxOptions);
        wait = new WebDriverWait(webDriver, Duration.ofMinutes(1));
        webDriver.manage().window().maximize();
        webDriver.manage().deleteAllCookies();
        webDriver.manage().timeouts().pageLoadTimeout(Duration.ofMinutes(1));
        webDriver.manage().timeouts().implicitlyWait(Duration.ofSeconds(40));*/


        ProfilesIni profile = new ProfilesIni();
        FirefoxProfile myprofile = profile.getProfile("default-release");
//        WebDriver driver = new FirefoxDriver(myprofile);

//        File file = new File(AppConstant.resources_path);
//        String profilePath = "C:\\Users\\quocnna.UNITEK\\AppData\\Roaming\\Mozilla\\Firefox\\Profiles\\4mcjrl8w.default-release-1688974778331";
//        FirefoxProfile profile = new FirefoxProfile(new File(profilePath));
        FirefoxOptions firefoxOptions = new FirefoxOptions();
        System.setProperty("webdriver.gecko.driver", CommonUtil.getValueProperties("selenium.path"));
        firefoxOptions.setProfile(myprofile);
//        firefoxOptions.addArguments("-headless");
        webDriver = new FirefoxDriver(firefoxOptions);
        wait = new WebDriverWait(webDriver, Duration.ofSeconds(30));
        webDriver.manage().window().maximize();
        webDriver.manage().deleteAllCookies();
        webDriver.manage().timeouts().pageLoadTimeout(Duration.ofMinutes(1));
        webDriver.manage().timeouts().implicitlyWait(Duration.ofSeconds(30));
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
