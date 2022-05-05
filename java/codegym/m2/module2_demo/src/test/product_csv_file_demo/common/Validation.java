package test.product_csv_file_demo.common;

import test.product_csv_file_demo.common.AppConstant.Regex;

import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class Validation {
    private static Pattern pattern;
    private static Matcher matcher;

    public static boolean checName(String name, boolean isHandProduct){
        pattern= Pattern.compile(isHandProduct? Regex.handProductName: Regex.authenticProductName);
        matcher= pattern.matcher(name);
        return matcher.matches();
    }
}
