
// Realtime Face Detection and camera tracking
// Using EmguCV cross platform .Net wrapper to the Intel OpenCV image processing library for C#.Net
// Tutorial code written by Sergio Andrés Guitérrez Rojas
// https://www.codeproject.com/members/sergio-a-gutierrez

// Derivative work protected by The Code Project Open License (CPOL) 1.02
// see CPOL in project files
// Derivative work written by Liam Lawless


using System;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;


namespace MultiFaceRec
{
    public partial class FrmPrincipal : Form
    {
        //Declararation of all variables, vectors and haarcascades
        Image<Bgr, Byte> currentFrame;
        Capture grabber;
        HaarCascade face;
        Image<Gray, byte> gray = null;

        public FrmPrincipal()
        {
            //InitializeComponent();

            //Load haarcascades for face detection
            face = new HaarCascade("haarcascade_frontalface_default.xml");
           
            //Initialize the capture device
            grabber = new Capture();
            grabber.QueryFrame();

            //Initialize the FrameGraber event
            Application.Idle += new EventHandler(FrameGrabber);
        }


        void FrameGrabber(object sender, EventArgs e)
        {   
            // Hides the form from showing up
            this.Hide();

            while (true)
            {
                //Get the current frame form capture device
                currentFrame = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                //Convert it to Grayscale
                gray = currentFrame.Convert<Gray, Byte>();

                //Face Detector
                MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(face, 1.2, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));

                //Action for each element detected
                foreach (MCvAvgComp f in facesDetected[0])
                {
                    //draw the face detected in the 0th (gray) channel with blue color
                    currentFrame.Draw(f.rect, new Bgr(Color.Pink), 2);

                    // Detect if the face is too close to boundary
                    // Define boundaries of the camera
                    int lowerBoundary = 120;
                    int upperBoundary = 40;
                    int leftBoundary = 50;
                    int rightBoundary = 200;

                    int posX = f.rect.X;
                    int posY = f.rect.Y;

                    // print the current position of the face *temp
                    var posN = new Point(f.rect.X, f.rect.Y);
                    Console.WriteLine("Face Position: " + posN.ToString());

                    // only trigger if there is a single face detected
                    // avoids conflicting camera movements
                    if (facesDetected.Length == 1)
                    {
                        if (posX <= leftBoundary)
                        {
                            // insert camera controls
                            Console.WriteLine("Camera Right");
                        }
                        if (posX >= rightBoundary)
                        {
                            // insert camera controls
                            Console.WriteLine("Camera Left");
                        }
                        if (posY <= upperBoundary)
                        {
                            // insert camera controls
                            Console.WriteLine("Camera Up");
                        }
                        if (posY >= lowerBoundary)
                        {
                            // insert camera controls
                            Console.WriteLine("Camera Down");
                        }
                    }
                }

                // Display live feed 
                //imageBoxFrameGrabber.Image = currentFrame;
            }
            
        }
    }
}