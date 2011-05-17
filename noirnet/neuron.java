
public class neuron {
  public static double LEARN_RATE = .3;
  public double latestActivation;
  double[] weight;// Weights of this perceptron
  
  //Amount that the weights may vary when initially set, above and below zero
  public static double initialWeightVarity = .4;
  
  /**
   * Set the weights to initial random values if they aren't already
   * 
   * @param length
   */
  void setWeights(int length) {
    weight = new double[length];
    for (int i = 0; i < weight.length; i++)
      weight[i] = Math.random() * 2 * initialWeightVarity
          - initialWeightVarity;
  }
  
  double activationFunction(double sum){
    latestActivation = 1 / (1 + Math.exp(-sum));
    return latestActivation;
  }
  
  /* Not necessary at this stage of the project
   * Can incorporate these features once everything else is smoothly working
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
    */
}
