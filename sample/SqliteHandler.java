import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.Statement;
import java.util.ArrayList;

public class SqliteHandler {

   public void save(String products, String fileName) {

      String[] listProducts = removeEmptyElems(products
         .split(" |\n|:|\\[|\\]|>|,"));

      PreparedStatement prep = null;
      Connection conn = null;

      try {
         Class.forName("org.sqlite.JDBC");
         conn = DriverManager.getConnection("jdbc:sqlite:" + fileName);
         Statement stat = conn.createStatement();
         stat.executeUpdate("drop table if exists products;");
         stat
            .executeUpdate("create table if not exists products (username, productname, productclass, vals);");
         prep = conn
            .prepareStatement("insert into products values (?, ?, ?, ?);");
      } catch (Exception e) {
         e.printStackTrace();
      }

      String userName = null, productName = null, productClass = null, values = "";
      for (int i = 0; i < listProducts.length;) {
         String name = listProducts[i++];
         if (i == listProducts.length) {// found product with no values
            productName = name;
            queueEntry(prep, userName, productName, productClass, values);
            productName = null;
         } else if (listProducts[i].equalsIgnoreCase("has")) {
            // found new userName

            queueEntry(prep, userName, productName, productClass, values);
            productName = null;
            values = "";
            userName = name;
            i++;
         } else if (name.endsWith("="))// found another value
            values += listProducts[i++] + " ";
         else if (name.endsWith("-")) {// found a product's class
            queueEntry(prep, userName, productName, productClass, values);
            values = "";
            // remove the minus sign suffix
            productClass = name.substring(0, name.length() - 1);
         } else
            // found another product
            productName = name;
      }
      queueEntry(prep, userName, productName, productClass, values);

      try {
         conn.setAutoCommit(false);
         prep.executeBatch();
         conn.setAutoCommit(true);

         conn.close();
      } catch (Exception e) {
         e.printStackTrace();
      }
   }

   private String[] removeEmptyElems(String[] hasEmptyElems) {
      ArrayList<String> noEmptyElems = new ArrayList<String>();
      for (String elem : hasEmptyElems) {
         if (elem.length() > 0) {
            noEmptyElems.add(elem);
         }
      }
      return noEmptyElems.toArray(new String[0]);
   }

   private void queueEntry(PreparedStatement prep, String userName,
      String productName, String productClass, String values) {
      if (productName != null)
         try {
            prep.setString(1, userName);
            prep.setString(2, productName);
            prep.setString(3, productClass);
            prep.setString(4, values);
            prep.addBatch();
         } catch (Exception e) {
            e.printStackTrace();
         }
   }

   public String load(String fileName) {
      String ret = "";

      try {
         Class.forName("org.sqlite.JDBC");
         Connection conn = DriverManager.getConnection("jdbc:sqlite:"
            + fileName);
         Statement stat = conn.createStatement();

         ResultSet rs = stat.executeQuery("select * from products;");
         while (rs.next()) {
            ret += " new ";
            ret += rs.getString("username");
            ret += " ";
            ret += rs.getString("productname");
            ret += " ";
            ret += rs.getString("productclass");
            ret += " ";
            ret += rs.getString("vals");
         }
         rs.close();
         conn.close();
      } catch (Exception e) {
         e.printStackTrace();
      }

      ret += " endAutomation";
      return ret;
   }
}