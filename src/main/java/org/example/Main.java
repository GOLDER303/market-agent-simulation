package org.example;


public class Main {
    public static void main(String[] args) {
        System.out.println("Hello World!");

        WindowManager window = new WindowManager("Market Agent Simulation", 1600, 900, false);
        window.init();

        System.out.println("Window initialized successfully");

        while (!window.windowShouldClose()) {
            window.update();
        }

        window.cleanup();
    }
}