/**
 * This program, Perceptrons, is meant to show how a computer
 * can learn to identify patterns in images
 * 
 * @author James Koval
 * License: GNU GPL v3+
 */

//Allows this class (java code file) to function through a window
import java.awt.event.WindowEvent;
import java.awt.event.WindowAdapter;

//For drawing in our window
import java.awt.Container;//Window contents can be given a Graphics object to draw
import java.awt.Image;
import java.awt.Color;//Colors for graphics shapes and text
import java.awt.Graphics;//The actual canvas that knows how to draw shapes and text on itself

//Keyboard events
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;

//Basic event and listener for events
import java.awt.event.ActionListener;
import java.awt.event.ActionEvent;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.FilenameFilter;
import java.io.IOException;
import java.util.ArrayList;

//JFrame for window menu bar and content
import javax.imageio.ImageIO;
import javax.swing.JFrame;

//Timer for screen refresh rate
import javax.swing.Timer;

@SuppressWarnings("serial")
public class guifrontend extends JFrame implements ActionListener, KeyListener {

  // Variable Declaration Pool
  private Container pane;
  Image offscreen;
  Graphics screen;
  static int w, h;
  boolean bSpace, bEnter, bBackSpace;// Keyboard events

  /*
   * Program Specific variables Not part of Template
   */

  perceptron vertical, horizontal, backslash, forwardslash;
  ArrayList<boolean[][]> vertical_inputs = new ArrayList<boolean[][]>(),
      horizontal_inputs = new ArrayList<boolean[][]>(),
      backslash_inputs = new ArrayList<boolean[][]>(),
      forwardslash_inputs = new ArrayList<boolean[][]>(),
      vertical_tests = new ArrayList<boolean[][]>(),
      forwardslash_tests = new ArrayList<boolean[][]>(),
      backslash_tests = new ArrayList<boolean[][]>(),
      horizontal_tests = new ArrayList<boolean[][]>();

  /*
   * Function Description init(): Initializes all variables needed to make and
   * sustain this JFrame template program
   */
  void init() {
    // Variable Initialization Pool setting values

    // Screen Width / Height
    w = 1000;
    h = 820;

    // User Input through Keyboard
    bSpace = false;
    bEnter = false;
    bBackSpace = false;

    // Activating window and listener functionality
    pane = getContentPane();
    pane.addKeyListener(this);
    pane.requestFocus();

    // Clock
    Timer clock = new Timer(20, this); // Constructed with (delay in ms,
    // ActionListener to fire on)
    clock.start();

    // Program Specific variables Not part of Template
    vertical = new perceptron("Vertical", 5, 5);
    horizontal = new perceptron("Horizontal", 5, 5);
    backslash = new perceptron("Backslash", 5, 5);
    forwardslash = new perceptron("Forwardslash", 5, 5);

    getTrainingData();
    getTestingData();

  }// init

  /*
   * Function Description point(Graphics g): Is called from repaint() and
   * required for drawing on Graphics objects
   */
  public void paint(Graphics g) {
    if (screen == null) {
      offscreen = createImage(w, h);
      screen = offscreen.getGraphics();
    }
    paint2(screen);
    g.drawImage(offscreen, 1, 29, this);// Seemingly evil random integers
    // accommodate for menu bar and window
    // borders
  }// paint

  /*
   * Function Description paint(Graphics g): Buffers graphics so there isn't
   * flickering on refresh
   */
  void paint2(Graphics g) {
    int weightSize = 70;
    g.setColor(Color.black);
    g.fillRect(0, 0, w, h);
    vertical.paint(g, 0, 0, weightSize);
    horizontal.paint(g, weightSize * 6, 0, weightSize);
    backslash.paint(g, 0, weightSize * 6, weightSize);
    forwardslash.paint(g, weightSize * 6, weightSize * 6, weightSize);

    // Display statistics on the right side of the view
    g.drawString("Learn Rate: "
        + String.format("%1$.5f", perceptron.LEARN_RATE), weightSize * 12,
        weightSize);

    // Varity: Amount that something varies
    g.drawString("Initial Varity: "
        + String.format("%1$.5f", perceptron.initialWeightVarity),
        weightSize * 12, weightSize * 2);
    
    showControls(g, weightSize * 12, weightSize * 3);
  }// paint2
  
  void showControls(Graphics g, int left, int top){
    int fontHeight = 14;
    int columnOffset = 85;
    g.drawString("Enter", left, top + fontHeight);
    g.drawString("Train", left + columnOffset, top + fontHeight);
    g.drawString("Space", left, top + fontHeight*2);
    g.drawString("Test", left+columnOffset, top + fontHeight*2);
    g.drawString("Backspace", left, top + fontHeight*3);
    g.drawString("Reset", left+columnOffset, top + fontHeight*3);
    g.drawString("Up", left, top + fontHeight*4);
    g.drawString("Rate++", left+columnOffset, top + fontHeight*4);
    g.drawString("Down", left, top + fontHeight*5);
    g.drawString("Rate--", left+columnOffset, top + fontHeight*5);
    g.drawString("Left", left, top + fontHeight*6);
    g.drawString("Varity--", left+columnOffset, top + fontHeight*6);
    g.drawString("Right", left, top + fontHeight*7);
    g.drawString("Varity++", left+columnOffset, top + fontHeight*7);
  }

