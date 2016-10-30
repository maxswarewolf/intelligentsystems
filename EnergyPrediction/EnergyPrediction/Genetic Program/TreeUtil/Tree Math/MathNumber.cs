//
// MIT LICENSE
//
// MathNumber.cs
//
// Author:
//       Katie Clark, Sean Grinter, Adrian Pellegrino <Energy Prediction>
//
// Copyright (c) 2016 Katie Clark, Sean Grinter, Adrian Pellegrino
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
namespace EnergyPrediction
{
    public class MathNumber : MathObject
    {
        public double Value { get; private set; }
        public MathNumber()
        {
            this.ChangeValue();
        }
        public MathNumber(int value)
        {
            this.Value = value;
            this.isX = Equals(Value, 0.0);
        }
        /// <summary>
        /// Will return the value of the number, but this should not be used.
        /// Call object.Value instead
        /// </summary>
        /// <returns>Value.</returns>
        public override double doCalculation(int x)
        {
            return isX ? x : Value;
        }
        public override double doCalculation(double aLeftValue, double aRightValue)
        {
            throw new NotImplementedException();
        }

        public override void ChangeValue()
        {
            Value = Randomizer.NextDouble(MathObject.RangePeek * -1, MathObject.RangePeek + 1);
            Value = Math.Round(Value, 4);
            base.isX = Equals(Value, 0.0);
        }

        public override string ToString()
        {
            return (isX) ? "x" : "" + Value;
        }
    }
}

