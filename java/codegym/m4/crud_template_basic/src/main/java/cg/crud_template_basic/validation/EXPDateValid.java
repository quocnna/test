package cg.crud_template_basic.validation;

import javax.validation.Constraint;
import javax.validation.Payload;
import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

@Target({ ElementType.FIELD })
@Retention(RetentionPolicy.RUNTIME)
@Constraint(validatedBy = EXPDateValidImpl.class)
public @interface EXPDateValid {
    String message() default "Please select a date that is at least 3 months from now";
    Class<?>[] groups() default {};
    Class<? extends Payload>[] payload() default {};
}
