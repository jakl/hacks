
public class Outfit extends Product {
  public Pants pants;
  public Shirt shirt;

  public Outfit() {this("");}
  
  public Outfit(String name) {
     super(name);
  }

  public Pants getPants() {
     return pants;
  }

  public void setPants(Pants pants) {
     this.pants = pants;
  }

  public Shirt getShirt() {
     return shirt;
  }

  public void setShirt(Shirt shirt) {
     this.shirt = shirt;
  }
}