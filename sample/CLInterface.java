import java.io.OutputStream;
import java.io.PrintStream;
import java.util.Scanner;

public class CLInterface {
   private static final String commands = "new UserName InstanceName ClassName\n"
      + "   create a new product\n"
      + "   named InstanceName\n"
      + "   owned by UserName\n"
      + "   of type ClassName\n"
      + "ls\n"
      + "   list all products\n"
      + "save fileName\n"
      + "   save products to a sqlite db file\n"
      + "   named fileName\n"
      + "load fileName\n"
      + "   load products from a sqlite db file\n"
      + "   named fileName";

   public void init(Scanner in) {
      String com1 = null, userName = null, productName = null, productClass = null, fileName = null;
      boolean badCommand = false;
      ProductHandler store = new ProductHandler();
      SqliteHandler database = new SqliteHandler();
      PrintStream out = System.out;

      do {
         do {
            out.print("Enter command: ");

            try {
               com1 = in.hasNext("new|ls|save|load|endAutomation") ? in.next()
                  : null;
               badCommand = com1 == null;
               if (badCommand)
                  com1 = in.next();
            } catch (Exception e) {
               System.exit(0);// Ctrl+D in *nix will result in an exit here
            }

            if (com1.equalsIgnoreCase("new")) {
               userName = in.next();
               productName = in.next();
               productClass = in.next();
            }

            if (com1.equalsIgnoreCase("save")) {
               fileName = in.next();
            }

            if (com1.equalsIgnoreCase("load")) {
               fileName = in.next();
            }

            // error messages
            if (badCommand) {
               out.println("Bad command! Commands are:");
               out.println(commands);
            }

         } while (badCommand);

         if (com1.equalsIgnoreCase("new"))
            store.newProduct(userName, productName, productClass, in, out);
         if (com1.equalsIgnoreCase("ls"))
            out.println(store.listProducts());
         if (com1.equalsIgnoreCase("save"))
            database.save(store.listProducts(), fileName);
         if (com1.equalsIgnoreCase("load")) {
            store.init();//clear old products
            store.disableDefaults();
            out = new PrintStream(new OutputStream() {
               public void write(int b) {
               }
            });//discard output, similar to /dev/null
            in = new Scanner(database.load(fileName));
         }
         if (com1.equalsIgnoreCase("endAutomation")) {
            in = new Scanner(System.in);
            store.enableDefaults();
            out = System.out;
         }
      } while (true);
   }

   static public void main(String[] args) {
      Scanner in = new Scanner(System.in);
      CLInterface frontend = new CLInterface();
      frontend.init(in);
   }
}