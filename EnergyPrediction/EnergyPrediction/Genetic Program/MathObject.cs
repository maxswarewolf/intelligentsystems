//
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
    /// Implements Math operation add (addition) 
    /// result = a*x + b
    /// </summary>
    public class MathOpAdd : MathObject 
    {
        

        public override double doCalc(double leftVal, double rightVal, int x)
        {
            return (leftVal* x)+ rightVal;
        }
    }

    /// <summary>
    /// Implements Math operation sub (subtraction) 
    /// result = (a*x) -b 
    /// </summary>
    public class MathOpSub : MathObject
    {
        public override double doCalc(double leftVal, double rightVal, int x)
        {
            return (leftVal*x) - rightVal; 
        }
    }
   }