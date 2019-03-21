import javafx.application.Application;
import javafx.concurrent.Task;
import javafx.fxml.FXML;
import javafx.scene.Scene;
import javafx.scene.control.Button;
import javafx.scene.control.Label;
import javafx.scene.control.ProgressBar;
import javafx.scene.layout.TilePane;
import javafx.stage.FileChooser;
import javafx.stage.Stage;

import java.io.File;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class Client extends Application {
    //pola klasy kontrolera:
    @FXML private Label statusLabel;
    @FXML private ProgressBar progressBar;
    private Task<Void> sendFileTask;
    private ExecutorService executor = Executors.newSingleThreadExecutor();


    public static void main(String[] args) {
        launch(args);
    }

    @Override
    public void start(Stage primaryStage) {
        // Przygotowanie okna
        primaryStage.setTitle("Aplikacja kliencka");

        statusLabel = new Label();
        statusLabel.setVisible(false);
        progressBar = new ProgressBar();
        progressBar.setVisible(false);

        Button btn_wybor = new Button();
        btn_wybor.setText("Wybierz plik");
        btn_wybor.setOnAction(event -> chooseFile(primaryStage));

        Button btn_wyslanie = new Button();
        btn_wyslanie.setText("Wyślij plik");
        btn_wyslanie.setOnAction(event -> sendFile());

        // Dodanie rzeczy do okna
        TilePane root = new TilePane();
        root.getChildren().add(btn_wybor);
        root.getChildren().add(btn_wyslanie);
        root.getChildren().add(statusLabel);
        root.getChildren().add(progressBar);

        // Wyświetlenie okna
        primaryStage.setScene(new Scene(root, 300,100));
        primaryStage.show();
    }

    private void chooseFile(Stage stage) {
        FileChooser fc = new FileChooser();
        fc.setTitle("Choose a file");
        File file = fc.showOpenDialog(stage);
        if (file == null) return;

        sendFileTask = new SendFileTask(file);
        statusLabel.textProperty().bind(sendFileTask.messageProperty());
        progressBar.progressProperty().bind(sendFileTask.progressProperty());
        statusLabel.setVisible(true);
        progressBar.setVisible(true);
    }

    private void sendFile() {
        if(sendFileTask == null)
            return;
        executor.submit(sendFileTask);
    }
}
