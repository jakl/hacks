public class Car extends Product {
   private double price;
   private int numSeats;
   private String maker;
   
   public Car(String name) {
      super(name);
   }

   public double getPrice() {
      return price;
   }

   public void setPrice(double price) {
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
}