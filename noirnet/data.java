import java.awt.image.BufferedImage;
import java.io.File;
import java.io.FilenameFilter;
import java.io.IOException;
import java.util.ArrayList;

import javax.imageio.ImageIO;

//An array of arraylists that contain double arrays makes type checking more like duck typing
@SuppressWarnings("unchecked")
public class data {
  
  public ArrayList<double[]>[] trainNumerals = new ArrayList[10];
  public ArrayList<double[]>[] testNumerals = new ArrayList[10];
  
  static final String trainFolder = "train";
  static final String testFolder = "test";
  
  //Filter to grab only png files
  static final FilenameFilter filter = new FilenameFilter() {
    public boolean accept(File dir, String name) {
      String[] names = name.split("\\.");
      String extension = names[names.length - 1];
      return extension.equalsIgnoreCase("png");
    }
  };
  
  /**
   * Constructor populates training and testing data from png files within the project's folder structure
   */
  public data(){
    for(int i = 0; i < 10; i++){
      
      //Store training png data into RAM
      trainNumerals[i] = new ArrayList<double[]>();
      File dir = new File(trainFolder + "/" + Integer.toString(i));
      for (String child : dir.list(filter))
        trainNumerals[i].add(getGrayscaleData(new File(dir.getPath() + "/" + child)));
      
      //Store testing png data into RAM
      testNumerals[i] = new ArrayList<double[]>();
      dir = new File(testFolder + "/" + Integer.toString(i));
      for (String child : dir.list(filter))
        testNumerals[i].add(getGrayscaleData(new File(dir.getPath() + "/" + child)));
    }
  }
  
  /**
   * Open a png image file and save it to memory in an array of greyscale Pixel values
   * 
   * @param file
   * @return
   */
  double[] getGrayscaleData(File file) {
    BufferedImage img = null;
    try {
      img = ImageIO.read(file);
    } catch (IOException e) {
      e.printStackTrace();
    }

    double[] data = new double[20*20];//20 x 20 image data
    for (int x = 0; x < 5; x++)
      for (int y = 0; y < 5; y++) {
        int pixel = img.getRGB(x, y);
        // int alpha = (pixel >> 24) & 0xff;
        int red = (pixel >> 16) & 0xff;
        int green = (pixel >> 8) & 0xff;
        int blue = (pixel) & 0xff;
        
        //a grayscale value closer to 0 is black, so evaluate to true
        data[x + 5*y] = grayscale(red, green, blue);
      }
    return data;
  }

  /**
   * Convert red, green, blue values to grayscale
   */
  int grayscale(int r, int g, int b) {
    return (int) (0.212671 * r + 0.715160 * g + 0.072169 * b);
  }
}
