package cg.crud_template_basic.validation;

import javax.validation.Constraint;
import javax.validation.Payload;
import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

@Target({ ElementType.FIELD })
@Retention(RetentionPolicy.RUNTIME)
@Constraint(validatedBy = PriceValidImpl.class)
public @interface PriceValid {
    String message() default "Price should be greater than 1000";
    Class<?>[] groups() default {};
    Class<? extends Payload>[] payload() default {};
}
