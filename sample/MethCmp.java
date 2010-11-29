import java.lang.reflect.Method;
import java.util.Comparator;


   public class MethCmp implements Comparator<Method> {
      public int compare(Method m1, Method m2) {
         /**
          * Sort all lists of methods (gotten using reflections) alphabetically
          * 
          * @author valley
          */
         return m1.getName().toLowerCase()
            .compareTo(m2.getName().toLowerCase());
      }
   }