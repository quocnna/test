//package org.example;
//
//import org.apache.logging.log4j.LogManager;
//import org.apache.logging.log4j.Logger;
//import org.quartz.Job;
//import org.quartz.JobExecutionContext;
//
//import java.net.HttpURLConnection;
//import java.net.URL;
//
//public class JobTestRequest implements Job {
//    private static final Logger LOGGER = LogManager.getLogger(Main.class);
//
//    @Override
//    public void execute(JobExecutionContext context) {
//        try {
//            LOGGER.info("start job test connect");
//            URL url = new URL("https://jira.it.ewerk.com/browse/DPDVRV-18");
//            HttpURLConnection con = (HttpURLConnection) url.openConnection();
//            con.setRequestMethod("GET");
//            con.setConnectTimeout(5000);
//            con.setReadTimeout(5000);
//            int status = con.getResponseCode();
//            LOGGER.info("end job test connect with status:" + status);
//            con.disconnect();
//        } catch (Exception e) {
//            LOGGER.info("end job test connect with error " + e.getMessage());
//        }
//    }
//}
