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
            WebElement e = WebAction.getElementForVisibility("//*[@id=\"codes\"]/div[3]/a/div[@class='code']");
            String code = e.getText();
            String tmp;

            do{
                Thread.sleep(2000);
                tmp = e.getText();
            }while (code.equals(tmp));

            BufferedWriter bufferedWriter = new BufferedWriter(new FileWriter(CommonUtil.getValueProperties("login.path")));
            bufferedWriter.write(CommonUtil.getValueProperties("user.ewerk"));
            bufferedWriter.newLine();
            bufferedWriter.write(CommonUtil.getValueProperties("pass.ewerk") + tmp);
            bufferedWriter.flush();
            bufferedWriter.close();
            connectVPN(CommonUtil.getValueProperties("command.ewerk"));
            WebAction.quit();
        } catch (Exception e) {
            LOGGER.info(e.getMessage());
            throw new RuntimeException(e);
        }
        LOGGER.info("end vpn ewerk");
        System.exit(0);

//        LOGGER.info("start schedule");
//        SchedulerConnect.run("0 0 */4 ? * *");
    }

    private static void connectVPN(String command){
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