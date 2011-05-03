//JFrame Template
//author: Alrecenk
//Don't forget to go to void main to change filename
//Window width, height are the static integers w, h

//Imports from the JDK library and will be handled by the constructor

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

//Mouse events
import java.awt.event.MouseListener;//Knows when mouse does anything
import java.awt.event.MouseMotionListener;//Fires on mouse movement
import java.awt.event.MouseEvent;//Knows mouse position

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
import javax.swing.JFileChooser;
import javax.swing.JFrame;

//Timer for screen refresh rate
import javax.swing.Timer;
import javax.swing.filechooser.FileFilter;

@SuppressWarnings("serial")
public class guifrontend extends JFrame // For a browser Java-Applet instead of
    // JFrame use 'extends JApplet' and
    // remove the main function
    // This class (java code file) will function as a JFrame, an ActionListener,
    // a MouseListener, etc
    implements ActionListener, MouseListener, KeyListener, MouseMotionListener {

  // Variable Declaration Pool
  private Container pane;
  Image offscreen;
  Graphics screen;
  static int w, h;
  boolean bSpace, bEnter, bBackSpace, bUp, bDown, bLeft, bRight, bW, bS, bA,
      bD;// Keyboard events

  /*
   * Program Specific variables Not part of TemplateBe as descriptive with
   * variable names as possible, then think of being conciseWill someone
   * understand your program if they read it unassisted?
   */

  perceptron vertical, horizontal, backslash, forwardslash;
  ArrayList<boolean[][]> vertical_inputs = new ArrayList<boolean[][]>(),
      horizontal_inputs = new ArrayList<boolean[][]>(),
      backslash_inputs = new ArrayList<boolean[][]>(),
      forwardslash_inputs = new ArrayList<boolean[][]>();

  /*
   * Function Description init(): Initializes all variables needed to make and
   * sustain this JFrame template program
   */
  void init() {
    // Variable Initialization Pool setting values

    // Screen Width / Height
    w = 800;
    h = 800;

    // User Input through Keyboard
    bSpace = false;
    bEnter = false;
    bBackSpace = false;
    bUp = false;
    bDown = false;
    bLeft = false;
    bRight = false;
    bW = false;
    bS = false;
    bA = false;
    bD = false;

    // Activating window and listener functionality
    pane = getContentPane();
    pane.addMouseListener(this);// Add this class to pane as a MouseListener
    // that pane will fire when it sees the mouse do
    // something from it's perspective
    pane.addMouseMotionListener(this);
    pane.addKeyListener(this);
    pane.requestFocus();

    // Clock
    Timer clock = new Timer(20, this); // Constructed with (delay in ms,
    // ActionListener to fire on)
    clock.start();

    // Program Specific variables Not part of Template
    vertical = new perceptron("vertial", 5, 5);
    horizontal = new perceptron("horizontal", 5, 5);
    backslash = new perceptron("backslash", 5, 5);
    forwardslash = new perceptron("forwardslash", 5, 5);
    
    getTestingData();

  }// init

  /*
   * Function Descriptino point(Graphics g): Is called from repaint() and
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
  }// paint2

  /*
   * Function Description actionPerformed(ActionEvent): Fires on timer tick
   */
  public void actionPerformed(ActionEvent e) {
    KeyboardStrokes();
    repaint();
  }// actionPerformed

  /*
   * Function Description KeyboardStrokes(): Up Down Left Right if / else
   * statements with code bodies for each.if public / private isn't specified it
   * is assumed private meaning the function can not be accessed from another
   * class (java code file) which is cleaner and safer
   */
  void KeyboardStrokes() {
    if (bSpace) {

    } else {

    }
    if (bEnter) {

    } else {

    }
    if (bBackSpace) {

    } else {

    }
    if (bUp) {
    } else {

    }

    if (bDown) {
    } else {

    }

    if (bLeft) {
    } else {

    }

    if (bRight) {
    } else {

    }
    if (bW) {

    } else {

    }
    if (bS) {

    } else {

    }
    if (bA) {

    } else {

    }
    if (bD) {

    } else {

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

    if (t == KeyEvent.VK_LEFT) {
      bLeft = true;
    }
    if (t == KeyEvent.VK_RIGHT) {
      bRight = true;
    }
    if (t == KeyEvent.VK_UP) {
      bUp = true;
    }
    if (t == KeyEvent.VK_DOWN) {
      bDown = true;
    }

    if (t == KeyEvent.VK_A) {
      bA = true;
    }
    if (t == KeyEvent.VK_D) {
      bD = true;
    }
    if (t == KeyEvent.VK_W) {
      bW = true;
    }
    if (t == KeyEvent.VK_S) {
      bS = true;
    }
    repaint();
  }// keyPressed

  public void keyTyped(KeyEvent e) {
    char t = e.getKeyChar();
  }// keyTyped

  public void keyReleased(KeyEvent e) {
    int t = e.getKeyCode();

    if (t == KeyEvent.VK_SPACE) {
      vertical.reset();
      horizontal.reset();
      backslash.reset();
      forwardslash.reset();
      bSpace = false;
    }
    if (t == KeyEvent.VK_ENTER) {
      bEnter = false;
    }
    if (t == KeyEvent.VK_BACK_SPACE) {
      bBackSpace = false;
    }

    if (t == KeyEvent.VK_ENTER) {
      for(boolean[][] input : vertical_inputs)
        trainVertical(input);
      for(boolean[][] input : horizontal_inputs)
        trainHorizontal(input);
      for(boolean[][] input : backslash_inputs)
        trainBackslash(input);
      for(boolean[][] input : forwardslash_inputs)
        trainForwardslash(input);
    }
    if (t == KeyEvent.VK_LEFT) {
      trainBackslash(getInputsFromPNG());
      bLeft = false;
    }
    if (t == KeyEvent.VK_RIGHT) {
      trainForwardslash(getInputsFromPNG());
      bRight = false;
    }
    if (t == KeyEvent.VK_UP) {
      trainVertical(getInputsFromPNG());
      bUp = false;
    }
    if (t == KeyEvent.VK_DOWN) {
      trainHorizontal(getInputsFromPNG());
      bDown = false;
    }

    if (t == KeyEvent.VK_A) {
      bA = false;
    }
    if (t == KeyEvent.VK_D) {
      bD = false;
    }
    if (t == KeyEvent.VK_W) {
      bW = false;
    }
    if (t == KeyEvent.VK_S) {
      bS = false;
    }
    repaint();
  }// keyReleased

  /*
   * ##############################Mouse Events##############################
   */
  public void mousePressed(MouseEvent e) {
    pane.requestFocus();
  }// mousePressed

  public void mouseClicked(MouseEvent e) {
  }// mouseClicked

  public void mouseReleased(MouseEvent e) {
  }// mouseReleased

  public void mouseEntered(MouseEvent e) {
  }// mouseEntered

  public void mouseExited(MouseEvent e) {
  }// mouseExited

  public void mouseMoved(MouseEvent e) {
  }// mouseMoved

  public void mouseDragged(MouseEvent e) {
  }// mouseDragged

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

  boolean[][] getInputsFromPNG() {
    JFileChooser fd = new JFileChooser(".");
    int fdStatus = fd.showOpenDialog(null);
    if (fdStatus == JFileChooser.APPROVE_OPTION)
      return getGrayscaleData(new File(fd.getSelectedFile().getPath()));
    return null;
  }

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
        data[x][y] = grayscale(red, green, blue) < 255 / 2;
      }
    return data;
  }

  int grayscale(int r, int g, int b) {
    return (int) (0.212671 * r + 0.715160 * g + 0.072169 * b);
  }

  private void getTestingData() {
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
      vertical_inputs.add(getGrayscaleData(new File("vertical/"+children[i])));
    children = horizontal_dir.list(filter);
    for (int i = 0; i < children.length; i++)
      horizontal_inputs.add(getGrayscaleData(new File("horizontal/"+children[i])));
    children = backslash_dir.list(filter);
    for (int i = 0; i < children.length; i++)
      backslash_inputs.add(getGrayscaleData(new File("backslash/"+children[i])));
    children = forwardslash_dir.list(filter);
    for (int i = 0; i < children.length; i++)
      forwardslash_inputs.add(getGrayscaleData(new File("forwardslash/"+children[i])));
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