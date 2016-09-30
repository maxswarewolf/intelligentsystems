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

        // TODO: Adrian, have fun :-) 

        //Todo: if this visitor pattern also fixes the tree, please rename to "makeValidSubTree"!



        /*
         * THis is what it's supposed to do
         * 
         *  internal Boolean isValidSubTree()
        {
            //validation task 1: Check if it contains x!

            if (this.Data.isThisX())
            {
                return true;
            }
            else if (ChildLeft == null && ChildRight == null)
            {
                return false;
            }
            else if (ChildLeft == null)
            {
                return ChildRight.isValidSubTree();
            }
            else if (ChildRight == null)
            {
                return ChildLeft.isValidSubTree(); 
            }

            return (ChildLeft.isValidSubTree() || ChildRight.isValidSubTree());


            //todo: are there any additional validation steps required
            // todo: double-check if depth is still correct!

        */
        public static bool visit(TreeNode<MathObject> root)
        {
            if (root.GetType().Equals(typeof(MathSymbol)))
            {
                if (visit(root.ChildLeft))
                {
                    return true;
                }
                else if (visit(root.ChildRight))
                {
                    return true;
                }
                return false;
            }
            else if (root.Data.isX)
            {
                return true;
            }
            return false;
        }
    }
}


