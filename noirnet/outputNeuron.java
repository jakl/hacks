public class outputNeuron extends neuron {
  hiddenNeuron[] input;//Input neurons to this output neuron
  
  public outputNeuron(hiddenNeuron[] input){
    this.input = input;
    setWeights(input.length);
  }

  /**
   * Train this perceptron against a set of inputs It will adjust its weights if
   * it fails to return the correct answer
   * 
   * @param inputs
   * @param answer
   */
  public void train(double[][] inputs, boolean answer) {
  }

  /**
   * Sum the weights * inputs, pass through activation function, and return
   * value
   * 
   * @param inputs
   * @return
   */
  public double activation() {
    double sum = 0;
    for (int i = 0; i < input.length; i++)
      sum += weight[i] * input[i].latestActivation;
    
    return activationFunction(sum);
  }
  
  public void train(double desiredActivation){
    double change = LEARN_RATE * (desiredActivation - latestActivation) * latestActivation * (1-latestActivation);
    for(int i = 0; i < weight.length; i++){
      weight[i] += change * input[i].latestActivation;
      input[i].errorDelta += change*weight[i];
    }
  }
}