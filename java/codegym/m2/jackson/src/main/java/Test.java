import com.fasterxml.jackson.databind.MappingIterator;
import com.fasterxml.jackson.databind.SequenceWriter;
import com.fasterxml.jackson.dataformat.csv.CsvMapper;
import com.fasterxml.jackson.dataformat.csv.CsvParser;
import com.fasterxml.jackson.dataformat.csv.CsvSchema;

import java.io.*;
import java.util.List;

public class Test {
    public static void main(String[] args) throws IOException {
        final String CSV_DOC = "1,2,true\n2,9,false\n-13,0,true\n";
//
        final CsvMapper mapper = new CsvMapper();
//        MappingIterator<List<String>> it = mapper
//                .readerForListOf(String.class)
//                .with(CsvParser.Feature.WRAP_AS_ARRAY) // !!! IMPORTANT
//                .readValues(CSV_DOC);
//
//        List<List<String>> all = it.readAll();
//


//        CsvSchema schema = CsvSchema.builder()
//                .addColumn("x")
//                .addColumn("y")
//                .addColumn("visible")
//                .build();

        CsvSchema schema = mapper.schemaFor(Point.class);

        MappingIterator<Point> it = mapper
                .readerFor(Point.class)
                .with(schema)
                .readValues(CSV_DOC);
//        while (it.hasNextValue()) {
//            Point p = it.nextValue();
//            int x = p.x;
//            // do something!
//        }
// or, you could alternative slurp 'em all:
        List<Point> points = it.readAll();
        points.stream().forEach(System.out::println);





        CsvSchema altSchema = mapper.schemaFor(Person.class);
        try (StringWriter strW = new StringWriter()) {
            SequenceWriter seqW = mapper.writerWithSchemaFor(Person.class)
                    .writeValues(strW);
            seqW.write(new Person("Bob", 37, false));
            seqW.write(new Person("Jeff", 28, true));

            File a= new File("src/main/java/test.csv");
            boolean b = a.exists();
            FileWriter fileWriter = new FileWriter("src/main/java/test.csv", true);

            BufferedWriter buffWriter = new BufferedWriter(fileWriter);
            buffWriter.write(strW.toString());
            buffWriter.close();
        }


    }
}
