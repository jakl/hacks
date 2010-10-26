//Take in a line of text of the form
//newProduct/showProducts userName productName productClass
//all strings
//Starts with either showProducts or newProduct

//search local classes for the productClass by name
//use the userName as the owner
//use the productName as the local variable name of the instance
//Ask questions for each get/set function within the productClass

import java.awt.Color;
import java.awt.List;
import java.lang.reflect.Constructor;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.util.Comparator;
import java.util.HashMap;
import java.util.Map;
import java.util.Scanner;
import java.util.LinkedList;
import java.util.SortedMap;
import java.util.TreeMap;

import javax.print.DocFlavor.STRING;

public class Gateu {
    static private void newProduct(Scanner in, Map products, String userName,
            String productName, String productClass) {

        if (products.containsKey(userName))
            ((LinkedList) products.get(userName)).add(makeProduct(in,
                    productName, productClass));
        else {
            products.put(userName, new LinkedList());
            ((LinkedList)products.get(userName)).add(makeProduct(in,
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
            for (Constructor constructor :productClass.getConstructors()) {
                if(constructor.getParameterTypes().length > 0)
                    productInstance = (Product)constructor.newInstance(productName);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
        // For each setVar -> read in data
        for (Method m : productClass.getDeclaredMethods()) {
            if (m.getName().startsWith("set")) {
                System.out.print("Enter value for "
                        + m.getName().substring(3) + ": ");
                String value = in.next();
                Class param = m.getParameterTypes()[0];
                try {
                    if (int.class == param) {
                        m.invoke(productInstance, Integer.parseInt(value));
                    } else if (double.class == param) {
                        m.invoke(productInstance, Double.parseDouble(value));
                    } else if (Color.class == param) {
                        m.invoke(productInstance, C.get(value));
                    } else if (String.class == param) {
                        m.invoke(productInstance, value);
                    } else if (Pants.class == param) {
                        m.invoke(productInstance, makeProduct(in, "", "Pants"));
                    } else if (Shirt.class == param) {
                        m.invoke(productInstance, makeProduct(in, "", "Shirt"));
                    }
                } catch (Exception e) {
                    e.printStackTrace();
                }

            }
        }
        return productInstance;
    }

    static private void showProducts(Map products) {
        /*
         * Clint has: pants1: [Cost=>12.34, Length=>42, Size=>30] shirt1:
         * [Price=>24, Size=>42] shirt2: [Price=>10, Size=>20] Jane has: car1:
         * [Maker=>Toyota, NumSeats=>4, Price=>12345]
         */
        for (Object userName : products.keySet()) {
            LinkedList userProducts = (LinkedList) products.get((String)userName);
            System.out.println(userName + " has:");
            for (Object obj : userProducts) {
                Product product = (Product) obj;
                System.out.print(product.getName()+": ");
                Class productClass = product.getClass();
                System.out.println(showProduct(product, productClass));
            }
        }
    }
    
    static private String showProduct(Product product, Class productClass){
        String ret = "[";
        boolean first = true;
        for (Method m : productClass.getDeclaredMethods()) {
            if (m.getName().startsWith("get")) {
                if(!first)
                    ret += ", ";
                first = false;
                Class param = m.getReturnType();
                try {
                    if (int.class == param) {
                        ret += m.getName().substring(3) + "=>" + m.invoke(product);
                    } else if (double.class == param) {
                        ret += m.getName().substring(3) + "=>" +   m.invoke(product);
                    } else if (Color.class == param) {
                        ret += m.getName().substring(3) + "=>" +  ((Color)m.invoke(product)).toString();
                    } else if (String.class == param) {
                        ret += m.getName().substring(3) + "=>" + m.invoke(product);
                    } else if (Pants.class == param) {
                        ret += m.getName().substring(3) + "=>" +showProduct((Product)m.invoke(product), Pants.class);
                    } else if (Shirt.class == param) {
                        ret += m.getName().substring(3) + "=>" + showProduct((Product)m.invoke(product), Shirt.class);
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
        SortedMap products = new TreeMap();
        //products holds    UserName => List of Products
        String firstCom = null, userName = null, productName = null, productClass = null;
        boolean badCommand;
        Scanner in = new Scanner(System.in);
        do {
            do {
                System.out.print("Enter command: ");

                // number
                firstCom = in.hasNext("newProduct|showProducts") ? in.next()
                        : null;
                badCommand = firstCom == null;
                if (badCommand)
                    firstCom = in.next();

                if (firstCom.equalsIgnoreCase("newProduct")) {
                    userName = in.next();
                    productName = in.next();
                    productClass = in.next();
                }

                // error messages
                if (badCommand)
                    System.out.println("Bad command " + firstCom);
            } while (badCommand);
            in.nextLine();// gobble left over trailing characters or new lines

            if (firstCom.equalsIgnoreCase("newProduct"))
                newProduct(in, products, userName, productName, productClass);
            if (firstCom.equalsIgnoreCase("showProducts"))
                showProducts(products);
        } while (true);
    }
    enum C
    {
        red (Color.red), blue (Color.blue), green (Color.green),
        yellow (Color.yellow), black (Color.black), white (Color.white),
        grey (Color.gray), gray (Color.gray);
        
        private final Color color;
        C(Color color){this.color=color;}
        public static Color get(String name) {
            for(C a: C.values()) {
                if(a.toString().equals(name))
                    return a.color;
            }
            return null;
        }
    }
}