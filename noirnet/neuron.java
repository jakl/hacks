
public class neuron {
  public static double LEARN_RATE = .3;
  public double latestActivation;
  double[] weight;// Weights of this perceptron
  
  //Amount that the weights may vary when initially set, above and below zero
  public static double initialWeightVarity = .1;
  
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
  
  /**
   * Activation function is only successful if non-linear for multi-tired networks
   * @param sum Sum of the product of weights and inputs
   * @return
   */
  double activationFunction(double sum){
    latestActivation = 1 / (1 + Math.exp(-sum));
    return latestActivation;
  }
}
