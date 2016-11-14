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

namespace DotDevignetting
{
    /// <summary>
    /// Optimization function interface.
    /// </summary>
    /// <typeparam name="TData">data type.</typeparam>
    public interface IFunction<TData>
    {
        /// <summary>
        /// Checks whether the function parameters are valid.
        /// </summary>
        /// <returns>True if the parameters are valid, false otherwise.</returns>
        bool IsValid();

        /// <summary>
        /// Evaluates the function using the provided data.
        /// </summary>
        /// <param name="data">Data.</param>
        /// <returns>Error value (if the minimization is used).</returns>
        float Evaluate(TData data);
    }

    /// <summary>
    /// Array indexer for the optimization function.
    /// </summary>
    public interface IParameterIndexer
    {
        /// <summary>
        /// Gets or sets the function parameter.
        /// </summary>
        /// <param name="paramIndex">parameter index. [0.. <see cref="ParameterCount"/> - 1]</param>
        /// <returns>Parameter value.</returns>
        float this[int paramIndex] { get; set; }

        /// <summary>
        /// Gets the parameter count.
        /// </summary>
        int ParameterCount { get; }
    }
}
