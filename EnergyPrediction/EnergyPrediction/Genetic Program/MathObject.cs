﻿//
// MIT LICENSE
//
// EmptyClass.cs
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
using System.Collections.Generic;
using GeneticSharp.Domain.Randomizations;

namespace EnergyPrediction
{
    
    public class MathObject
    {
        public static int totalNrMathObjects { get; private set; } = 3;
        public static int unbranchableNrMathObjects { get; private set; } = 2; // mathX= case0, mathNr = case1
        MathObject type;
        double value;

        public MathObject()
        {
            type = getRandomOperandType();
        }

        public MathObject getMathObjectType()
        {
            return type;
        }

        public virtual double getNumberValue() // todo: does override work correctly? 
        {
            throw new Exception("Object is not a MathObject");
        }

        public Boolean equals(MathObject m)
        {
            if (type == m) return true;
            else return false;
        }

        MathObject getRandomOperandType()
        {
            //todo: get some sort of collection of implemented operand, select one at random
            throw new NotImplementedException();
        }

        public virtual double doCalc(double leftVal, double rightVal, int x)
        {
            return 0;  // should not be reached
        }

   }

    //case 0
    public class MathX : MathObject
    { 
        public override double doCalc(double leftVal, double rightVal, int x)
        {// leftVal and rightVal will be null!
            return x; 
        }
    }

    //case 1
    public class MathNumber : MathObject {
        double value;

        public MathNumber(int gRangePeek)
        {
            value = RandomizationProvider.Current.GetInt(gRangePeek * -1, gRangePeek + 1);
        }
        public override double getNumberValue()
        { return value; 
        }

    }

    /// <summary>
    /// CASE 2
    /// Implements Math operation add (addition) 
    /// result = a + b
    /// </summary>
    public class MathOpAdd : MathObject 
    {
        public override double doCalc(double leftVal, double rightVal, int x)
        {
            return (leftVal)+ rightVal;
        }
    }

    /// <summary>
    /// CASE 3
    /// Implements Math operation sub (subtraction) #### change, not needed 
    /// result = a * b 
    /// </summary>
    public class MathOpMult : MathObject
    {
        public override double doCalc(double leftVal, double rightVal, int x)
        {
            return (leftVal) * rightVal; 
        }
    }

    // todo: every new math object implementation must be included in the switch statement in the chromosone creation
   }