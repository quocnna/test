package test.other;

import java.util.Arrays;

public class AlgorithmSort {
    static int[] arr = {23, 9, 84, 8, 62};

    public static void main(String[] args) {
//        selectionSort();
        bubbleSort();
//        insetionSort();
    }

    private static void selectionSort() {
        int size = arr.length;
        for (int i = 0; i < size; i++) {
            int min_pos = i;

            for (int j = i; j < size; j++) {
                if (arr[min_pos] > arr[j]) {
                    min_pos = j;
                }
            }

            if (min_pos != i) {
                int tmp = arr[min_pos];
                arr[min_pos] = arr[i];
                arr[i] = tmp;
            }

            System.out.printf("Loop %d: %s %n", i + 1, Arrays.toString(arr));
        }
    }

    private static void bubbleSort() {
        int size = arr.length;
        for (int i = 0; i < size; i++) {
//            boolean isSorted= true;

            for (int j = 0; j < size - i - 1; j++) {
                if (arr[j] > arr[j + 1]) {
                    int tmp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = tmp;
//                    isSorted= false;
                }
            }

//            if(isSorted) break;

            System.out.printf("Loop %d: %s %n", i + 1, Arrays.toString(arr));
        }
    }

    private static void insetionSort() {
        int size = arr.length;
        for (int i = 1; i < size; i++) {
            int tmp = arr[i];
            int pos = i - 1;

            while (pos >= 0 && arr[pos] > tmp) {
                arr[pos + 1] = arr[pos];
                pos--;
            }

            arr[pos + 1] = tmp;

            System.out.printf("Loop %d: %s %n", i + 1, Arrays.toString(arr));
        }
    }
}
