import javafx.concurrent.Task;

import java.io.DataOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.net.Socket;

public class SendFileTask extends Task<Void> {
    private File file;

    SendFileTask(File file) { this.file = file; }

    @Override
    protected Void call() {
        updateMessage("Rozpoczynanie...");
        String host = "localhost";
        int port = 4444;
        updateMessage("Łączenie...");
        updateProgress(0,100);
        try (Socket server = new Socket(host, port)) {
            if (!server.isConnected()) {
                updateMessage("Nie można połączyć.");
                updateProgress(100,100);
                return null;
            }

            try (DataOutputStream dataOut = new DataOutputStream(server.getOutputStream())) {
                dataOut.writeUTF(file.getName());
                dataOut.writeLong(file.length());

                long length = file.length();
                long uploaded = 0;

                updateMessage("Wysyłanie pliku...");
                updateProgress(0,100);

                FileInputStream fileIn = new FileInputStream(file);
                byte[] bytes = new byte[8192];

                // Wysyłanie pliku...
                while (uploaded != length) {
                    int count = fileIn.read(bytes);
                    dataOut.write(bytes, 0, count);
                    uploaded += count;

                    updateProgress(uploaded, length);
                }
                fileIn.close();
            } catch (IOException e) {
                updateMessage("Problem przy wysyłaniu pliku: " + e.getMessage());
                return null;
            }
        } catch (Exception e) {
            updateMessage("Coś poszło nie tak z Socket!");
            System.err.println(e.getMessage());
            return null;
        }

        updateMessage("Plik pomyślnie wysłany!");
        updateProgress(100,100);
        return null;
    }
}
