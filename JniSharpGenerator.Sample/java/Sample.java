package JniSharpGenerator.Sample;

public class Sample { 

    public static int value;

    public static void main (String args[]) throws java.io.IOException {
        System.out.println("[Java] Hello from JniSharpGenerator java sample!");
        java.util.Scanner scanner = new java.util.Scanner(System.in);
        System.out.print("[Java] Enter an integer: ");
        value = scanner.nextInt();
        nativeSharpMethod();
        scanner.close();
    }

    public static int add(int a, int b) {
        return a + b;
    }
    
    public static native void nativeSharpMethod();
}