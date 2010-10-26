//import java.awt.Color;

public class Car extends Product {
  private int price;
  private int numSeats;
  private String maker;
  //private Color color;
 
  
  public Car(String name) {
     super(name);
  }

  public int getPrice() {
     return price;
  }

  public void setPrice(int price) {
     this.price = price;
  }
  


  public int getNumSeats() {
     return numSeats;
  }

  public void setNumSeats(int numSeats) {
     this.numSeats = numSeats;
  }

  public String getMaker() {
     return maker;
  }

  public void setMaker(String maker) {
     this.maker = maker;
  }
  /*Leave out until we do String parsing for each color name, like blue, red, green, etc...
  public Color getColor() {
      return color;
   }

   public void setColor(Color color) {
      this.color = color;
   }*/
}