  /*
   * Function Description actionPerformed(ActionEvent): Fires on timer tick
   */
  public void actionPerformed(ActionEvent e) {
    pane.requestFocus();//If this window has focus, then set this pane to have focus as well
    KeyboardStrokes();
    repaint();
  }// actionPerformed

  /*
   * Function Description KeyboardStrokes(): Up Down Left Right if / else
   * statements with code bodies for each.
   */
  void KeyboardStrokes() {
    if (bSpace) {
      for (boolean[][] input : vertical_tests)
        testVertical(input);
      for (boolean[][] input : horizontal_tests)
        testHorizontal(input);
      for (boolean[][] input : backslash_tests)
        testBackslash(input);
      for (boolean[][] input : forwardslash_tests)
        testForwardslash(input);
    }
    if (bEnter) {
      for (boolean[][] input : vertical_inputs)
        trainVertical(input);
      for (boolean[][] input : horizontal_inputs)
        trainHorizontal(input);
      for (boolean[][] input : backslash_inputs)
        trainBackslash(input);
      for (boolean[][] input : forwardslash_inputs)
        trainForwardslash(input);
    }
  }// KeyboardStrokes

  /*
   * ##############################Keyboad Events##############################
   */
  public void keyPressed(KeyEvent e) {
    int t = e.getKeyCode();

    if (t == KeyEvent.VK_SPACE) {
      bSpace = true;
    }
    if (t == KeyEvent.VK_ENTER) {
      bEnter = true;
    }
    if (t == KeyEvent.VK_BACK_SPACE) {
      bBackSpace = true;
    }
    repaint();
  }// keyPressed

  public void keyTyped(KeyEvent e) {
    // char t = e.getKeyChar();
  }// keyTyped

  public void keyReleased(KeyEvent e) {
    int t = e.getKeyCode();

    if (t == KeyEvent.VK_SPACE) {
      bSpace = false;
    }
    if (t == KeyEvent.VK_ENTER) {
      bEnter = false;
    }
    if (t == KeyEvent.VK_BACK_SPACE) {
      vertical.reset();
      horizontal.reset();
      backslash.reset();
      forwardslash.reset();
      bBackSpace = false;
    }

    if (t == KeyEvent.VK_ENTER) {
      bEnter = false;
    }
    if (t == KeyEvent.VK_LEFT){
      perceptron.decreaseInitialVarity();
    }
    if (t == KeyEvent.VK_RIGHT){
      perceptron.increaseInitialVarity();
    }
    if (t == KeyEvent.VK_UP){
      perceptron.increaseLearnRate();
    }
    if (t == KeyEvent.VK_DOWN){
      perceptron.decreaseLearnRate();
    }
    repaint();
  }// keyReleased

  // **************TRAIN OR TEST EACH PERCEPTRON WITH A SET OF INPUTS**********

  void trainVertical(boolean[][] inputs) {
    if (inputs == null)
      return;
    vertical.train(inputs, true);
    horizontal.train(inputs, false);
    backslash.train(inputs, false);
    forwardslash.train(inputs, false);
  }

  void trainHorizontal(boolean[][] inputs) {
    if (inputs == null)
      return;
    vertical.train(inputs, false);
    horizontal.train(inputs, true);
    backslash.train(inputs, false);
    forwardslash.train(inputs, false);
  }

  void trainBackslash(boolean[][] inputs) {
    if (inputs == null)
      return;
    vertical.train(inputs, false);
    horizontal.train(inputs, false);
    backslash.train(inputs, true);
    forwardslash.train(inputs, false);
  }

  void trainForwardslash(boolean[][] inputs) {
    if (inputs == null)
      return;
    vertical.train(inputs, false);
    horizontal.train(inputs, false);
    backslash.train(inputs, false);
    forwardslash.train(inputs, true);
  }

  void testVertical(boolean[][] inputs) {
    if (inputs == null)
      return;
    vertical.test(inputs, true);
    horizontal.test(inputs, false);
    backslash.test(inputs, false);
    forwardslash.test(inputs, false);
  }

  void testHorizontal(boolean[][] inputs) {
    if (inputs == null)
      return;
    vertical.test(inputs, false);
    horizontal.test(inputs, true);
    backslash.test(inputs, false);
    forwardslash.test(inputs, false);
  }

  void testBackslash(boolean[][] inputs) {
    if (inputs == null)
      return;
    vertical.test(inputs, false);
    horizontal.test(inputs, false);
    backslash.test(inputs, true);
    forwardslash.test(inputs, false);
  }

