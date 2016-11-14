#region Licence and Terms
// DotDevignetting library
// https://github.com/dajuric/dot-devignetting
//
// Copyright © Darko Jurić, 2016-2016 
// darko.juric2@gmail.com
//
//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU Lesser General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.
//
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU Lesser General Public License for more details.
// 
//   You should have received a copy of the GNU Lesser General Public License
//   along with this program.  If not, see <https://www.gnu.org/licenses/lgpl.txt>.
//
#endregion

using DotDevignetting;
using DotImaging;
using DotImaging.Primitives2D;
using System;

namespace Sample.DevignetteVideo
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //initialize capture and buffer
            VideoCaptureBase capture = new FileCapture("../Resources/video-vignette.mp4"); // /*or*/ capture = new CameraCapture();
            Bgr<byte>[,] frame = null;

            //initialize devignetting alg
            capture.ReadTo(ref frame);
            var devignetting = new Devignetting(frame, optimizeVignettingCentre: true);
            Bgr<byte>[,] correctedIm = frame.CopyBlank();

            //do the job
            do
            {
                capture.ReadTo(ref frame);
                if (frame == null) break;
                frame.Show("Original");

                frame.CopyTo(correctedIm, Point.Empty);
                bool isDone = devignetting.DevignetteSingleStep(correctedIm);
                correctedIm.Show("Corrected");

                Console.WriteLine("Frame: {0:000}, is done: {1}", capture.Position, isDone);
            }
            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape));

            capture.Close();
        }
    }
}
