<p align="center">
    <a href="https://www.nuget.org/profiles/dajuric"> <img src="Deployment/Logo/logo-big.png" alt="DotDevignetting logo" width="120" align="center"/> </a>
</p>

<p align="center">
    <a href="https://www.nuget.org/packages/DotDevignetting/">
       <img src="https://img.shields.io/badge/DotDevignetting-1.0.1-A71D89.svg?style=flat-square" alt="DotDevignetting"/> 
    </a>
</p>

**DotDevignetting** - Fast automatic image and video devignetting (lens shading correction).  
 Use it for standalone image correction or real-time video correction (e.g. live-feed from camera).

### Samples

<p align="center">
   <a href="./Deployment/sample-1.jpg" target="_blank">
      <img alt="Output sample" src="./Deployment/sample-1.jpg" width="250"/>
   </a>
   &nbsp; &nbsp;
   <a href="./Deployment/sample-2.jpg" target="_blank">
      <img alt="Output sample" src="./Deployment/sample-2.jpg" width="250"/>
   </a>
</p>

### Code

+ Image devignetting:
``` csharp
  var image = ImageIO.LoadColor("(your image)").Clone();
  image.Devignette(optimizeVignettingCentre: true);
  image.Show("Corrected", scaleForm: true);
```

+ Video devignetting:
``` csharp
  //initialize capture and buffer
  VideoCaptureBase capture = new FileCapture("(your video)"); // /*or*/ capture = new CameraCapture();
  Bgr<byte>[,] frame = null;

  //initialize devignetting alg
  capture.ReadTo(ref frame);
  var devignetting = new Devignetting(frame, optimizeVignettingCentre: true);

  //do the job
  while(true)
  {
      capture.ReadTo(ref frame);
      if (frame == null) break;
        
      devignetting.DevignetteSingleStep(frame);
      frame.Show("Corrected");
  }

  capture.Close();
```

## Getting started
+ Readme file - shown upon installation of the NuGet package. 
+ Samples

## How to Engage, Contribute and Provide Feedback  
Remember: Your opinion is important and will define the future roadmap.
+ questions, comments - message on Github, or write to: darko.juric2 [at] gmail.com
+ **spread the word** 

## Final word
If you like the project please **star it** in order to help to spread the word. That way you will make the framework more significant and in the same time you will motivate me to improve it, so the benefit is mutual.
