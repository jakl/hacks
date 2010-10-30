
//import java.awt.Color;

public class Shirt extends Product {
  private int size;
  private int price;
  //private Color color;

  public Shirt() {this("");}
  
  public Shirt(String name) {
     super(name);
  }

  public int getSize() {
     return size;
  }

  public void setSize(int size) {
     this.size = size;
  }

  public int getPrice() {
     return price;
  }

  public void setPrice(int price) {
     this.price = price;
  }

/*  public Color getColor() {
     return color;
  }

  public void setColor(Color color) {
     this.color = color;
  }*/   
}