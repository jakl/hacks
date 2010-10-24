//Take in a line of text of the form

//newProduct/showProducts userName productName productClass

//all strings

//Starts with either showProducts or newProduct



//search local classes for the productClass by name

//use the userName as the owner

//use the productName as the local variable name of the instance

//Ask questions for each get/set function within the productClass



import java.awt.Color;

import java.lang.reflect.InvocationTargetException;

import java.lang.reflect.Method;

import java.util.HashMap;

import java.util.Map;

import java.util.Scanner;



import javax.print.DocFlavor.STRING;



public class Gateu {

    static public void newProduct(Scanner in, Map products, String userName,

            String productName, String productClass) {



        Product product = makeProduct(in, productName, productClass);



        if (products.containsKey(userName))

            ;// ((Map)products.get(userName)).put(new shoes(productName));//make

             // the new more dynamic; use reflections

        else

            ;// products.put(userName, new LinkedList().put(new

             // shoes(productName)));



    }



    static public Product makeProduct(Scanner in, String productName,

            String productClass) {

        Class product = null;

        try {

            product = Class.forName(productClass);

        } catch (Exception e) {

            e.printStackTrace();

        }

        // For each setVar -> read in data

        for (Method m : product.getDeclaredMethods()) {

            if (m.getName().startsWith("set")) {

                System.out.println("Enter value for "

                        + m.getName().substring(3) + ": ");

                String value = in.next();

                Class param = m.getParameterTypes()[0];

                if (int.class == param) {

                    Integer.parseInt(value);

                } else if (double.class == param) {

                    Double.parseDouble(value);

                } else if (Color.class == param) {

                    Color.getColor(value);

                } else if (String.class == param) {

                    // value;

                } else if (Pants.class == param) {

                    makeProduct( in, "",            "Pants");

                }else if (Shirt.class==param) {

                    makeProduct(in,"","Shirt");

                }

                

            }

        }

        return null;

    }



    static public void showProducts(Map products) {

        /*

         * Clint has: pants1: [Cost=>12.34, Length=>42, Size=>30] shirt1:

         * [Price=>24, Size=>42] shirt2: [Price=>10, Size=>20] Jane has: car1:

         * [Maker=>Toyota, NumSeats=>4, Price=>12345]

         */

        for (Object userName : products.keySet()) {

            Map userProducts = (Map) products.get(userName);

            System.out.println(userName + " has:");

            for (Object product : userProducts.keySet()) {

                System.out.println("A product!");

                // reflections here to cast product to a instance of Pants or

                // Shoes....

            }

        }

    }



    static public void main(String[] args) {

        Map products = new HashMap();

        String firstCommand = null, userName = null, productName = null, productClass = null;

        boolean badCommand;

        Scanner in = new Scanner(System.in);



        do {

            System.out.print("Enter command: ");



            // number

            firstCommand = in.hasNext("newProduct|showProducts") ? in.next()

                    : null;

            badCommand = firstCommand == null;

            if (badCommand)

                firstCommand = in.next();



            if (firstCommand.equalsIgnoreCase("newProduct")) {

                userName = in.next();

                productName = in.next();

                productClass = in.next();

            }



            // error messages

            if (badCommand)

                System.out.println("Bad command " + firstCommand);

        } while (badCommand);

        in.nextLine();// gobble left over trailing characters or new lines



        if (firstCommand.equalsIgnoreCase("newProduct"))

            newProduct(in, products, userName, productName, productClass);

        if (firstCommand.equalsIgnoreCase("showProducts"))

            showProducts(products);

    }

}
