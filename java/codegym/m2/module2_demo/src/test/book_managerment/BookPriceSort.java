package test.book_managerment;

import java.util.List;

public class BookPriceSort {
    static void selectionSort(List<Book> list){
        int size= list.size();
        for (int i = 0; i < size; i++) {
            int min_pos= i;

            for (int j = i; j < size; j++) {
                if(list.get(min_pos).getPrice() > list.get(j).getPrice()){
                    min_pos= j;
                }
            }

            if(min_pos!= i){
                Book tmp= list.get(min_pos);
                list.set(min_pos, list.get(i));
                list.set(i, tmp);
            }
        }
    }

    static void bubbleSort(List<Book> list){
        int size= list.size();

        for (int i = 0; i < size; i++) {
            boolean isSorted= true;
            for (int j = 0; j < size - i- 1 ; j++) {
                if(list.get(j).getPrice() > list.get(j+1).getPrice()){
                    Book tmp= list.get(j);
                    list.set(j, list.get(j+1));
                    list.set(j+1, tmp);
                    isSorted= false;
                }

                if(isSorted) break;
            }
        }
    }

    static void insertionSort(List<Book> list){
        int size= list.size();
        for (int i = 1; i < size; i++) {
            Book tmp= list.get(i);
            int pos= i- 1;

            while (pos>= 0 && list.get(pos).getPrice() > tmp.getPrice() ){
                list.set(pos+1, list.get(pos));
                pos--;
            }

            list.set(pos+1, tmp);
        }
    }
}
