package cg.crud_template_basic.validation;

import org.springframework.beans.factory.annotation.Configurable;

import javax.validation.ConstraintValidator;
import javax.validation.ConstraintValidatorContext;
import java.time.LocalDate;

@Configurable
public class EXPDateValidImpl implements ConstraintValidator<EXPDateValid, LocalDate> {
    @Override
    public boolean isValid(LocalDate value, ConstraintValidatorContext context) {
        LocalDate after3months = LocalDate.now().plusMonths(3);
        return after3months.compareTo(value) <= 0;
    }
}
