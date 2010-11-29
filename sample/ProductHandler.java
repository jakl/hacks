import java.io.PrintStream;
import java.lang.reflect.Constructor;
import java.lang.reflect.Method;
import java.util.Scanner;
import java.util.SortedMap;
import java.util.SortedSet;
import java.util.TreeMap;
import java.util.TreeSet;

public class ProductHandler {
   // holds UserName => List of Products
   private SortedMap<String, SortedMap<String, Object>> m_defaults;

   // Keep track of default values for m_products' attributes
   private SortedMap<String, SortedSet<Product>> m_products;

   private boolean useDefaults;
   public void enableDefaults() {useDefaults = true;}
   public void disableDefaults() {useDefaults = false;}

   public ProductHandler() {
      init();
   }

   public void init() {
      m_defaults = new TreeMap<String, SortedMap<String, Object>>();
      m_products = new TreeMap<String, SortedSet<Product>>();
      useDefaults = true;
   }

   /**
    * Setup the products and default hash tables with the passed in userName;
    * Add the product returned from makeProduct to the products hash table
    * 
    * @param userName
    * @param productName
    * @param stringProductClass
    */
   public void newProduct(String userName, String productName,
      String stringProductClass, Scanner in, PrintStream out) {
      Class productClass = null;
      // look for class
      try {
         productClass = Class.forName(stringProductClass);
      } catch (Exception e) {
         out.println("Can't find class " + stringProductClass);
         return;
      }

      // create hashTable in default hashTable for userName
      if (!m_defaults.containsKey(userName))
         m_defaults.put(userName, new TreeMap<String, Object>());

      if (!m_products.containsKey(userName))
         m_products.put(userName, new TreeSet<Product>(new ProCmp()));

      // Add good product to hash table indexed by username
      Product newPro = makeProduct(userName, productName, productClass, in, out);
      SortedSet<Product> usersProducts = ((TreeSet<Product>) m_products
         .get(userName));
      if (!usersProducts.add(newPro)) {
         // Overwrite old products of the same name
         usersProducts.remove(newPro);
         usersProducts.add(newPro);
      }

   }

   /**
    * Generate a new instance of a class that inherits from Product.class with
    * name productName; Call setBeanMembers to cycle all setAttribute methods
    * using the bean pattern;
    * 
    * @param userName
    *           Only used in the call to setBeanMembers
    * @param productName
    * @param productClass
    * @return
    */
   private Product makeProduct(String userName, String productName,
      Class productClass, Scanner in, PrintStream out) {
      Product productInstance = null;

      try {// find and invoke constructor
         for (Constructor constructor : productClass.getConstructors()) {
            if (1 == constructor.getParameterTypes().length)
               productInstance = (Product) constructor.newInstance(productName);
         }
      } catch (Exception e) {
         e.printStackTrace();
      }

      // invoke all "set" methods
      setBeanMembers(productInstance, userName, in, out);
      return productInstance;
   }

   /**
    * Assume the beanInstance given has been constructed properly; Request
    * values, using the scanner m_in, to satisfy each setAttribute method; Track
    * values of bean attributes to use as potential default values, decided by
    * the scanner/user
    * 
    * @param beanClass
    * @return
    */
   private Object setBeanMembers(Object beanInstance, String userName,
      Scanner in, PrintStream out) {
      Class beanClass = beanInstance.getClass();

      // Sort methods to ask m_in the right order for Clint
      SortedSet<Method> methods = new TreeSet<Method>(new MethCmp());
      for (Method method : beanClass.getDeclaredMethods()) {
         if (method.getName().startsWith("set")) {
            methods.add(method);
         }
      }

      // Cycle methods and either use default, prompt for value, or make new
      // bean prompting for the bean's data members' values
      for (Method method : methods) {
         // name of method without "set"
         String name = method.getName().substring(3);

         // Check for defaults Use default 1 for Price (y/n):
         if (useDefaults && m_defaults.get(userName).containsKey(name)) {
            out.print("Use default "
               + stringObject(m_defaults.get(userName).get(name)) + " for "
               + name + " (y/n): ");
            if (in.next().equalsIgnoreCase("y")) {
               try {// deep copy here?
                  method.invoke(beanInstance, m_defaults.get(userName)
                     .get(name));
               } catch (Exception e) {
                  e.printStackTrace();
               }
               continue;
            }
         }// G<3Jghj
         Class param = method.getParameterTypes()[0];
         if (!isSupportedDataType(param))
            continue;
         Object parsedVal;
         do {
            out.print("Enter value for " + name + ": ");
            parsedVal = getValue(param, userName, in, out);
         } while (parsedVal == null);
         try {
            method.invoke(beanInstance, parsedVal);
         } catch (Exception e) {
            e.printStackTrace();
         }
         m_defaults.get(userName).put(name, parsedVal);

      }
      return beanInstance;
   }

