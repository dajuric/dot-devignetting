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

using DotImaging;

namespace DotDevignetting
{
    //See http://citeseerx.ist.psu.edu/viewdoc/download?doi=10.1.1.258.4780&rep=rep1&type=pdf for details.
    /// <summary>
    /// De-vignetting algorithm.
    /// <para>The implemented version is an enhanced version of the "Torsten Rohfling: Single-Image Vignetting Correction by Constrained Minimization of log-Intensity Entropy".
    /// The enhancements employ incremental image correction - suitable for real-time video correction and vignetting centre estimation.</para>
    /// </summary>
    public class Devignetting
    {
        HillClimbingOptimization<DevignettingFunction, Bgr<byte>[,]> optimizationAlg = null;

        /// <summary>
        /// Creates and initializes new de-vignetting algorithm.
        /// </summary>
        /// <param name="image">Original image.</param>
        /// <param name="optimizeVignettingCentre">
        /// True to optimize spatial vignetting position, false otherwise.
        /// <para>If set to false, the algorithm will perform significantly less number of steps.</para>
        /// </param>
        public Devignetting(Bgr<byte>[,] image, bool optimizeVignettingCentre = true)
        {
            var initialStep = new float[] {5, 5, 5, image.Width() / 4, image.Height() / 4 };
            var stepReduction = new float[] { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f };
            var finalStep = new float[] { 1f / 256, 1f / 256, 1f / 256, 1, 1 };

            var endParamIdx = DevignettingFunction.PARAMETER_COUNT - 1;
            if (!optimizeVignettingCentre) endParamIdx -= 2; /*dX, dY*/

            optimizationAlg = new HillClimbingOptimization<DevignettingFunction, Bgr<byte>[,]>(initialStep, stepReduction, finalStep,
                                                                                               0, endParamIdx);

            optimizationAlg.Initialize(DevignettingFunction.Empty, image);
        }

        /// <summary>
        /// Performs single de-vignetting step on the specified image.
        /// The image is modified and corrected iteratively.
        /// <para>Once estimated (return value will become true), the function can be applied again on the whole video in order to correct it.</para>
        /// </summary>
        /// <param name="image">Image to correct.</param>
        /// <returns>True if the algorithm has finished, false otherwise.</returns>
        public bool DevignetteSingleStep(Bgr<byte>[,] image)
        {
            optimizationAlg.MinimizeSingleStep(image);
            optimizationAlg.Function.Apply(image);

            return optimizationAlg.IsDone;
        }

        /// <summary>
        /// Performs de-vignetting onto the specified image.
        /// </summary>
        /// <param name="image">Image to correct.</param>
        public void Devignette(Bgr<byte>[,] image)
        {
            while (!optimizationAlg.IsDone)
            {
                optimizationAlg.MinimizeSingleStep(image);
            }

            optimizationAlg.Function.Apply(image);
        }
    }

    /// <summary>
    /// De-vignetting algorithm extensions.
    /// </summary>
    public static class DevignettingExtensions
    {
        /// <summary>
        /// Performs de-vignetting onto the specified image.
        /// </summary>
        /// <param name="image">Image to correct.</param>
        /// <param name="optimizeVignettingCentre">
        /// True to optimize spatial vignetting position, false otherwise.
        /// <para>If set to false, the algorithm will perform much faster.</para></param>
        public static void Devignette(this Bgr<byte>[,] image, bool optimizeVignettingCentre = true)
        {
            var devignetting = new Devignetting(image, optimizeVignettingCentre);
            devignetting.Devignette(image);
        }
    }
}
