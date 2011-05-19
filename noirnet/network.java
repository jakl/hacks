public class network {
  outputNeuron[] output = new outputNeuron[10];
  hiddenNeuron[] hidden = new hiddenNeuron[40];// Initially 40 hidden neurons
  dataFiles data = new dataFiles();

  public network() {
    reset();
  }

  public void reset() {
    // Instance all neurons
    for (int i = 0; i < hidden.length; i++)
      hidden[i] = new hiddenNeuron();
    for (int i = 0; i < output.length; i++)
      output[i] = new outputNeuron(hidden);
  }

  public double[] test() {
    double[] successRate = new double[10];
    for (int i = 0; i < successRate.length; i++)
      successRate[i] = 0;

    for (int i = 0; i < 10; i++) {

      int counter = 0;
      for (double[] testData : data.testNumerals[i]) {
        // Push test data through hidden neurons
        for (hiddenNeuron h : hidden)
          h.activation(testData);

        double max = Integer.MIN_VALUE;
        int maxNeuron = -1;
        
        //Cycle and test output neurons
        for (int j = 0; j < output.length; j++) {
          // Push hidden neuron activation values through output neurons
          double act = output[j].activation();

          // Calculate which neuron (aka which number the neural network leans
          // towards) fires highest on this input
          if (act > max) {
            max = act;
            maxNeuron = j;
          }
          // Show what each neuron thinks about each number. If the 0 neuron
          // thinks highly of the number 0 we are on the right track
          System.out.println("The " + j + " neuron thinks "
              + String.format("%1$.3f", act) + " about " + i);
        }
        //Track how well the neurons are identifying numerals
        successRate[i] = (successRate[i] * counter + (maxNeuron == i ? 1 : 0))
            / ++counter;
      }
    }
    return successRate;
  }

  public void train() {
    for (int i = 0; i < 10; i++) {

      for (double[] testData : data.trainNumerals[i]) {
        // Push train data through hidden neurons
        for (hiddenNeuron h : hidden)
          h.activation(testData);

        for (int j = 0; j < output.length; j++) {
          // Push hidden neuron activation values through output neurons
          double act = output[j].activation();

          if (j == i && act < 1)
            output[j].train(1);
          else
            output[j].train(0);
        }

        for (hiddenNeuron h : hidden)
          h.train();
      }
    }
  }

  public void train(int times) {
    for (int i = 0; i < times; i++)
      train();
  }

  public void setLearningRate(double rate) {
    neuron.LEARN_RATE = rate;
  }

  public void setRandomess(double randomness) {
    neuron.initialWeightVarity = randomness;
  }

  public void setHiddenQuantity(int quantity) {
    hidden = new hiddenNeuron[quantity];
    reset();
  }

  public String status() {
    return "Learning Rate: " + neuron.LEARN_RATE + "\nRandomness: "
        + neuron.initialWeightVarity + "\nHidden Nodes: " + hidden.length;
  }
}
