public class outputNeuron extends neuron {
  hiddenNeuron[] input;// Input neurons to this output neuron

  // Input / hidden neurons are references which update dynamically during
  // testing and training

  public outputNeuron(hiddenNeuron[] input) {
    this.input = input;
    setWeights(input.length);
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

  /**
   * Train self to activate closer to the desired activation value for the
   * current set of inputs
   * 
   * @param desiredActivation
   */
  public void train(double desiredActivation) {
    double delta = LEARN_RATE * (desiredActivation - latestActivation)
        * latestActivation * (1 - latestActivation);
    for (int i = 0; i < weight.length; i++) {
      weight[i] += delta * input[i].latestActivation;// Adjust weights within
      // output neuron
      input[i].errorDelta += delta * weight[i];// Back propagate to hidden
      // neurons, who will train
      // themselves
    }
  }
}