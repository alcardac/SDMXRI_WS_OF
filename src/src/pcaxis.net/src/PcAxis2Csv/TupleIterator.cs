 /*
 * Copyright (c) 2005, Regents of the University of California
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 *
 * * Redistributions of source code must retain the above copyright
 *   notice, this list of conditions and the following disclaimer.
 *
 * * Redistributions in binary form must reproduce the above copyright
 *   notice, this list of conditions and the following disclaimer in
 *   the documentation and/or other materials provided with the
 *   distribution.  
 *
 * * Neither the name of the University of California, Berkeley nor
 *   the names of its contributors may be used to endorse or promote
 *   products derived from this software without specific prior 
 *   written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
 * FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE
 * COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
 * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
 * STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
 * OF THE POSSIBILITY OF SUCH DAMAGE.
 */


using System;
using System.Collections.Generic;
 using System.Collections;
using System.Text;

namespace org.estat.PcAxis
{

    /** This class is an iterator over all tuples in a finite cartesian product 
     * of finite sets.  Some special cases: if the number of sets passed in is 
     * zero, then the Cartesian product contains one element, the empty tuple.  
     * If an empty set is passed in, then the Cartesian product is empty.  
     */
    public class TupleIterator : IEnumerator
    {
        int position = -1;

        
        public TupleIterator(ArrayList cartproduct)
        {
            this.cartproduct = cartproduct;
            

            for (int i = 0; i < cartproduct.Count; i++)
            {
                IEnumerator curIter = ((IEnumerable)cartproduct[i]).GetEnumerator();
                currstate.Add(curIter);
                if (curIter.MoveNext())
                {
                    nextelt.Add(curIter.Current);
                }
                else
                {
                    nextelt = null;
                    break;
                }
            }
        }


        private ArrayList result;
       
        /** A List of Collections. */
        private ArrayList cartproduct;

        /** A List of iterators storing the tuple-iterator's state. */
        private ArrayList currstate = new ArrayList();

        private ArrayList nextelt = new ArrayList();

        #region IEnumerator Members

        

        /** Returns another tuple not returned previously.
         *
         * <p>The iterator stores its state in the private member 
         * <code>currstate</code> -- an ArrayList of the iterators of the 
         * individual Collections in the cartesian product. In the start state, 
         * each iterator returns a single element. Afterwards, while iterator #1 
         * has anything to return, we replace the first element of the previous 
         * tuple to obtain a new tuple. Once iterator #1 runs out of elements we 
         * replace it and advance iterator #2. We keep on advancing iterator #1 
         * until it runs out of elements for the second time, reinitialize it 
         * again, and advance iterator #2 once more. We repeat these operations 
         * until iterator #2 runs out of elements and we start advancing 
         * iterator #3, and so on, until all iterators run out of elements.
         *
         * <p> This method of generating the m-tuples is very similar to producing 
         * all numbers of m "digits" in the order of their magnitude, where the 
         * i-th "digit" is in base<sub>i</sub> = #(i-th Collection in the cartesian
         *  product) and we assume that the "numbers" in the i-th Collection are 
         * ordered in some way.
         */
        public object Current
        {
            get 
            {
                return result;
            }
        }
        /** 
         * Returns true if there are any more elements in the Cartesian product 
         * to return.  
         */
        public bool MoveNext()
        {
            if (nextelt == null)
            {
                return false;
            }

            // It's important that we return a new list, not the list that we'll 
            // be modifying to create the next tuple (namely nextelt).  The 
            // caller might be storing some of these tuples in a collection, 
            // which would yield unexpected results if we just gave them the 
            // same List object over and over.  
            result = new ArrayList(nextelt);

            // compute the next element
            bool gotNext = false;
            for (int i = currstate.Count - 1; i >= 0; --i)
            {
                IEnumerator curIter = (IEnumerator)currstate[i];
                if (curIter.MoveNext())
                {
                    // advance this iterator, we have next tuple
                    nextelt[i] = curIter.Current;
                    gotNext = true;
                    break;
                }
                else
                {
                    // reset this iterator to its beginning, continue loop
                    curIter = ((IEnumerable)cartproduct[i]).GetEnumerator();
                    currstate[i] = curIter;
                    curIter.MoveNext();
                    nextelt[i] = curIter.Current;
                }
            }
            if (!gotNext)
            {
                nextelt = null;
            }

            return true;
        }

        public void Reset()
        {
            // TODO
            position = -1;
        }

        #endregion
    }

}

