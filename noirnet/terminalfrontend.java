import java.util.Scanner;

public class terminalfrontend {
  public static void main(String[] args) {
    Scanner in = new Scanner(System.in);
    network noirNetwork = new network();
    System.out.println("Welcome to the Noir Network  ::  Command List Below");
    while (true) {
      System.out.println("  (t)rain, t(e)st, (r)eset\nOr change:\n"
          + "  (l)earning rate, (i)nitial randomness, (h)idden nodes"
          + "\nOr (p)rint attributes  or  (q)uit");
      String userCommand = in.next();
      switch (userCommand.toLowerCase().charAt(0)) {
      case 't':
        System.out.println("How many times to train?");
        if (isInt(in))
          noirNetwork.train(in.nextInt());
        break;
      case 'h':
        System.out.println("How many hidden nodes should there be?");
        if (isInt(in))
          noirNetwork.setHiddenQuantity(in.nextInt());
        break;
      case 'e':
        printTestResults(noirNetwork.test());
        break;
      case 'r':
        noirNetwork.reset();
        break;
      case 'l':
        System.out.println("What should the learning rate be?");
        if (isDouble(in))
          noirNetwork.setLearningRate(in.nextDouble());
        break;
      case 'i':
        System.out.println("What should the initial randomness be?");
        if (isDouble(in)) {
          noirNetwork.setRandomess(in.nextDouble());
          System.out.println("You may want to call (r)eset now");
        }
        break;
      case 'p':
        System.out.println("\n" + noirNetwork.status() + "\n");
        break;
      case 'q':
        System.exit(0);
        break;

      default:
        System.out.println("I think you ment to type either t,e,r,l,i,h, or p");
      }
    }
  }

  static void printTestResults(double[] results) {
    double averageSuccess = 0;
    for (int i = 0; i < results.length; i++){
      System.out.println("For the number " + i + ", the network had "
          + String.format("%1$.1f", results[i]*100) + "% success.");
      averageSuccess = (averageSuccess * i + results[i]*100)/(i+1);
    }
    System.out.println("Average Success: " + String.format("%1$.1f%%", averageSuccess));
  }

  static boolean isInt(Scanner in) {
    if (in.hasNextInt())
      return true;
    in.next();
    System.out
        .println("Failed because you couldn't give a simple integer like 4");
    return false;
  }

  static boolean isDouble(Scanner in) {
    if (in.hasNextDouble())
      return true;
    in.next();
    System.out
        .println("Failed because you couldn't give a simple decimal like .42");
    return false;
  }
}