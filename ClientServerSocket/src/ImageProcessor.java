import javafx.application.Application;
import javafx.beans.property.SimpleObjectProperty;
import javafx.beans.property.SimpleStringProperty;
import javafx.fxml.FXML;
import javafx.geometry.Insets;
import javafx.scene.Group;
import javafx.scene.Scene;
import javafx.scene.control.*;
import javafx.scene.control.cell.ProgressBarTableCell;
import javafx.scene.layout.VBox;
import javafx.stage.DirectoryChooser;
import javafx.stage.FileChooser;
import javafx.stage.Stage;

import java.io.File;
import java.net.URL;
import java.util.List;
import java.util.ResourceBundle;

public class ImageProcessor extends Application {
    @FXML TableColumn<ImageProcessingJob, String> imageNameColumn;
    @FXML TableColumn<ImageProcessingJob, Double> progressColumn;
    @FXML TableColumn<ImageProcessingJob, String> statusColumn;

    private TableView tableView = new TableView();

    public static void main(String[] args) { launch(args); }

    @Override
    public void initialize(URL url, ResourceBundle rb) {
        imageNameColumn.setCellValueFactory(
                param -> new SimpleStringProperty(param.getValue().getFile().getName()));
        statusColumn.setCellValueFactory(
                param -> param.getValue().getStatusProperty());
        progressColumn.setCellFactory(
                ProgressBarTableCell.<ImageProcessingJob>forTableColumn());
        progressColumn.setCellValueFactory(
                param -> param.getValue().getProgressProperty().asObject());


    }

    @Override
    public void start(Stage primaryStage) {
        primaryStage.setTitle("Przetwarzanie obrazów na czarno-białe");
        primaryStage.setWidth(1000);
        primaryStage.setHeight(750);

        Scene scene = new Scene(new Group());

        final Label label = new Label("Lista plików");
        Button readButton = new Button("Select files...");
        Button writeButton = new Button("Choose output directory...");
        Button runSequential = new Button("Run\n(Sequential)");
        Button runParallel = new Button("Run\n(Parallel with commonPool)");
        Button changePool = new Button("Change # of threads in pool\n(default 4)");
        Button runCustom = new Button("Run\n(Parallel, custom thread pool)");

        readButton.setOnAction(event -> selectFiles());


        final VBox vbox = new VBox();
        vbox.setSpacing(5);
        vbox.setPadding(new Insets(10, 0, 0, 10));
        vbox.getChildren().addAll(label, tableView);
                //(label, tableView, readButton, writeButton, runSequential, runParallel, changePool, runCustom);
        ((Group) scene.getRoot()).getChildren().addAll(vbox);

        primaryStage.setScene(scene);
        primaryStage.show();
    }

    private void selectFiles() {
        FileChooser fileChooser = new FileChooser();
        fileChooser.getExtensionFilters().add(
                new FileChooser.ExtensionFilter("JPG Images", "*.jpg"));
        List<File> selectedFiles = fileChooser.showOpenMultipleDialog(null);
    }

    private void selectOutDir() {
        DirectoryChooser directoryChooser = new DirectoryChooser();
        File chosenDir = directoryChooser.showDialog(null);
    }
}
