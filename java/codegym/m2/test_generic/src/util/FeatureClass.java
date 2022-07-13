package util;

import java.util.Map;

public class FeatureClass {
    private static String searchBy;
    private static Map<String, String> sort;
    private static Map<String, String> valid;
    private static String[] menu;
    private static String path;

    public static String getSearchBy() {
        return searchBy;
    }

    public static void setSearchBy(String searchBy) {
        FeatureClass.searchBy = searchBy;
    }

    public static Map<String, String> getSort() {
        return sort;
    }

    public static void setSort(Map<String, String> sort) {
        FeatureClass.sort = sort;
    }

    public static Map<String, String> getValid() {
        return valid;
    }

    public static void setValid(Map<String, String> valid) {
        FeatureClass.valid = valid;
    }

    public static String[] getMenu() {
        return menu;
    }

    public static void setMenu(String[] menu) {
        FeatureClass.menu = menu;
    }

    public static String getPath() {
        return path;
    }

    public static void setPath(String path) {
        FeatureClass.path = path;
    }
}
