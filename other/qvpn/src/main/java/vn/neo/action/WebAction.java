package vn.neo.action;

import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.chrome.ChromeDriver;
import org.openqa.selenium.chrome.ChromeOptions;
import org.openqa.selenium.edge.EdgeDriver;
import org.openqa.selenium.edge.EdgeOptions;
import org.openqa.selenium.support.ui.ExpectedConditions;
import org.openqa.selenium.support.ui.WebDriverWait;
import vn.neo.util.AppConstant.FindBy;
import vn.neo.util.CommonUtil;

import java.io.File;
import java.time.Duration;
import java.util.List;

public class WebAction {
    private static WebDriver webDriver;

    public static WebDriver getWebDriver() {
        if (webDriver == null) {
            throw new NullPointerException();
        }

        return webDriver;
    }

    private static WebDriverWait wait;

    public static void launchWeb() {
        if (CommonUtil.getValueProperties("app.browser").equals("chrome")){
            ChromeOptions options = new ChromeOptions();
            options.addArguments("user-data-dir="+ CommonUtil.getValueProperties("data.edge"));
            options.addArguments("profile-directory="+ CommonUtil.getValueProperties("profile.edge"));
            options.addArguments("--headless=new");
            webDriver = new ChromeDriver(options);
        }
        else {
            EdgeOptions options = new EdgeOptions();
            options.addArguments("user-data-dir="+ CommonUtil.getValueProperties("data.edge"));
            options.addArguments("profile-directory="+ CommonUtil.getValueProperties("profile.edge"));
            options.addArguments("--headless=new");
            webDriver = new EdgeDriver(options);
        }

        wait = new WebDriverWait(webDriver, Duration.ofSeconds(30));
        webDriver.manage().window().maximize();
        webDriver.manage().deleteAllCookies();
        webDriver.manage().timeouts().pageLoadTimeout(Duration.ofMinutes(1));
        webDriver.manage().timeouts().implicitlyWait(Duration.ofSeconds(30));
    }

    public static void click(WebElement webElement) {
        webElement.click();
    }

    public static List<WebElement> getElements(FindBy findBy, String valueToFind) {
        By by = findBy(findBy, valueToFind);
        return webDriver.findElements(by);
    }

    public static WebElement getElementForClick(FindBy findBy, String valueToFind) {
        By by = findBy(findBy, valueToFind);
        wait.until(ExpectedConditions.elementToBeClickable(by));

        return webDriver.findElement(by);
    }

    public static WebElement getElementForVisibility(FindBy findBy, String valueToFind) {
        By by = findBy(findBy, valueToFind);

        return wait.until(ExpectedConditions.visibilityOfElementLocated(by));
    }

    public static WebElement waitElementVisibility(FindBy findBy, String valueToFind, Duration duration, int countLoop) {
        WebDriverWait webDriverWait = new WebDriverWait(webDriver, duration);
        By by = findBy(findBy, valueToFind);

        for (int i = 0; i < countLoop; i++) {
            try {
                return webDriverWait.until(ExpectedConditions.visibilityOfElementLocated(by));
            } catch (Exception e) {
                System.out.println(e.getMessage());
            }
        }

        return null;
    }

    public static WebElement waitElementVisibilityAlways(FindBy findBy, String valueToFind) throws InterruptedException {
        By by = findBy(findBy, valueToFind);
        WebElement res = null;
        boolean visibility = false;

        while (!visibility) {
            try {
                res = wait.until(ExpectedConditions.visibilityOfElementLocated(by));
                visibility = true;
            } catch (Exception e) {
                System.out.println(e.getMessage());
                System.out.println("still invisibility");
                webDriver.navigate().refresh();
                Thread.sleep(8000);
            }
        }

        return res;
    }

    public static void waitElementInvisibilityAlways(FindBy findBy, String valueToFind) throws InterruptedException {
        By by = findBy(findBy, valueToFind);
        boolean invisible = true;

        while (invisible) {
            try {
                invisible = wait.until(ExpectedConditions.invisibilityOfElementLocated(by));
            } catch (Exception e) {
                System.out.println(e.getMessage());
                System.out.println("still visibility");
                webDriver.navigate().refresh();
                Thread.sleep(5000);
            }
        }
    }

    public static void setText(WebElement webElement, String value) {
        webElement.clear();
        webElement.sendKeys(value);
    }

    public static void navigate(String url) {
        webDriver.navigate().to(url);
    }

    public static void close() {
        webDriver.close();
    }

    public static void quit() {
        webDriver.quit();
    }

    public static void refresh() {
        webDriver.navigate().refresh();
    }

    private static By findBy(FindBy findBy, String valueToFind) {
        return findBy == FindBy.Id ? By.id(valueToFind) :
                findBy == FindBy.Name ? By.name(valueToFind) : findBy == FindBy.TagName ? By.tagName(valueToFind) :
                        findBy == FindBy.ClassName ? By.className(valueToFind) :
                                findBy == FindBy.CssSelector ? By.cssSelector(valueToFind) :
                                        findBy == FindBy.XPath ? By.xpath(valueToFind) :
                                                findBy == FindBy.LinkText ? By.linkText(valueToFind) : By.partialLinkText(valueToFind);
    }
}