   /**
    * Using the scanner m_in, get a value and convert it to type type before
    * returning the value; Construct a bean and call setBeanMembers when type is
    * a bean, this will be recursive; Return null if the entered string cannot
    * be parsed to the correct data type
    * 
    * @param type
    * @param userName
    * @return
    */
   private Object getValue(Class type, String userName, Scanner in,
      PrintStream out) {
      String val = in.next();
      try {
         if (int.class == type)
            return Integer.parseInt(val);
         else if (double.class == type)
            return Double.parseDouble(val);
         else if (String.class == type)
            return val;
         else if (Boolean.class == type)
            // How does java know what is a boolean?
            return Boolean.parseBoolean(val);
         else if (isBean(type)) {
            out.println("[");
            Object beanParam = null;
            for (Constructor constructor : type.getConstructors()) {
               if (constructor.getParameterTypes().length == 0)
                  beanParam = constructor.newInstance();
            }
            return setBeanMembers(beanParam, userName, in, out);
         }
      } catch (Exception e) {
         return null;
      }
      return null;
   }

   /**
    * Convenience function for basic datatypes this program supports
    * 
    * @param type
    * @return
    */
   private boolean isSupportedDataType(Class type) {
      // should Integer, Boolean, Double be supported specifically?
      if (type.isPrimitive() || String.class == type || isBean(type))
         return true;
      return false;
   }

   /**
    * @param objClass
    * @return True if objClass is a bean, else false
    */
   private boolean isBean(Class objClass) {
      for (Constructor c : objClass.getConstructors())
         if (c.getParameterTypes().length == 0)
            return true;
      return false;
   }

   /**
    * Output the entire hash table of products to System.out
    */
   public String listProducts() {
      String ret = "";
      for (Object userName : m_products.keySet()) {
         SortedSet<Product> userProducts = (TreeSet<Product>) m_products
            .get((String) userName);
         ret += userName + " has:\n";
         for (Object obj : userProducts) {
            Product product = (Product) obj;
            ret += product.getClass().getName() + "->";
            ret += product.getName() + ": ";
            ret += stringObject(product) + "\n";
         }
      }
      return ret;
   }

   /**
    * To-String an object, recursively as need be for beans
    * 
    * @param obj
    * @return
    */
   private String stringObject(Object obj) {

      Class objClass = obj.getClass();

      if (Integer.class == objClass)
         return obj.toString();
      else if (Boolean.class == objClass)
         return obj.toString();
      else if (Double.class == objClass)
         return obj.toString();
      else if (String.class == objClass)
         return (String) obj;

      String ret = "[";
      SortedSet<Method> methods = new TreeSet<Method>(new MethCmp());
      for (Method method : obj.getClass().getDeclaredMethods()) {
         String name = method.getName();
         if (name.startsWith("get") && !name.endsWith("Name")) {
            methods.add(method);
         }
      }

      boolean first = true;
      for (Method method : methods) {
         String name = method.getName().substring(3);
         if (!first)// add , before every attribute besides the first one
            ret += ", ";
         Class type = method.getReturnType();
         try {
            if (int.class == type) {
               first = false;
               ret += name + "=>" + method.invoke(obj);
            } else if (double.class == type) {
               first = false;
               ret += name + "=>" + method.invoke(obj);
            } else if (String.class == type) {
               first = false;
               ret += name + "=>" + method.invoke(obj);
            } else if (isBean(type)) {
               first = false;
               ret += name + "=>" + stringObject(method.invoke(obj));
            }
         } catch (Exception e) {
            e.printStackTrace();
         }
      }
      return ret + "]";
   }
}