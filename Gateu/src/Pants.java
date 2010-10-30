
public class Pants extends Product {
  public int length;
  public int size;
  public double cost;
  
  public Pants() {this("");}
  
  public Pants(String name) {
     super(name);
  }
  public double getCost() {
      return cost;
   }
   public void setCost(double cost) {
      this.cost = cost;
   }
  public int getLength() {
     return length;
  }
  public void setLength(int length) {
     this.length = length;
  }
  public int getSize() {
     return size;
  }
  public void setSize(int size) {
     this.size = size;
  }

  
  
}