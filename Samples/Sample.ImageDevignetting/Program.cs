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
using System;
using DotDevignetting;

namespace Sample.DevignetteImage
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var image = ImageIO.LoadColor("../Resources/pair.jpg").Clone(); /*../Resources/nature.jpg*/
            image.Show("Original", scaleForm: true);

            var tic = DateTime.Now.Ticks;
            {
                image.Devignette(optimizeVignettingCentre: true);
                image.Show("Corrected", scaleForm: true);
            }
            var toc = DateTime.Now.Ticks;
            Console.WriteLine("Elapsed: {0} ms.", (toc - tic) / TimeSpan.TicksPerMillisecond);
        }
    }
}
