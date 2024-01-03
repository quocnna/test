package vn.neo.util;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import org.openqa.selenium.WebElement;
import vn.neo.action.WebAction;

import java.io.BufferedWriter;
import java.io.FileWriter;

public class VPNUtil {
    private static final Logger LOGGER = LogManager.getLogger(VPNUtil.class);

    public static void connectEwerk() {
        try {
            LOGGER.info("disconnect ewerk");
            runCommand(CommonUtil.getValueProperties("disconnect.ewerk"));
            LOGGER.info("navigate to extention");
            WebAction.navigate("moz-extension://76020629-d0d4-4f8d-ab28-5b96bd3f418a/view/popup.html");
            WebElement e = WebAction.getElementForVisibility(AppConstant.FindBy.XPath,"//*[@id=\"codes\"]/div[3]/a/div[@class='code']");
            String code = e.getText();
            String tmp;

            do{
                Thread.sleep(2000);
                tmp = e.getText();
            }while (code.equals(tmp));

            LOGGER.info("write code: "+ tmp);
            BufferedWriter bufferedWriter = new BufferedWriter(new FileWriter(CommonUtil.getValueProperties("login.path")));
            bufferedWriter.write(CommonUtil.getValueProperties("user.ewerk"));
            bufferedWriter.newLine();
            bufferedWriter.write(CommonUtil.getValueProperties("pass.ewerk") + tmp);
            bufferedWriter.flush();
            bufferedWriter.close();
            LOGGER.info("connect ewerk");
            runCommand(CommonUtil.getValueProperties("connect.ewerk"));
            LOGGER.info("quit selenium");
            WebAction.quit();
        } catch (Exception e) {
            throw new RuntimeException(e);
        }
    }

    public static void runCommand(String command) {
        try {
            ProcessBuilder processBuilder = new ProcessBuilder("cmd", "/c", command);
            processBuilder.start();
            LOGGER.info("run cmd");
        } catch (Exception e) {
            LOGGER.info(e.getMessage());
            throw new RuntimeException(e);
        }
    }
}
