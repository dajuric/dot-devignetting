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

using System;

namespace DotDevignetting
{
    /// <summary>
    /// Hill-climbing algorithm.
    /// </summary>
    /// <typeparam name="TFunction">Optimization function type.</typeparam>
    /// <typeparam name="TData">Data type for the provider function.</typeparam>
    public class HillClimbingOptimization<TFunction, TData>
        where TFunction: IFunction<TData>, IParameterIndexer
    {
        float[] step;
        int prevIterId;
        int delta;
        float bestValue;

        int updateIndex;
        float updateParam;
        int maxParamIndex;
        int currentParamIndex;

        bool isImproved;

        /// <summary>
        /// Creates new hill-climbing algorithm.
        /// </summary>
        /// <param name="initialStep">Initial step for each parameter.</param>
        /// <param name="stepReduction">Step reduction for each parameter.</param>
        /// <param name="finalStep">Final step for each parameter.</param>
        /// <param name="startParamIdx">Starting index of an parameter to optimize.</param>
        /// <param name="endParamIdx">Ending index of an parameter to optimize.</param>
        public HillClimbingOptimization(float[] initialStep, float[] stepReduction, float[] finalStep,
                                        int startParamIdx, int endParamIdx)
        {
            InitialStep = initialStep;
            StepReduction = stepReduction;
            FinalStep = finalStep;

            StartParamIdx = startParamIdx;
            EndParamIdx = endParamIdx;
        }

        /// <summary>
        /// Initializes the optimization algorithm by performing initial function evaluation to set the initial state.
        /// </summary>
        public void Initialize(TFunction initialFunction, TData data)
        {
            Function = initialFunction;

            step = (float[])InitialStep.Clone();
            prevIterId = -1;
            delta = -1;
            bestValue = initialFunction.Evaluate(data);

            updateIndex = -1;
            updateParam = Single.PositiveInfinity;
            maxParamIndex = StartParamIdx;
            currentParamIndex = StartParamIdx;

            isImproved = false;
            IsDone = false;
        }

        private bool minimizeSingleStepDeltaOnly1D(TData data)
        {
            //----- for delta = [-1, +1] -----
            if (delta <= 1)
            {
                //----- core -----
                float oldP = Function[currentParamIndex];
                Function[currentParamIndex] += step[currentParamIndex] * delta;

                int thisIterId = (currentParamIndex + 1) * delta;
                bool isInBounds = Function.IsValid();

                if (prevIterId != thisIterId && isInBounds)
                {
                    float value = Function.Evaluate(data);

                    if (value < bestValue)
                    {
                        isImproved = true;
                        prevIterId = thisIterId;
                        bestValue = value;

                        updateIndex = currentParamIndex;
                        updateParam = Function[currentParamIndex];
                    }
                }

                Function[currentParamIndex] = oldP;
                //----- core -----

                delta += 2; //(-1, +1, -1, +1...)

                if (delta <= 1)
                    return false;
                else
                    delta = -1;
            }
            //----- for delta = [-1, +1] -----

            return true;
        }

        /// <summary>
        /// Performs single iteration of minimization using the given data.
        /// </summary>
        /// <param name="data">Data to be passed to the evaluating function.</param>
        /// <returns>
        /// True if the optimization has finished, false otherwise.
        /// <para>See <see cref="IsDone"/>.</para>
        /// </returns>
        public bool MinimizeSingleStep(TData data)
        {
            //incremental optimization by increasing polynomial degree
            //----- for maxParamIdx = 1: length(initialFuncParams) -----
            if (maxParamIndex <= EndParamIdx)
            {
                //----- while step >= finalStep -----
                if (step[currentParamIndex] >= FinalStep[currentParamIndex])
                {
                    //repeat hill - climbing procedure if we improving the result
                    //	----- for i = 1: maxParamIdx -----
                    if (currentParamIndex <= maxParamIndex)
                    {
                        if (!minimizeSingleStepDeltaOnly1D(data)) return false;
                        currentParamIndex++;

                        if (currentParamIndex <= maxParamIndex)
                            return false;
                        else
                        {
                            currentParamIndex = StartParamIdx;
                            prevIterId = 0;
                        }
                    }
                    //----- for i = 1: maxParamIdx -----

                    if (isImproved) //after all parameters are tried out
                    {
                        Function[updateIndex] = updateParam;
                        isImproved = false;
                        return false;
                    }
                    //----- while isImproved -----

                    step[currentParamIndex] *= StepReduction[currentParamIndex];

                    if (step[currentParamIndex] >= FinalStep[currentParamIndex])
                        return false;
                    else
                        step[currentParamIndex] = InitialStep[currentParamIndex];
                }
                //----- while step >= finalStep -----

                maxParamIndex++;

                if (maxParamIndex < Function.ParameterCount)
                    return false; //do not return counter back to 1
            }

            IsDone = true;
            return true;
        }

        /// <summary>
        /// Gets the initial step size.
        /// </summary>
        public float[] InitialStep { get; private set; }
        /// <summary>
        /// Gets the step reduction factor.
        /// </summary>
        public float[] StepReduction { get; private set; }
        /// <summary>
        /// Gets the final step size.
        /// </summary>
        public float[] FinalStep { get; private set; }
        /// <summary>
        /// Gets the index for the starting optimizing parameter.
        /// </summary>
        public int StartParamIdx { get; private set; }
        /// <summary>
        /// Gets the index for the last parameter to optimize.
        /// </summary>
        public int EndParamIdx { get; private set; }

        /// <summary>
        /// Returns true if the optimization is finished, false otherwise.
        /// </summary>
        public bool IsDone { get; private set; }

        /// <summary>
        /// Gets the current function state.
        /// </summary>
        public TFunction Function { get; private set; }
    }
}
