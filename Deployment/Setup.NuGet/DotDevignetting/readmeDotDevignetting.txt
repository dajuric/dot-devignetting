Fast automatic image and video de-vignetting.

1) image devignetting (**requires DotImaging.IO, DoTimaging.UI**)

   var image = ImageIO.LoadColor("<your imge>").Clone();
   image.Devignette(optimizeVignettingCentre: true);
   image.Show("Corrected", scaleForm: true);


2) video devignetting (**requires DotImaging.IO, DoTimaging.UI**)
  
   //initialize capture and buffer
   VideoCaptureBase capture = new FileCapture("<your video>"); // /*or*/ capture = new CameraCapture();
   Bgr<byte>[,] frame = null;

   //initialize devignetting alg
   capture.ReadTo(ref frame);
   var devignetting = new Devignetting(frame, optimizeVignettingCentre: true);

   //do the job
   while(true)
   {
       capture.ReadTo(ref frame);
       if (frame == null) break;
        
	   devignetting.DevignetteSingleStep(frame); //will perform faster once the optimization procedure is done
       frame.Show("Corrected");
   }

   capture.Close();
