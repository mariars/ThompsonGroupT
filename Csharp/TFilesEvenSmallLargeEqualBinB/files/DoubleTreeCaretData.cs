using System;
using System.Collections.Generic;



namespace TFilesEvenSLEbin
{
	class DoubleTreeCaretData
	{
		public	LinkedList<int> xlist_range ;
		public	LinkedList<int> ylist_range ;
		public	LinkedList<int> xlist_domain;
		public	LinkedList<int> ylist_domain;
		public LinkedList<int> sigmaRangeLabelsToDomainLabelsList;//0 leaf of range (= 0 labeled leaf of range) maps to the 0 labeled leaf of domain (not necessarily the 0 leaf of domain).
		private LinkedListNode<int> xnode_range;
		private LinkedListNode<int> ynode_range;
		private LinkedListNode<int> xnode_domain;
		private LinkedListNode<int> ynode_domain;
		public int numberOfLeaves;
		public int marker_range;//pointer of the range forest (i.e. where the interval [1/2,?]  occurs)
		public int marker_domain;//pointer of the domain forest (i.e. where the interval [1/2,?]  occurs)



		public DoubleTreeCaretData(DoubleTree doubleTree) {
			
			 xlist_range = new LinkedList<int>();
			ylist_range = new LinkedList<int>();
			 xlist_domain = new LinkedList<int>();
			 ylist_domain = new LinkedList<int>();
			 sigmaRangeLabelsToDomainLabelsList = new LinkedList<int>();
			 int[] markersandnumberofleaves = DoubleTreeFunctions.DoubleTreeCarets(doubleTree, ref xlist_range, ref ylist_range, ref xlist_domain, ref ylist_domain, ref sigmaRangeLabelsToDomainLabelsList);
			 marker_range = markersandnumberofleaves[0];
			 marker_domain = markersandnumberofleaves[1];
			 numberOfLeaves = markersandnumberofleaves[2];
			 First();

		}

		public void First(){
			 xnode_range = xlist_range.First;
			 ynode_range = ylist_range.First;
			 xnode_domain = xlist_domain.First;
			 ynode_domain = ylist_domain.First;
		}

		public bool Next(ref int x_range, ref int y_range, ref int x_domain, ref int y_domain) {
			if (xnode_range != null) {
				x_range = xnode_range.Value;
				y_range = ynode_range.Value;
				x_domain = xnode_domain.Value;
				y_domain = ynode_domain.Value;


				xnode_range = xnode_range.Next;
				ynode_range = ynode_range.Next;
				xnode_domain = xnode_domain.Next;
				ynode_domain = ynode_domain.Next;

				return true;
			}
			return false;
		}




		public string CaretPoints() {
			string str;
			str="range: ";
			LinkedListNode<int> xnode = xlist_range.First;
			LinkedListNode<int> ynode = ylist_range.First;
			while (xnode != null) {
				str += "(" + xnode.Value + "," + ynode.Value + ") ";
				xnode = xnode.Next;
				ynode = ynode.Next;
			}
			str += Environment.NewLine + "domain: ";
			xnode = xlist_domain.First;
			ynode = ylist_domain.First;
			while (xnode != null) {
				str += "(" + xnode.Value + "," + ynode.Value + ") ";
				xnode = xnode.Next;
				ynode = ynode.Next;
			}
			str += Environment.NewLine;
			return str;
		}

		//designed by for the function Draw in DoubleTree Functions.
		public string sigmaDraw() {
			string str = ""; bool firstchar = true;
			LinkedListNode<int> sigmaNode = sigmaRangeLabelsToDomainLabelsList.First;
			while (sigmaNode != null) {
				if (sigmaNode.Value < 10 && firstchar!=true ) str += " " + sigmaNode.Value; else str += sigmaNode.Value;
				firstchar = false;
				sigmaNode = sigmaNode.Next;
			}
			return str;
		}


		
	}
}
