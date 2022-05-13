package test;

// switch = if, else if;  ternary = all (nested if)
public class TernaryOperator {
    public static void main(String[] args) {
        int diem = 12;

        String kq = diem >= 8 ? "Giỏi"
                : diem >= 6.5 ? "Khá"
                : diem >= 5 ? "Trung bình"
                : "Kém";

//        String kq;
//        if (diem >= 8)
//            kq = "Giỏi";
//        else if (diem >= 6.5)
//            kq = "Khá";
//        else if (diem >= 5)
//            kq = "Trung bình";
//        else
//            kq = "Kém";

        System.out.println(kq);

        int year = 2022;
        String ms = year % 4 == 0 ?
                year % 100 == 0 ?
                        year % 400 == 0 ? "nhuan" : "ko phai nhuan"
                        : "nhuan"
                : "ko phai nhuan";
    }
}
