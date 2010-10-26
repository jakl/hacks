
public class Product implements Comparable<Product> {
  private String name;

  public Product(String name) {
     this.name = name;
  }

  public String getName() {
     return name;
  }

  public int compareTo(Product rhs) {
     return name.compareTo(rhs.name);
  }
}