  void testForwardslash(boolean[][] inputs) {
    if (inputs == null)
      return;
    vertical.test(inputs, false);
    horizontal.test(inputs, false);
    backslash.test(inputs, false);
    forwardslash.test(inputs, true);
  }

  /**
   * Open a png image file and save it to memory in a 2d boolean array Pixels
   * that are closer to black will be true. Pixels closer to white are false
   * 
   * @param file
   * @return
   */
  boolean[][] getGrayscaleData(File file) {
    BufferedImage img = null;
    try {
      img = ImageIO.read(file);
    } catch (IOException e) {
      e.printStackTrace();
    }

    boolean[][] data = new boolean[5][5];
    for (int x = 0; x < 5; x++)
      for (int y = 0; y < 5; y++) {
        int pixel = img.getRGB(x, y);
        // int alpha = (pixel >> 24) & 0xff;
        int red = (pixel >> 16) & 0xff;
        int green = (pixel >> 8) & 0xff;
        int blue = (pixel) & 0xff;
        
        //a grayscale value closer to 0 is black, so evaluate to true
        data[x][y] = grayscale(red, green, blue) < 255 / 2;
      }
    return data;
  }

  /**
   * Convert a red, green, blue values to grayscale
   * 
   * @param r
   * @param g
   * @param b
   * @return
   */
  int grayscale(int r, int g, int b) {
    return (int) (0.212671 * r + 0.715160 * g + 0.072169 * b);
  }

  /**
   * Populate testing data - array lists of 2d boolean arrays Look in each
   * h,v,b,f folders and parse images inside the test folder
   */
  private void getTestingData() {
    FilenameFilter filter = new FilenameFilter() {
      public boolean accept(File dir, String name) {
        String[] names = name.split("\\.");
        String extension = names[names.length - 1];
        return extension.equalsIgnoreCase("png");
      }
    };

    File vertical_dir = new File("vertical/test");
    File horizontal_dir = new File("horizontal/test");
    File backslash_dir = new File("backslash/test");
    File forwardslash_dir = new File("forwardslash/test");

    String[] children = vertical_dir.list(filter);
    for (int i = 0; i < children.length; i++)
      vertical_tests.add(getGrayscaleData(new File("vertical/test/"
          + children[i])));
    children = horizontal_dir.list(filter);
    for (int i = 0; i < children.length; i++)
      horizontal_tests.add(getGrayscaleData(new File("horizontal/test/"
          + children[i])));
    children = backslash_dir.list(filter);
    for (int i = 0; i < children.length; i++)
      backslash_tests.add(getGrayscaleData(new File("backslash/test/"
          + children[i])));
    children = forwardslash_dir.list(filter);
    for (int i = 0; i < children.length; i++)
      forwardslash_tests.add(getGrayscaleData(new File("forwardslash/test/"
          + children[i])));
  }

  /**
   * Populate training data - array lists of 2d boolean arrays Look in each
   * h,v,b,f folders and parse images
   */
  private void getTrainingData() {
    FilenameFilter filter = new FilenameFilter() {
      public boolean accept(File dir, String name) {
        String[] names = name.split("\\.");
        String extension = names[names.length - 1];
        return extension.equalsIgnoreCase("png");
      }
    };

    File vertical_dir = new File("vertical");
    File horizontal_dir = new File("horizontal");
    File backslash_dir = new File("backslash");
    File forwardslash_dir = new File("forwardslash");

    String[] children = vertical_dir.list(filter);
    for (int i = 0; i < children.length; i++)
      vertical_inputs
          .add(getGrayscaleData(new File("vertical/" + children[i])));
    children = horizontal_dir.list(filter);
    for (int i = 0; i < children.length; i++)
      horizontal_inputs.add(getGrayscaleData(new File("horizontal/"
          + children[i])));
    children = backslash_dir.list(filter);
    for (int i = 0; i < children.length; i++)
      backslash_inputs
          .add(getGrayscaleData(new File("backslash/" + children[i])));
    children = forwardslash_dir.list(filter);
    for (int i = 0; i < children.length; i++)
      forwardslash_inputs.add(getGrayscaleData(new File("forwardslash/"
          + children[i])));
  }

  /*
   * ##############################Main runs at program startup turns the
   * program into a window calls the window's/program's init function
   * ##############################
   */
  public static void main(String[] args) {
    guifrontend window = new guifrontend();
    window.init();
    window.addWindowListener(new WindowAdapter() {
      public void windowClosing(WindowEvent e) {
        System.exit(0);
      }
    });

    window.setSize(w + 2, h + 23);
    window.setTitle("Template");
    window.setResizable(false);
    window.setLocationRelativeTo(null);// Center the window
    // window.setLocation(x, y);//Sets the upper left corner of the window to
    // this screen pixel coordinate
    window.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);// Close the program
    // on exit press
    window.setVisible(true);
    // window.pack();//Finalize window layout once everything is added; this
    // isn't required because nothing is really added
  }// main
}// Template