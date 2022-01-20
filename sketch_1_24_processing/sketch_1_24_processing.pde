/* CSCI 5609: Visualization
 * In-Class 1/24: Processing
 *
 * Author: Bridger Herman <herma582@umn.edu>
 */
 
// GLOBAL VARS
final color BAR_COLOR = color(163, 222, 255, 255);
final float BAR_WIDTH = 100;
final float[] DATA = {1.0, 1.5, 5.0, 2.5, 0.0};

// HELPER FUNCTIONS
float remap(float value, float min1, float max1, float min2, float max2) {
    return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
}

// (Part 3)
void drawColumn(int index) {
  fill(BAR_COLOR);
  // Remap from data space --> 0 to 1
  float normalizedValue = remap(DATA[index], min(DATA), max(DATA), 0, 1);
  rect(width * (index / (float)DATA.length), height, BAR_WIDTH, -height * normalizedValue);
}

void setup() {
  // PART 1: Draw some graphics
  // Set the graphics window size (720p) and background color (white)
  size(1280, 720);
  background(255, 255, 255);

  // Disable stroke
  strokeWeight(0);
  
  // Make a couple of bars in the middle-ish.
  if (false) {
    fill(BAR_COLOR);
    rect(0, height, BAR_WIDTH, -100);
    rect(width * 0.2, height, BAR_WIDTH, -150);
    rect(width * 0.4, height, BAR_WIDTH, -500);
    rect(width * 0.6, height, BAR_WIDTH, -250);
    rect(width * 0.8, height, BAR_WIDTH, 0);
  }
    
  // PART 2: Drawing a bar chart (better)
  if (true) {
    for (int i = 0; i < DATA.length; i++) {
      drawColumn(i);
    }
  }
}

void draw() {
}
