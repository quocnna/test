package cg.crud_template_basic.validation;

import javax.validation.ConstraintValidator;
import javax.validation.ConstraintValidatorContext;

public class PriceValidImpl implements ConstraintValidator<PriceValid,Double> {
    @Override
    public boolean isValid(Double value, ConstraintValidatorContext context) {
        return value > 500;
    }
}
