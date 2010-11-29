import java.util.Comparator;

   public class ProCmp implements Comparator<Product> {
      /**
       * Sort all products alphabetically
       * 
       * @author valley
       */
      public int compare(Product p1, Product p2) {
         return p1.getName().toLowerCase()
            .compareTo(p2.getName().toLowerCase());
      }
   }