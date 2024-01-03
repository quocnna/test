package org.example;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

public class VPNUtil {
    private static final Logger LOGGER = LogManager.getLogger(VPNUtil.class);

    public static void connectPDV() {
        LOGGER.info("start vpn pdv");
        runCommand(CommonUtil.getValueProperties("command.pdv"));
        LOGGER.info("end vpn pdv");
    }

    public static void connectEwerk(){
        LOGGER.info("start vpn ewerk");
        try {
            LOGGER.info("launch web");
            WebAction.launchWeb();
            Thread.sleep(2000);
            LOGGER.info("navigate to extention");
            WebAction.navigate("moz-extension://ea99d529-354e-4883-aaf6-27b4c25126f1/view/popup.html");
//            WebElement e = WebAction.getElementForVisibility("//*[@id=\"codes\"]/div[3]/a/div[@class='code']");
//            String code = e.getText();
//            String tmp;
//
//            do{
//                Thread.sleep(2000);
//                tmp = e.getText();
//            }while (code.equals(tmp));
//
//            LOGGER.info("write code: "+ tmp);
//            BufferedWriter bufferedWriter = new BufferedWriter(new FileWriter(CommonUtil.getValueProperties("login.path")));
//            bufferedWriter.write(CommonUtil.getValueProperties("user.ewerk"));
//            bufferedWriter.newLine();
//            bufferedWriter.write(CommonUtil.getValueProperties("pass.ewerk") + tmp);
//            bufferedWriter.flush();
//            bufferedWriter.close();
//            LOGGER.info("connect ewerk");
//            connectVPN(CommonUtil.getValueProperties("command.ewerk"));
//            LOGGER.info("quit selenium");
            WebAction.quit();
        } catch (Exception e) {
            LOGGER.info(e.getMessage());
            throw new RuntimeException(e);
        }
        LOGGER.info("end vpn ewerk");
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
