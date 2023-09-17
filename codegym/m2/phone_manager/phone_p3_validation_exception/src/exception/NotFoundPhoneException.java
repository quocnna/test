package exception;

public class NotFoundPhoneException extends RuntimeException{
    public NotFoundPhoneException(String message) {
        super(message);
    }
}
