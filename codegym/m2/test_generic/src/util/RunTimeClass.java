package util;

import java.util.Map;

public class RunTimeClass {
    private String entityName;
    private Map<String, String> fields;
    private Class<?> cls;

    public RunTimeClass() {
    }

    public RunTimeClass(String entityName, Map<String, String> fields, Class<?> cls) {
        this.entityName = entityName;
        this.fields = fields;
        this.cls = cls;
    }

    public String getEntityName() {
        return entityName;
    }

    public void setEntityName(String entityName) {
        this.entityName = entityName;
    }

    public Map<String, String> getFields() {
        return fields;
    }

    public void setFields(Map<String, String> fields) {
        this.fields = fields;
    }

    public Class<?> getCls() {
        return cls;
    }

    public void setCls(Class<?> cls) {
        this.cls = cls;
    }
}
