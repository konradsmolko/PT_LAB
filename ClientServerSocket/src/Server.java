import javafx.concurrent.Task;

import java.io.DataInputStream;
import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class Server {
    @SuppressWarnings("FieldCanBeLocal")
    private static Task<Void> getFileTask;
    private static ExecutorService executor = Executors.newFixedThreadPool(8);
//    private static ExecutorService executor = Executors.newSingleThreadExecutor();

    @SuppressWarnings("WeakerAccess")
    public static void receiveFile(Socket client) throws IOException {
        String name;
        long size;
        try (DataInputStream dataIn = new DataInputStream(client.getInputStream())) {
            try {
                name = dataIn.readUTF();
                size = dataIn.readLong();
            } catch (IOException e) {
                System.out.println(e.getMessage());
                client.close();
                return;
            }

            File file = new File(".",name);
            System.out.println("Pobieranie " + name +
                    " z " + client.getInetAddress().toString() +
                    " (" + size + " bajtów)");

            // Odbieranie pliku...
            //noinspection ResultOfMethodCallIgnored
            file.createNewFile();
            FileOutputStream fileOut = new FileOutputStream(file);
            try {
                int total = 0;
                byte[] bytes = new byte[8192];

                while (total < size) {
                    int count = dataIn.read(bytes);
                    if (count != -1) total += count;
                    else throw new IOException("Nie można odczytać całego pliku!");

                    fileOut.write(bytes, 0, count);
                }

                System.out.println("Zakończono pobieranie " + name + "!");
                fileOut.close();
            } catch (IOException e) {
                System.out.println("Błąd przy pobieraniu " + name + ": " + e.getMessage());
                //noinspection ResultOfMethodCallIgnored
                file.delete();
            }

            dataIn.close();
            fileOut.close();
        } catch (IOException e) {
            System.out.println(e.getMessage());
            return;
        }

        client.close();
    }

    public static void main(String[] args) {
        System.out.println("Otwieranie aplikacji serwera...");
        int port = 4444;
        try (ServerSocket serverSocket = new ServerSocket(port)) {
            while (true) {
                try {
                    System.out.println("Oczekiwanie na nowego klienta...");
                    final Socket client = serverSocket.accept();
                    System.out.println("Klient połączony...");
                    executor.submit(() -> {
                       try {
                           Server.receiveFile(client);
                       } catch (IOException e) {
                           System.out.println("Problem przy pobieraniu: " + e.getMessage());
                       }
                    });
                    System.out.println("...i odprawiony do nowego zadania.");

                } catch (IOException e) {
                    System.out.println("Błąd przy oczekiwaniu na klienta.");
                    System.exit(1);
                }
            }
        } catch (IOException e) {
            System.out.println("Coś poszło nie tak.");
            System.out.println(e.getMessage());
        }

        executor.shutdown();
        System.out.println("Executor zatrzymany.");
        System.out.println("Zamykanie serwera...");
    }
}