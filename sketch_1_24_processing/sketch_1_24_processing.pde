import org.gicentre.utils.stat.*;        // For chart classes.

/* CSCI 5609: Visualization
 * In-Class 1/24: Processing
 *
 * Author: Bridger Herman <herma582@umn.edu>
 */
 
// GLOBAL VARS
final color BAR_COLOR = color(163, 222, 255, 255);
final float BAR_WIDTH = 100;

// Min temperature (in degrees F) for January 24th, 2017-2021
// DATA SOURCE: https://www.weather.gov/wrh/climate?wfo=mpx
final float[] DATA = {32, 10, -8, 29, 15};

final String[] LABELS = {"2017", "2018", "2019", "2020", "2021"};

// Sometimes helpful to manually declare min/max for y axis
final float Y_MAX = 40;
final float Y_MIN = -10;

BarChart chart;

// HELPER FUNCTIONS
float remap(float value, float min1, float max1, float min2, float max2) {
    return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
}

// (Part 2)
void drawColumn(int index) {
  
  // Remap from data space --> 0 to 1
  float normalizedValue = remap(DATA[index], Y_MIN, Y_MAX, 0, 1);
  float barX = width * (index / (float)DATA.length);
  
  fill(BAR_COLOR);
  rect(barX, height, BAR_WIDTH, -height * normalizedValue);
  
  fill(0);
  text(LABELS[index], barX, height - 50);
}

void setup() {
  // Set the graphics window size (720p) and background color (white)
  size(1280, 720);
  
  // Part 3: set up bar chart so we don't have to do it every frame
  chart = new BarChart(this);
  chart.setData(DATA);
  chart.setBarLabels(LABELS);
  chart.setBarColour(BAR_COLOR);
  chart.setBarGap(2);
  chart.showValueAxis(true);
  chart.showCategoryAxis(true);
}

// Get in the habit of putting all your drawing calls in draw() - even if it's a static image!
void draw() {
  background(255, 255, 255);

  // Disable stroke
  noStroke();
  
  // PART 1: Draw some graphics
  // Make a couple of bars in the middle-ish.
  if (true) {
    fill(BAR_COLOR);
    rect(0, height, BAR_WIDTH, -605);
    rect(width * 0.2, height, BAR_WIDTH, -288);
    rect(width * 0.4, height, BAR_WIDTH, -29);
    rect(width * 0.6, height, BAR_WIDTH, -562);
    rect(width * 0.8, height, BAR_WIDTH, -360);
    for (int i = 0; i < DATA.length; i++) {
      println(remap(DATA[i], Y_MIN, Y_MAX, 0, height));
    }
    println();
  }
    
  // PART 2: Drawing a bar chart (better)
  if (false) {
    for (int i = 0; i < DATA.length; i++) {
      drawColumn(i);
    }
    // draw "zero" line
    float zero = remap(0, Y_MIN, Y_MAX, 0, 1);
    stroke(0);
    line(0, height - zero * height, width, height - zero * height);
  }
  
  // PART 3: Using a library
  if (false) {
    // Draw the chart as a big rectangle to splat onto the canvas
    chart.draw(10, 10, width - 20, height - 20);
  }
  
  // PART 4: Titles and legends
  if (false) {
    // Add title
    fill(0);
    textAlign(CENTER);
    textSize(50); //px
    text("Min Temp (F) on January 24th", width / 2, 50);
    
    // Add legend
    textSize(10);
    float legendX = 100;
    float legendY = 50;
    fill(240);
    rect(legendX, legendY, 120, 70);
    fill(BAR_COLOR);
    rect(legendX + 10, legendY + 10, 50, 50);
    fill(0);
    textAlign(LEFT);
    text("Values", legendX + 70, legendY + 30);
    
    // Adjust chart "margins" to fit title
    chart.draw(10, 50, width - 20, height - 60);
    
    // Another way to draw a 'zero' line (on top of chart)
    float zero = remap(0, Y_MIN, Y_MAX, height - 10, 50);
    stroke(150);
    line(10, zero, width - 20, zero);
  }
  
  // EXTRA 1: Interactivity
  if (false) {
    chart.draw(10, 10, width - 20, height - 20);

    // Check which category mouse x is over
    int mouseIndex = int ((mouseX / (float) width) * DATA.length);

    fill(color(255, 255, 255, 100));
    rect(mouseX, mouseY, -50, -20);
    fill(0);
    text(DATA[mouseIndex], mouseX - 50, mouseY - 10);
  }
  
  // EXTRA 2: Beyond Bar Charts
  if (false) {
    // Map data values to circle size AND color instead!
    float minRadius = 5.0;
    float maxRadius = 200.0;
    color c0 = color(80, 70, 56);
    color c1 = color(246, 176, 84);
    
    for (int i = 0; i < DATA.length; i++) {
      float circleRadius = remap(DATA[i], min(DATA), max(DATA), minRadius, maxRadius);
      float normalized = remap(DATA[i], min(DATA), max(DATA), 0, 1);

      // WARNING: this interpolates in HSB space - we'll learn why this isn't ideal!
      color c = lerpColor(c0, c1, normalized);
      fill(c);

      // Add the circle
      float cx = (i / (float)DATA.length) * width + maxRadius / 2;
      float cy = height / 2 + 100;
      circle(cx, cy, circleRadius);

      // Add a text label
      fill(30);
      textAlign(CENTER);
      text(LABELS[i], cx, cy - circleRadius - 30);
    }

    // Make a legend
    fill(240);
    rect(0, 0, 300, 300);
    fill(c0);
    circle(100, 50, minRadius);
    fill(c1);
    circle(100, 200, maxRadius);
    fill(30);
    text(min(DATA), 250, 50);
    text(max(DATA), 250, 200);
  }
}
