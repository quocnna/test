package vn.neo;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;
import vn.neo.action.WebAction;
import vn.neo.util.VPNUtil;

public class Main {
    private static final Logger LOGGER = LogManager.getLogger(Main.class);

    public static void main(String[] args)  {
        try {
            WebAction.launchWeb();
            VPNUtil.connectEwerk();
        } catch (Exception e) {
            LOGGER.info(e.getMessage());
            throw new RuntimeException(e);
        }
    }
}