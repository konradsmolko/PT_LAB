import javafx.application.Application;
import javafx.beans.property.DoubleProperty;
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

import java.awt.event.ActionEvent;
import java.io.File;
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.ForkJoinPool;

public class ImageProcessor extends Application {
    @FXML
    private TableColumn<ImageProcessingJob, String> imageNameColumn;
    @FXML
    private TableColumn<ImageProcessingJob, Double> progressColumn;
    @FXML
    private TableColumn<ImageProcessingJob, String> statusColumn;

    private List<ImageProcessingJob> jobs = new ArrayList<>();
//    private DoubleProperty progressProp;
    private ForkJoinPool pool;

    @FXML
    void processFiles(ActionEvent event) {
        new Thread(this::backgroundJob).start();
    }

    private void backgroundJob() {
        jobs.forEach(pool::execute);
//        for (ImageProcessingJob job : jobs) pool.execute(job);
        while (!pool.isTerminated());
        jobs.clear();
    }

    public static void main(String[] args) { launch(args); }

    @FXML
    private void initialize() {
        imageNameColumn.setCellValueFactory( // nazwa pliku
                param -> new SimpleStringProperty(param.getValue().getFilename()));
        statusColumn.setCellValueFactory( // status przetwarzania
                param -> param.getValue().getStatusProperty());
        progressColumn.setCellFactory( // wykorzystanie paska postępu
                ProgressBarTableCell.forTableColumn());
        progressColumn.setCellValueFactory( // postęp przetwarzania
                param -> param.getValue().getProgressProperty());


    }

    @Override
    public void start(Stage primaryStage) {
        imageNameColumn = new TableColumn<>();
        progressColumn = new TableColumn<>();
        statusColumn = new TableColumn<>();
        initialize();
        primaryStage.setTitle("Przetwarzanie obrazów na czarno-białe");
        primaryStage.setWidth(1000);
        primaryStage.setHeight(750);

        Scene scene = new Scene(new Group());

        final Label label = new Label("Lista plików");
        TextField textField = new TextField();

        Button readButton = new Button("Select files and output directory...");
        readButton.setOnAction(event -> selectFiles());
//        Button writeButton = new Button("Choose output directory...");
//        writeButton.setOnAction(event -> selectOutDir());
        Button runSequential = new Button("Run\n(Sequential)");
        runSequential.setOnAction(event -> runSequential());
        Button runParallel = new Button("Run\n(Parallel with commonPool)");
        runParallel.setOnAction(event -> runParallel());
        Button runCustom = new Button("Run\n(Parallel, custom thread pool)");
        runCustom.setOnAction(event -> runCustom(textField));



        TableView tableView = new TableView();
        tableView.getColumns().addAll(imageNameColumn, progressColumn, statusColumn);
        VBox vbox = new VBox();
        vbox.setSpacing(5);
        vbox.setPadding(new Insets(10, 0, 0, 10));
        vbox.getChildren().addAll(label, tableView, textField, readButton,
                runSequential, runParallel, runCustom);
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

        DirectoryChooser directoryChooser = new DirectoryChooser();
        File chosenDir = directoryChooser.showDialog(null);

        for (File file : selectedFiles) {
            jobs.add(new ImageProcessingJob(file, chosenDir, null));
        }
    }

    private void runCustom(TextField textField) {
        int count = Integer.parseInt(textField.getText());
        pool = new ForkJoinPool(count);
    }

    private void runParallel() {
        pool = new ForkJoinPool(4);
    }

    private void runSequential() {
        pool = new ForkJoinPool(1);
        processFiles(null);
    }
}
