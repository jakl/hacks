public class Shoes extends Product {
   public int size;
   public double price;
   
   public Shoes() {this("");}
   
   public Shoes(String name) {
      super(name);
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
