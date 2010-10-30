import java.lang.reflect.Constructor;
import java.lang.reflect.Method;
import java.util.Comparator;
import java.util.Map;
import java.util.Scanner;
import java.util.SortedMap;
import java.util.SortedSet;
import java.util.TreeMap;
import java.util.TreeSet;

public class Gateu {
    //Keep track of default values for products
    SortedMap<String, SortedSet<Product>> productDefaults;
    
    private static class ProCmp implements Comparator<Product> {
        public int compare(Product p1, Product p2) {
            return p1.getName().toLowerCase()
                    .compareTo(p2.getName().toLowerCase());
        }
    }

    private static class MethCmp implements Comparator<Method> {
        public int compare(Method m1, Method m2) {
            return m1.getName().toLowerCase()
                    .compareTo(m2.getName().toLowerCase());
        }
    }

    static private void newProduct(Scanner in,
            SortedMap<String, SortedSet<Product>> products,
            String userName, String productName, String productClass) {

        try {
            Class.forName(productClass);
        } catch (Exception e) {
            System.out.println("Can't find class " + productClass);
            return;
        }

        // Add good product to hash of products with keys being user names
        if (products.containsKey(userName))
            ((TreeSet<Product>) products.get(userName)).add(makeProduct(in,
                    productName, productClass));
        else {
            products.put(userName, new TreeSet<Product>(new ProCmp()));
            ((TreeSet<Product>) products.get(userName)).add(makeProduct(in,
                    productName, productClass));
        }
    }

    static private Product makeProduct(Scanner in, String productName,
            String productClassString) {
        Class productClass = null;
        Product productInstance = null;

        try {
            productClass = Class.forName(productClassString);
        } catch (Exception e) {
            e.printStackTrace();
        }
        try {
            for (Constructor constructor : productClass.getConstructors()) {
                if (constructor.getParameterTypes().length > 0)
                    productInstance = (Product) constructor
                            .newInstance(productName);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }

        SortedSet<Method> methods = new TreeSet<Method>(new MethCmp());
        for (Method method : productClass.getDeclaredMethods()) {
            if (method.getName().startsWith("set")) {
                Class param = method.getParameterTypes()[0];
                if (param.isPrimitive() || String.class == param
                        || Shirt.class == param || Pants.class == param)
                    methods.add(method);
            }
        }

        setInstanceMethods(in, productInstance, methods);
        return productInstance;
    }

    private static void setInstanceMethods(Scanner in, Product productInstance,
            SortedSet<Method> methods) {
        // For each setVar -> read in data
        for (Method method : methods) {
            System.out.print("Enter value for " + method.getName().substring(3)
                    + ": ");
            Class param = method.getParameterTypes()[0];
            String value = null;
            if (param.isPrimitive() || String.class == param)
                value = in.next();
            else
                System.out.println('[');
            try {
                if (int.class == param) {
                    method.invoke(productInstance, Integer.parseInt(value));
                } else if (double.class == param) {
                    method.invoke(productInstance, Double.parseDouble(value));
                } else if (String.class == param) {
                    method.invoke(productInstance, value);
                } else if (Pants.class == param) {
                    method.invoke(productInstance, makeProduct(in, "", "Pants"));
                } else if (Shirt.class == param) {
                    method.invoke(productInstance, makeProduct(in, "", "Shirt"));
                }
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }

    static private void showProducts(Map products) {
        for (Object userName : products.keySet()) {
            SortedSet<Product> userProducts = (TreeSet<Product>) products
                    .get((String) userName);
            System.out.println(userName + " has:");
            for (Object obj : userProducts) {
                Product product = (Product) obj;
                System.out.print(product.getName() + ": ");
                Class productClass = product.getClass();
                System.out.println(showProduct(product, productClass));
            }
        }
    }

    static private String showProduct(Product product, Class productClass) {
        String ret = "[";
        boolean first = true;

        SortedSet<Method> methods = new TreeSet<Method>(new MethCmp());
        for (Method method : productClass.getDeclaredMethods()) {
            if (method.getName().startsWith("get")) {
                Class param = method.getReturnType();
                if (param.isPrimitive() || String.class == param
                        || Shirt.class == param || Pants.class == param)
                    methods.add(method);
            }
        }

        for (Method method : methods) {
            if (method.getName().startsWith("get")) {
                if (!first)
                    ret += ", ";
                first = false;
                Class param = method.getReturnType();
                try {
                    if (int.class == param) {
                        ret += method.getName().substring(3) + "=>"
                                + method.invoke(product);
                    } else if (double.class == param) {
                        ret += method.getName().substring(3) + "=>"
                                + method.invoke(product);
                    } else if (String.class == param) {
                        ret += method.getName().substring(3) + "=>"
                                + method.invoke(product);
                    } else if (Pants.class == param) {
                        ret += method.getName().substring(3)
                                + "=>"
                                + showProduct((Product) method.invoke(product),
                                        Pants.class);
                    } else if (Shirt.class == param) {
                        ret += method.getName().substring(3)
                                + "=>"
                                + showProduct((Product) method.invoke(product),
                                        Shirt.class);
                    }
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        }
        ret += "]";
        return ret;
    }

    static public void main(String[] args) {
        SortedMap<String, SortedSet<Product>> products = new TreeMap<String, SortedSet<Product>>();
        // products holds UserName => List of Products
        String com1 = null, userName = null, productName = null, productClass = null;
        boolean badCommand = false;
        Scanner in = new Scanner(System.in);
        do {
            do {
                System.out.print("Enter command: ");

                // number
                try {
                    com1 = in.hasNext("newProduct|showProducts") ? in.next()
                            : null;
                    badCommand = com1 == null;
                    if (badCommand)
                        com1 = in.next();
                } catch (Exception e) {
                    System.exit(0);
                }

                if (com1.equalsIgnoreCase("newProduct")) {
                    userName = in.next();
                    productName = in.next();
                    productClass = last(in.next().split("\\."));
                }

                // error messages
                if (badCommand)
                    System.out.println("Bad command " + com1);
            } while (badCommand);
            in.nextLine();// gobble left over trailing characters or new lines

            if (com1.equalsIgnoreCase("newProduct"))
                newProduct(in, products, userName,
                        productName, productClass);
            if (com1.equalsIgnoreCase("showProducts"))
                showProducts(products);
        } while (true);
    }

    public static <T> T last(T[] array) {
        return array[array.length - 1];
    }
}