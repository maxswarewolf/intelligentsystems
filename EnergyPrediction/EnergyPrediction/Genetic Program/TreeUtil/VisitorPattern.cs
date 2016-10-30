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
namespace EnergyPrediction
{
    public static class VisitorPattern
    {
        public static int NumX(TreeNode<MathObject> root)
        {
            if (root.ChildLeft != null)
            {
                return NumX(root.ChildLeft) + NumX(root.ChildRight);
            }
            else {
                return (root.Data.isX) ? 1 : 0;
            }
        }

        public static int addXPostOrder(TreeNode<MathObject> root)
        {
            int counter = 0;
            if (root != null)
            {
                counter += addXPostOrder(root.ChildLeft);
                counter += addXPostOrder(root.ChildRight);
                if (root.Data.GetType().Equals(typeof(MathNumber)))
                {
                    if (Randomizer.NextDouble(0, 1) > 0.80)
                    {
                        root.Data = new MathNumber(0);
                        counter++;
                    }
                }
            }
            return counter;
        }


        public static int confirmNumXInOrder(TreeNode<MathObject> root, int aXThreshold, bool aLeftFirst)
        {
            if (aXThreshold >= 1)
            {
                int currentXThresh = aXThreshold;
                if (root.Data.GetType().Equals(typeof(MathNumber)))
                {
                    if (!root.Data.isX)
                    {
                        root.Data = new MathNumber(0);
                    }
                    return aXThreshold - 1;
                }
                else {
                    if (aLeftFirst)
                    {
                        currentXThresh = confirmNumXInOrder(root.ChildLeft, currentXThresh, false);
                        currentXThresh = confirmNumXInOrder(root.ChildRight, currentXThresh, false);
                    }
                    else {
                        currentXThresh = confirmNumXInOrder(root.ChildRight, currentXThresh, true);
                        currentXThresh = confirmNumXInOrder(root.ChildLeft, currentXThresh, true);
                    }
                }
                return currentXThresh;
            }
            return aXThreshold;
        }

        public static double Calculate(TreeNode<MathObject> root, int x)
        {
            if (root.Data.GetType().Equals(typeof(MathNumber)))
            {
                return root.Data.doCalculation(x);
            }
            else {
                return root.Data.doCalculation(Calculate(root.ChildLeft, x), Calculate(root.ChildRight, x));
            }
        }

        public static string RootToString(TreeNode<MathObject> root)
        {
            if (root.ChildLeft == null)
            {
                MathNumber temp = root.Data as MathNumber;
                return temp.ToString();
            }
            else {
                MathSymbol temp = root.Data as MathSymbol;
                return temp.ToString(RootToString(root.ChildLeft), RootToString(root.ChildRight));
            }
        }
    }
}


