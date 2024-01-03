package org.example;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import static org.example.VPNUtil.connectEwerk;

public class Main {
    private static final Logger LOGGER = LogManager.getLogger(Main.class);

    public static void main(String[] args) {


        connectEwerk();

//        LOGGER.info("start schedule");
//        SchedulerConnect.run("0 0 1/2 ? * *");
    }




}