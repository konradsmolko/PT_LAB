import javafx.application.Platform;
import javafx.beans.InvalidationListener;
import javafx.beans.property.DoubleProperty;
import javafx.beans.value.ChangeListener;
import javafx.beans.value.ObservableValue;

import javax.imageio.ImageIO;
import java.awt.*;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import java.nio.file.Path;
import java.nio.file.Paths;

class ImageProcessingJob implements Runnable{
    private File file;
    private File dir;
    private DoubleProperty progressProp;
    private String currentStatus = "Waiting...";

    ImageProcessingJob(File file, File dir, DoubleProperty progressProp) {
        this.file = file;
        this.dir = dir;
        this.progressProp = progressProp;
    }

    private void convertToGrayscale(
            File sourceFile,
            File destDir,
            DoubleProperty progressProp
    ) {
        currentStatus = "Processing";
        try {
            BufferedImage original = ImageIO.read(sourceFile);
            BufferedImage grayscale = new BufferedImage(
                    original.getWidth(),
                    original.getHeight(),
                    original.getType());

            for (int i = 0; i < original.getWidth(); i++) {
                for (int j = 0; j < original.getHeight(); j++) {
                    int red = new Color(original.getRGB(i, j)).getRed();
                    int green = new Color(original.getRGB(i, j)).getGreen();
                    int blue = new Color(original.getRGB(i, j)).getBlue();

                    int luminosity = (int)(0.21*red + 0.71*green + 0.07*blue);
                    int newPixel = new Color(luminosity,luminosity,luminosity).getRGB();

                    grayscale.setRGB(i, j, newPixel);
                }
                double progress = (1.0 + i) / original.getWidth();
                //Platform.runLater(() -> progressProp.set(progress));
            }

            Path outputPath = Paths.get(destDir.getAbsolutePath(), sourceFile.getName());

            ImageIO.write(grayscale, "jpg", outputPath.toFile());

        } catch (IOException e) {
            currentStatus = "Error";
            System.out.println("Błąd convertToGrayscale.");
        }
        currentStatus = "Finished";
    }

    @Override
    public void run() {
        convertToGrayscale(file, dir, progressProp);
    }

    String getFilename() {
        return file.getName();
    }

    ObservableValue<String> getStatusProperty() {
        return new ObservableValue<String>() {
            @Override
            public void addListener(ChangeListener<? super String> listener) {

            }

            @Override
            public void removeListener(ChangeListener<? super String> listener) {

            }

            @Override
            public String getValue() {
                return currentStatus;
            }

            @Override
            public void addListener(InvalidationListener listener) {

            }

            @Override
            public void removeListener(InvalidationListener listener) {

            }
        };
    }

    ObservableValue<Double> getProgressProperty() {
        return new ObservableValue<Double>() {
            @Override
            public void addListener(ChangeListener<? super Double> listener) {

            }

            @Override
            public void removeListener(ChangeListener<? super Double> listener) {

            }

            @Override
            public Double getValue() {
                return progressProp.getValue();
            }

            @Override
            public void addListener(InvalidationListener listener) {

            }

            @Override
            public void removeListener(InvalidationListener listener) {

            }
        };
    }
}
