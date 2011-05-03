import java.awt.Color;
import java.awt.Graphics;

public class perceptron {
  static final double LEARN_RATE = .1;

  final int WIDTH, HEIGHT;
  double minWeight = 0, maxWeight = 0, twoThirds = 0, oneThird = 0;
  
  boolean winning;
  
  String name;

  double[][] weight;

  public perceptron(String name, int width, int height) {
    WIDTH = width;
    HEIGHT = height;
    weight = new double[WIDTH][HEIGHT];
    this.name = name;

    reset();
  }

  public void reset() {
    // Set all initial weights to zero
    for (int x = 0; x < WIDTH; x++)
      for (int y = 0; y < HEIGHT; y++)
        weight[x][y] = 0;
  }

  public void train(boolean[][] inputs, boolean answer) {
    winning = test(inputs);
    int actual = winning ? 1 : 0;
    int expected = answer ? 1 : 0;
    if(actual != expected)
      for (int x = 0; x < WIDTH; x++)
        for (int y = 0; y < HEIGHT; y++) {
          weight[x][y] += LEARN_RATE * (expected - actual) * (inputs[x][y]?1:-1);
          if (weight[x][y] > maxWeight) 
            setMax(weight[x][y]);
          if (weight[x][y] < minWeight)
            setMin(weight[x][y]);
        }
  }
  
  void setMin(double min){
    minWeight = min;
    recalcThirds();
  }
  void setMax(double max){
    maxWeight = max;
    recalcThirds();
  }
  void recalcThirds(){
    twoThirds = (maxWeight - minWeight) * 2 / 3 + minWeight;
    oneThird = (maxWeight - minWeight) / 3 + minWeight;
  }

  public boolean test(boolean[][] inputs) {
    double sum = 0;
    for (int x = 0; x < WIDTH; x++)
      for (int y = 0; y < HEIGHT; y++)
        sum += weight[x][y] * (inputs[x][y]?1:0);
    return sum > 0;
  }

  public void paint(Graphics g, int left, int top, int size) {
    for (int x = 0; x < WIDTH; x++)
      for (int y = 0; y < HEIGHT; y++) {
        float red = 0, green = 0, blue = 0;
        if (weight[x][y] > twoThirds)
          green = 1;
        else if (weight[x][y] < oneThird)
          red = 1;
        else
          blue = 1;
        g.setColor(new Color(red, green, blue));
        g.fillOval(left + size * x, top + size * y, size, size);
        g.setColor(Color.GRAY);
        g.drawString(Double.toString(weight[x][y]), left + size * x + size / 3,
            top + size * y + size/2);
      }
    g.setColor(Color.GRAY);
    g.drawString(Boolean.toString(winning), size*WIDTH/2 + left, size*HEIGHT+14 + top);
    g.drawString(name, size*WIDTH/4 + left, size*HEIGHT+14 + top);
  }
}