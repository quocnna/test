package org.example;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

public class CommonUtil {
    private static final Logger logger = LogManager.getLogger(CommonUtil.class);

    public static String getValueProperties(String key)
    {
        try
        {
            InputStream is= CommonUtil.class.getClassLoader().getResourceAsStream("config.properties");
            Properties prop = new Properties();
            prop.load(is);
            assert is != null;
            is.close();

            return prop.getProperty(key);
        }
        catch(IOException e)
        {
            logger.info("Fail: oad value properties");
            throw new RuntimeException(e);
        }
    }
}
