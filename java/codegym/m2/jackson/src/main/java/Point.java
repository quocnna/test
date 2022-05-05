import com.fasterxml.jackson.annotation.JsonPropertyOrder;

@JsonPropertyOrder({ "x", "y", "visible" })
public class Point {
    public int x, y;
    public boolean visible;
}
