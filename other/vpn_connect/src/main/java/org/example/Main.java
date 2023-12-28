package org.example;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import org.openqa.selenium.WebElement;

import java.io.BufferedWriter;
import java.io.FileWriter;

public class Main {
    private static final Logger LOGGER = LogManager.getLogger(Main.class);

    public static void main(String[] args) {
        LOGGER.info("start vpn pdv");
        connectVPN(CommonUtil.getValueProperties("command.pdv"));
        LOGGER.info("end vpn pdv");

        LOGGER.info("start vpn ewerk");
        try {
            WebAction.launchWeb();
            WebAction.navigate("chrome-extension://bhghoamapcdpbohphigoooaddinpkbai/view/popup.html");
            WebElement e = WebAction.getElementForVisibility("//*[@id=\"codes\"]/div[3]/a/div[@class='code timeout']");
            Thread.sleep(6000);
            String code = e.getText();
            BufferedWriter bufferedWriter = new BufferedWriter(new FileWriter(CommonUtil.getValueProperties("login.path")));
            bufferedWriter.write(CommonUtil.getValueProperties("user.ewerk"));
            bufferedWriter.newLine();
            bufferedWriter.write(CommonUtil.getValueProperties("pass.ewerk") + code);
            bufferedWriter.flush();
            bufferedWriter.close();
            connectVPN(CommonUtil.getValueProperties("command.ewerk"));
            WebAction.quit();
        } catch (Exception e) {
            LOGGER.info(e.getMessage());
            throw new RuntimeException(e);
        }
        LOGGER.info("end vpn ewerk");

        LOGGER.info("start schedule");
        SchedulerConnect.run("0 0 */3 ? * *");
    }

    private static void connectVPN(String command){
        try {
            ProcessBuilder processBuilder = new ProcessBuilder("cmd", "/c", command);
            Process process = processBuilder.start();
            int exitCode = process.waitFor();
            LOGGER.info("Command executed with exit code: " + exitCode);
        } catch (Exception e) {
            LOGGER.info(e.getMessage());
            throw new RuntimeException(e);
        }
    }
}