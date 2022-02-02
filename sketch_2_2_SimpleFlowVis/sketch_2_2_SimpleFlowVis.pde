// Define constants
int R = 200;             // pipe radius in units of pixels
float Vmax = 5.0;        // max flow velocity in units of pixels per frame
int numParticles = 5000; // number of particles used for the visualization

// Each particle has a (x,y) position.  The x's are stored in one array, and
// the y's are stored in a second array.
float[] particleX = new float[numParticles];
float[] particleY = new float[numParticles];


void setup() { 
  size(1000, 400);
  stroke(255, 255, 255);
  strokeWeight(2);
  
  // Assign a random starting (x,y) position to each particle.
  // x varies from 0 to "width", which is a built-in variable that stores the width of the canvas
  // y varies from 0 to 2*R
  for (int i=0;i<5000;i++) {
    particleX[i] = random(0,width);
    particleY[i] = random(0,2*R);
  }
}


void draw() {

  // Start each frame by clearing the screen
  background(30,30,30);  

  // Loop over all particles.  For each, update its position and draw a point on the screen
  for (int i=0;i<numParticles;i++) {
    // Calculate the distance of this particle away from the center of the pipe
    float r = abs(particleY[i]-R);
    // Calcuate the velocity of this particle according to the flow equation
    float v = Vmax * (1.0 - sq(r/R));
    // new position = old position + velocity
    particleX[i] = (particleX[i] + v);
    // If the particle has gone off the screen, the reset its position to be between 0-width
    particleX[i] = particleX[i] % width;

  
    // Draw a point at the particle's new location
    point(particleX[i], particleY[i]);
  }
}
