using System;


namespace TFilesEvenSLEbin
{
    class Odom
    {
        public int TupleSize;
        private int n;//alias for Tuplesize
        public byte[] tuple;//ordinary tuple.
		private byte[] beginTuple;
		private byte[] endTuple;
		private bool started;
		public bool isAlive;
		private int matchedAtIndex;
        
        byte[] BASE;


        public Odom(int TupleSize = 2,byte constantbase=6)
        {
            this.TupleSize = TupleSize;
            n = TupleSize;
            BASE = new byte[n];
            for (int k = 1; k < n; k++) BASE[k] = constantbase; BASE[0] = constantbase;//base=66...6

            tuple = new byte[n];//tuple=d0d1d2...d(n-1) where d stands for digit
            Array.Clear(tuple, 0, n);//tuple=000...0
			started = false;
			isAlive = true;
        }

		public Odom(string begin="00",string end="22", byte constantbase=6) {
			TupleSize = begin.Length;
			n = TupleSize; if (n != end.Length) Console.WriteLine("Error in OdometerSimple: begin and end are of different lengths");
			char[] cbeginTuple = begin.ToCharArray();
			beginTuple = new byte[n];
			for (int i = 0; i < n; i++) {
				beginTuple[i] = (byte)char.GetNumericValue(cbeginTuple[i]);
			}

			char[] cendTuple = end.ToCharArray();
			endTuple = new byte[n];
			for (int i = 0; i < n; i++) {
				endTuple[i] = (byte)(char.GetNumericValue(cendTuple[i]));
			}

			tuple = new byte[n];//tuple=d0d1d2...d(n-1) where d stands for digit
			Array.Copy(beginTuple,tuple,n);//tuple=000...0

			BASE = new byte[n];
			for (int k = 1; k < n; k++) BASE[k] = constantbase; BASE[0] = constantbase;//base=66...6

			started = false;
			isAlive = true;
			matchedAtIndex = 0;
		}

		public bool Read()//0<= j < total
	  {
			if (started == false) { started = true; return true; }//tuple and newtuple were already initialized in the constructor
			if (isAlive == false) { return false; }//Odometer has gone through all tuples.
			byte carry;
			int currentdigit = n - 1;//Currentdigit points to the last entry of the tuple.
			carry = 0;//Sum using carry. i.e. 1+1=0 with carry 1 in base 2

			++tuple[currentdigit];
			if (tuple[currentdigit] == BASE[currentdigit]) {
				carry = 1;
				while (carry == 1 & currentdigit > 0) {
					carry = 0;
					tuple[currentdigit] = 0;// current entry is set to 0
					++tuple[--currentdigit];//next entry is incremented by 1
					if (tuple[currentdigit] == BASE[currentdigit]) carry = 1;
				}
				currentdigit = n - 1;//Currentdigit points to the last entry of the tuple.

			}
			while (matchedAtIndex<n && tuple[matchedAtIndex] == endTuple[matchedAtIndex]) ++matchedAtIndex;
			if(matchedAtIndex==n) { isAlive = false; return true; }
			return true;
		}



        public void Next()//0<= j < total
        {
			if (started == false) { started = true; return; }//tuple and newtuple were already initialized in the constructor
			if (isAlive == false) { return; }//Odometer has gone through all tuples.
            byte carry;
            int currentdigit = n - 1;//Currentdigit points to the last entry of the tuple.
            carry = 0;//Sum using carry. i.e. 1+1=0 with carry 1 in base 2

            ++tuple[currentdigit];
            if (tuple[currentdigit] == BASE[currentdigit])
            {
                carry = 1;
                while (carry == 1 & currentdigit > 0)
                {
                    carry = 0;
                    tuple[currentdigit] = 0;//current entry is set to 0
                    ++tuple[--currentdigit];//next entry is incremented by 1
                    if (tuple[currentdigit] == BASE[currentdigit]) carry = 1;
                }
                currentdigit = n - 1;//Currentdigit points to the last entry of the tuple.

            }
			if (tuple[0] == BASE[0]) { isAlive = false; }

        }

    }
}