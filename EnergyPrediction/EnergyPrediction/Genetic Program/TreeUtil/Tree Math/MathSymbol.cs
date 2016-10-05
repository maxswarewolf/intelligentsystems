﻿//
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
                    return aLeftValue * Math.Sin(aRightValue);
                case SymbolTypes.Cos:
                    return aLeftValue * Math.Cos(aRightValue);
                case SymbolTypes.Log:
                    return aLeftValue * Math.Log10(aRightValue);
                case SymbolTypes.Exp:
                    return aLeftValue * Math.Exp(aRightValue);
                case SymbolTypes.Pow:
                    return Math.Pow(aLeftValue, aRightValue);
                case SymbolTypes.Sqrt:
                    return aLeftValue * Math.Sqrt(aRightValue);
                case SymbolTypes.Sinh:
                    return aLeftValue * Math.Sinh(aRightValue);
                case SymbolTypes.Cosh:
                    return aLeftValue * Math.Cosh(aRightValue);
                default:
                    return 0;
            }
        }

        public override void ChangeValue()
        {
            var r = Rand.Next(0, Enum.GetNames(typeof(SymbolTypes)).Length);
            Symbol = (SymbolTypes)(Enum.GetValues(typeof(SymbolTypes)).GetValue(r));
        }

        public override string ToString()
        {
            switch (Symbol)
            {
                case SymbolTypes.Add:
                    return " + ";
                case SymbolTypes.Multiply:
                    return " * ";
                case SymbolTypes.Devide:
                    return " / ";
                case SymbolTypes.Sin:
                    return " * Sin( ";
                case SymbolTypes.Cos:
                    return " * Cos( ";
                case SymbolTypes.Log:
                    return " * Log( ";
                case SymbolTypes.Exp:
                    return " * Exp( ";
                case SymbolTypes.Pow:
                    return "^";
                case SymbolTypes.Sqrt:
                    return " * Sqrt( ";
                case SymbolTypes.Sinh:
                    return " * Sinh( ";
                case SymbolTypes.Cosh:
                    return " * Cosh( ";
                default:
                    return "";
            }
        }
    }
}

