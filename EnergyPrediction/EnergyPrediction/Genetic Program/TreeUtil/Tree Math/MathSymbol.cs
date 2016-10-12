//
// MIT LICENSE
//
// MathSymbol.cs
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
    public class MathSymbol : MathObject
    {
        public SymbolTypes Symbol { get; private set; }
        public MathSymbol()
        {
            this.ChangeValue();
        }
        //todo: add in other operands
        public override double doCalculation(double aLeftValue, double aRightValue)
        {
            switch (Symbol)
            {
                case SymbolTypes.Add:
                    return aLeftValue + aRightValue;
                case SymbolTypes.Multiply:
                    return aLeftValue * aRightValue;
                case SymbolTypes.Devide:
                    return aLeftValue / aRightValue;
                case SymbolTypes.Sin:
                    return Math.Sin(aLeftValue * aRightValue);
                case SymbolTypes.Cos:
                    return Math.Cos(aLeftValue * aRightValue);
                case SymbolTypes.Log:
                    return Math.Log10(aLeftValue * aRightValue);
                case SymbolTypes.Exp:
                    return Math.Exp(aLeftValue * aRightValue);
                case SymbolTypes.Pow:
                    return Math.Pow(aLeftValue, aRightValue);
                case SymbolTypes.Sqrt:
                    return Math.Sqrt(aLeftValue * aRightValue);
                case SymbolTypes.Sinh:
                    return Math.Sinh(aLeftValue * aRightValue);
                case SymbolTypes.Cosh:
                    return Math.Cosh(aLeftValue * aRightValue);
                default:
                    return 0;
            }
        }

        public override void ChangeValue()
        {
            var r = Rand.Next(0, Enum.GetNames(typeof(SymbolTypes)).Length);
            Symbol = (SymbolTypes)(Enum.GetValues(typeof(SymbolTypes)).GetValue(r));
        }

        public string ToString(string aLeftValue, string aRightValue)
        {
            switch (Symbol)
            {
                case SymbolTypes.Add:
                    return "(" + aLeftValue + " + " + aRightValue + ")";
                case SymbolTypes.Multiply:
                    return "(" + aLeftValue + " * " + aRightValue + ")";
                case SymbolTypes.Devide:
                    return "(" + aLeftValue + " / " + aRightValue + ")";
                case SymbolTypes.Sin:
                    return "Sin(" + aLeftValue + " * " + aRightValue + ")";
                case SymbolTypes.Cos:
                    return "Cos(" + aLeftValue + " * " + aRightValue + ")";
                case SymbolTypes.Log:
                    return "Log(" + aLeftValue + " * " + aRightValue + ")";
                case SymbolTypes.Exp:
                    return "Exp(" + aLeftValue + " * " + aRightValue + ")";
                case SymbolTypes.Pow:
                    return "(" + aLeftValue + "^" + aRightValue + ")";
                case SymbolTypes.Sqrt:
                    return "Sqrt(" + aLeftValue + " * " + aRightValue + ")";
                case SymbolTypes.Sinh:
                    return "Sinh(" + aLeftValue + " * " + aRightValue + ")";
                case SymbolTypes.Cosh:
                    return "Cosh(" + aLeftValue + " * " + aRightValue + ")";
                default:
                    return "";
            }
        }
    }
}

