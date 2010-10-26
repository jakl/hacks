import java.lang.reflect.Method;

public class sample {
    public sample() {

        StringBuilder cake = null;
        try {
            cake = new StringBuilder();
            cake.append("wertyuiop");
            Object obj = cake;
            Method len = obj.getClass().getMethod("length", null);
            Method del = obj.getClass().getMethod("deleteCharAt", int.class);
            if ((Integer) len.invoke(obj) > 0) {
                del.invoke(obj, 0);
                System.out.println("deleted");
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
        System.out.println(cake);
        Object obj1 = new frosting(1), obj2 = new frosting(2);
        Method git = null, sat = null;
        try {
            git = obj1.getClass().getMethod("getCount");
            sat = obj2.getClass().getMethod("setCount", Object.class);
            if (git.getReturnType().equals(sat.getGenericParameterTypes()[0]))
                sat.invoke(obj2, git.invoke(obj1));

        } catch (Exception e) {
            e.printStackTrace();
        }

        System.out.println("obj1: " + ((frosting) obj1).getCount());
        System.out.println("obj2: " + ((frosting) obj2).getCount());

    }
}

class frosting {
    int g;

    public frosting(int x) {
        g = x;
    }

    public frosting() {
        g = 42;
    }

    public int getCount() {
        return g;
    }

    public void setCount(int x) {
        g = x;
    }
}
