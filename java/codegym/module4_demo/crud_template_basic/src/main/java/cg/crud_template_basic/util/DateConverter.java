package cg.crud_template_basic.util;

import org.springframework.core.convert.converter.Converter;
import org.springframework.stereotype.Component;

import java.time.LocalDate;

@Component
public class DateConverter implements Converter<String, LocalDate> {
    @Override
    public LocalDate convert(String source) {
        System.out.println(source);
        try {
            return LocalDate.parse(source);
        }
        catch (Exception e) {
            return LocalDate.parse("1000-01-01");
        }
    }
}
