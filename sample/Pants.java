public class Pants extends Product {
   public int length;
   public int size;
   public double price;
   
   public Pants() {this("");}
   
   public Pants(String name) {
      super(name);
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
   public double getPrice() {
      return price;
   }
   public void setPrice(double price) {
      this.price = price;
   }
   
   
}
