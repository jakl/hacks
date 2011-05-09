import java.awt.Color;
import java.awt.Graphics;

public class perceptron {
  static double LEARN_RATE = .1;

  final int WIDTH, HEIGHT;

  // Track useful statistics for coloring weights in paint function
  double minWeight = 0, maxWeight = 0, twoThirds = 0, oneThird = 0;

  // Amount of training, testing, and successes
  int training, testing, successes;

  String name;// name of this perceptron

  double[][] weight;//Weights of this perceptron

  //Amount that the weights may vary when initially set
  public static double initialWeightVarity = .02;

  /**
   * Create a new perceptron, capable of recognizing a named image of width and height
   * after training enough
   * 
   * @param name
   * @param width
   * @param height
   */
  public perceptron(String name, int width, int height) {
    WIDTH = width;
    HEIGHT = height;
    weight = new double[WIDTH][HEIGHT];
    this.name = name;

    reset();
  }

  /**
   * Reset instanced data
   */
  public void reset() {
    // Set all initial weights
    for (int x = 0; x < WIDTH; x++)
      for (int y = 0; y < HEIGHT; y++)
        weight[x][y] = Math.random() * 2 * initialWeightVarity
            - initialWeightVarity;

    training = 0;// Perceptron has gone through zero training
    testing = 0;
    successes = 0;

    recalcMinMaxThirds();
  }

  public static void increaseLearnRate() {
    LEARN_RATE *= 2;
  }

  public static void decreaseLearnRate() {
    LEARN_RATE /= 2;
  }

  public static void increaseInitialVarity() {
    initialWeightVarity *= 2;
  }

  public static void decreaseInitialVarity() {
    initialWeightVarity /= 2;
  }

  /**
   * Train this perceptron against a set of inputs 
   * It will adjust its weights if it fails to return the correct answer
   * 
   * @param inputs
   * @param answer
   */
  public void train(boolean[][] inputs, boolean answer) {
    int actual = test(inputs) ? 1 : 0;
    int expected = answer ? 1 : 0;

    if (actual != expected) {
      training++;
      successes = 0;
      testing = 0;

      for (int x = 0; x < WIDTH; x++)
        for (int y = 0; y < HEIGHT; y++)
          weight[x][y] += LEARN_RATE * (expected - actual)
              * (inputs[x][y] ? 1 : 0);
    }

    recalcMinMaxThirds();
  }

  /**
   * Keep track of minimum maximum
   * one third and two thirds of the weights
   * This helps in drawing colors
   */
  void recalcMinMaxThirds() {
    maxWeight = weight[0][0];
    minWeight = weight[0][0];

    for (double row[] : weight)
      for (double w : row) {
        if (w > maxWeight)
          maxWeight = w;
        if (w < minWeight)
          minWeight = w;
      }

    twoThirds = (maxWeight - minWeight) * 2 / 3 + minWeight;
    oneThird = (maxWeight - minWeight) / 3 + minWeight;
  }

  /**
   * Determine whether this perceptron fires on the inputs
   * 
   * @param inputs
   * @return
   */
  public boolean test(boolean[][] inputs) {
    double sum = 0;
    for (int x = 0; x < WIDTH; x++)
      for (int y = 0; y < HEIGHT; y++)
        sum += weight[x][y] * (inputs[x][y] ? 1 : 0);
    return sum > 0;
  }

  public void test(boolean[][] inputs, boolean answer) {
    if (testing >= 999)
      return;
    testing++;
    if (test(inputs) == answer)
      successes++;
  }

  /**
   * Draw this Perceptron's weights to a graphics object at x,y position
   * left,top where each weight is sized at size
   * 
   * @param g
   * @param left
   * @param top
   * @param size
   */
  public void paint(Graphics g, int left, int top, int size) {
    
    //Cycle through each weight
    for (int x = 0; x < WIDTH; x++)
      for (int y = 0; y < HEIGHT; y++) {
        
        float red = 0, green = 0, blue = 0;
        if (weight[x][y] > twoThirds)
          green = 1;
        else if (weight[x][y] < oneThird)
          red = 1;
        else
          blue = 1;
        
        //Display lowest third of weights in red
        //Middle third in blue, and highest in green.
        g.setColor(new Color(red, green, blue));
        
        g.fillOval(left + size * x, top + size * y, size, size);
        
        //Text Color for weights
        g.setColor(Color.GRAY);
        
        //Display each weight's value
        g.drawString(String.format("%1$.5f", weight[x][y]),
            left + size * x + 2, top + size * y + size / 2);
      }
    
    //Text Color
    g.setColor(Color.GRAY);

    final int TEXT_HEIGHT = 14;//Height of the default font

    //Display name of this perceptron
    g.drawString(name + " Perceptron", left + size * 2, size * HEIGHT
        + TEXT_HEIGHT + top);
    
    //Display Min Max Weights and quantity of training
    g.drawString("WEIGHTS:  Min:" + String.format("%1$.2f", minWeight)
        + "  Max:" + String.format("%1$.2f", maxWeight)
        , left + size, size * HEIGHT + TEXT_HEIGHT * 2 + top);

    //Display quantity of tests, successes, and percent success
    g.drawString("Training: " + Integer.toString(training) + "  Tests: " + testing + " Successes: " + successes
        + " Percent: "
        
        //Add 1 to testing to avoid divide by zero
        + String.format("%1$.2f", (double) successes / (double) (testing + 1)),
        left + size, size * HEIGHT + TEXT_HEIGHT * 3 + top);
  }
}