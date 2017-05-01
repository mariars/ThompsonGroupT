using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Diagnostics;
//using System.Diagnostics;//Dellater for debugging purposes.

namespace TFilesEvenSLEbin
{
	static class DoubleTreeFunctions
    {
		//This program was originally written for composing elements of Thompson group T, using the generators C, D which are not the Cannon-Floyd-Perry generators.
		//The actions of C and D on elements of T (represented as reduced double trees with a rotation) will appear in future work by S. Haagerup, U. Haagerup and M. Ramirez-Solano.
		//The algorithm for the action of x0 x1 C or D can be generalized to the action of any element from T.


		//This function draws the double tree to the screen.
		//it is mapped however first to two arrays.
		static public string Draw(DoubleTree doubleTree) {
			string strOutput="";
			DoubleTreeCaretData doubleTreeCaretData = new DoubleTreeCaretData(doubleTree);
			int x = 0, y = 0, xp = 0, yp = 0;
			int numberofleafs = doubleTreeCaretData.numberOfLeaves;
			if (numberofleafs == 1)//is identity
				{
				strOutput+=".\n0\n.";
				return strOutput;
			}
			strOutput+="nLeaves= " + numberofleafs +" sigma("+doubleTreeCaretData.sigmaRangeLabelsToDomainLabelsList.First.Value+")=0leaf "+ " rMark= " + doubleTreeCaretData.marker_range + " dMark= " + doubleTreeCaretData.marker_domain;

			char[,] ranScreen = new char[numberofleafs, 2 * numberofleafs];
			char[,] domScreen = new char[numberofleafs, 2 * numberofleafs];


			while (doubleTreeCaretData.Next(ref x, ref y, ref xp, ref yp) == true) {
				//drawRangeCaret in screen array [row=yaxis, column=xaxis]
				for (int i = 0; i < y - x; i++) {
					ranScreen[numberofleafs - 1 - i, 2 * x + i] = '/';// -1= array starts with 0
					ranScreen[numberofleafs - 1 - i, 2 * y - 1 - i] = '\\';
				}
				//drawDomainCaret in screen array [row=yaxis, colum=xaxis]
				for (int i = 0; i < yp - xp; i++) {
					domScreen[i, 2 * xp + i] = '\\';// -1= array starts with 0
					domScreen[i, 2 * yp - 1 - i] = '/';
				}
			}
			//drawing range tree on screen
			for (int i = 0; i < numberofleafs; i++) {
				for (int j = 0; j < 2 * numberofleafs; j++)
					if (ranScreen[i, j] == 0) strOutput+=" "; else strOutput+=ranScreen[i, j];
				strOutput+="\n";
			}
			//write 01234.. on the range leaves
			for (int i = 0; i < numberofleafs; i++) strOutput += (i % 10) + " "; strOutput+="\n";
			//write the permutation on the domain leaves
			strOutput+=doubleTreeCaretData.sigmaDraw()+"\n";
			//drawing domain tree on screen
			for (int i = 0; i < numberofleafs; i++) {
				for (int j = 0; j < 2 * numberofleafs; j++)
					if (domScreen[i, j] == 0) strOutput+=" "; else strOutput+=domScreen[i, j];
				strOutput+="\n";
			}
			return strOutput;
		}

		#region Information functions InfoT, InfoF 
		static public string InfoT(DoubleTree doubleTree) {
			string infostr = "";

			infostr += "INFO_T:" + Environment.NewLine;

			DoubleTreeCaretData doubleTreeCaretData = new DoubleTreeCaretData(doubleTree);
			infostr += "carets:\n"+doubleTreeCaretData.CaretPoints();
			infostr += "Leave_Depths: " + DoubleTree_DepthOfLeavesSTR(doubleTree);

			infostr += "\ndoubleTree=" + DoubleTreeFunctions.DoubleTreeToString_Format01Sigma0(doubleTree) + " (3 = \"sigma(0) maps to this leaf\".) ";
			infostr += "\ndoubleTree=" + DoubleTreeFunctions.DoubleTreeToStringBin(doubleTree);
			infostr += "\ndoubleTreeInverse=" + DoubleTreeFunctions.DoubleTreeToInverseToString(doubleTree) + "\n";


			int counter = 0;
			int leftcounter = 0;
			int rightcounter = 0;
			int depth = 0;
			string strdepthrange = "Leaf depth R: ";// depth of leaves.
			MyList myListR = new MyList();
			string strRLeaves = readTreePreorder(doubleTree.range, ref counter, ref leftcounter, ref rightcounter, true, ref myListR, depth, ref strdepthrange);

			counter = 0;
			leftcounter = 0;
			rightcounter = 0;
			depth = 0;
			string strdepthdomain = "Leaf depth D: ";//depth of leaves.
			MyList myListD = new MyList();
			string strDleaves = readTreePreorder(doubleTree.domain, ref counter, ref leftcounter, ref rightcounter, true, ref myListD, depth, ref strdepthdomain);

			MyNode cur = myListD.First;
			string strDomain = "Domain: ";
			while (cur != null) {
				strDomain = strDomain + (cur.Value.leaves) + " ";
				cur = cur.Next;
			}

			cur = myListR.First;
			string strRange = " Range: ";
			string strSigma = "";
			string str123 = ""; int ct = 1;

			while (cur != null) {
				strRange += (cur.Value.leaves) + " ";
				strSigma += (cur.Value.sigma.leaves) + " ";
				str123 += ct + " "; ++ct;
				cur = cur.Next;
			}

			//strSigma maps range to domain. we want domain to range
			string cat = str123 + str123;
			string strSigmaInverse = cat.Substring(strSigma.IndexOf('1'), strSigma.Length);

			//infostr += "\nbbbb\n";
			//infostr += strRLeaves + Environment.NewLine;
			//infostr += "\ncccc\n";


			infostr += strRange + " Length= " + myListR.Length + Environment.NewLine;
			infostr += " sigma: " + strSigma + " Length= " + myListR.Length + " (Sigma:RangeLeaves->DomainLeaves)" + Environment.NewLine;
			infostr += "   inv: " + strSigmaInverse + " Length= " + myListD.Length + " (SigmaInverse:DomainLeaves->RangeLeaves )" + Environment.NewLine;

			infostr += strDomain + " Length= " + myListD.Length + Environment.NewLine;
			infostr += strdepthrange + " Length= " + myListD.Length + Environment.NewLine;
			infostr += strdepthdomain + " Length= " + myListD.Length + Environment.NewLine;

			//infostr += "\ncccc\n";
			//infostr += strDleaves + Environment.NewLine;
			//infostr += "\nddd\n";

			return infostr;
		}

		static public string InfoF(DoubleTree doubleTree) {
			string infostr = "";

		
			infostr+="INFO_F:"+Environment.NewLine;
			

			DoubleTreeCaretData doubleTreeCaretData = new DoubleTreeCaretData(doubleTree);
			infostr += doubleTreeCaretData.CaretPoints()+Environment.NewLine;

			infostr +="\nNormalForm: "+ DoubleTreeNormalFormSTR(doubleTree);

			infostr +="\nLeave_Depths: "+ DoubleTree_DepthOfLeavesSTR(doubleTree);

			infostr += "\nLeafLabels:\n" + DoubleTreeLeafLabels(doubleTree);
			infostr += "\nLeafLabels used to obtain the length:\n" + DoubleTreeLeafLabels_length(doubleTree) +
				"\n...gives length= " + DoubleTreeLength(doubleTree) + Environment.NewLine;

			infostr += "\ndoubleTree=" + DoubleTreeFunctions.DoubleTreeToString_Format01Sigma0(doubleTree) +" (The 3 is irrelevant for elements of F (3is0inF). It is relevant for elements of T.) ";
			infostr += "\ndoubleTree=" + DoubleTreeFunctions.DoubleTreeToStringBin(doubleTree);
			infostr += "\ndoubleTreeInverse=" + DoubleTreeFunctions.DoubleTreeToInverseToString(doubleTree)+"\n";
			infostr += "\ndoubleTreeToForest=" + DoubleTreeFunctions.DoubleTreeToForestToString(doubleTree);
			infostr += "\ndoubleTreeInverseToForest=" + DoubleTreeFunctions.DoubleTreeToInverseToForestToString(doubleTree) ;



			int counter = 0;
			int leftcounter = 0;
			int rightcounter = 0;
			int depth = 0;
			string strdepthrange = "Leaf depth R: ";// depth of leaves.
			MyList myListR = new MyList();
			string strRLeaves = readTreePreorder(doubleTree.range, ref counter, ref leftcounter, ref rightcounter, true, ref myListR,depth,ref strdepthrange);

			counter = 0;
			leftcounter = 0;
			rightcounter = 0;
			depth = 0;
			string strdepthdomain = "Leaf depth D: ";//depth of leaves.
			MyList myListD = new MyList();
			string strDleaves = readTreePreorder(doubleTree.domain, ref counter, ref leftcounter, ref rightcounter, true, ref myListD,depth,ref strdepthdomain);

			MyNode cur = myListD.First;
			string strDomain = "Domain: ";
			while (cur != null) {
				strDomain = strDomain + (cur.Value.leaves) + " ";
				cur = cur.Next;
			}


			cur = myListR.First;
			string strRange = " Range: ";
			string strSigma = "";
			string str123 = ""; int ct = 1;

			while (cur != null) {
				strRange +=  (cur.Value.leaves) + " ";
				strSigma +=  (cur.Value.sigma.leaves) + " ";
				str123 +=   ct + " "; ++ct;
				cur = cur.Next;
			}

			//strSigma maps range to domain. we want domain to range
			string cat = str123 + str123;
			string strSigmaInverse = cat.Substring(strSigma.IndexOf('1'), strSigma.Length);


			infostr += strRLeaves + Environment.NewLine;


			infostr +=strRange + " Length= " + myListR.Length + Environment.NewLine;
			infostr +=" sigma: "+strSigma + " Length= " + myListR.Length + " (Sigma:RangeLeaves->DomainLeaves)" + Environment.NewLine;
			infostr +="   inv: "+strSigmaInverse + " Length= " + myListD.Length + " (SigmaInverse:DomainLeaves->RangeLeaves )" + Environment.NewLine;
			infostr +=strDomain + " Length= " + myListD.Length + Environment.NewLine;
			infostr +=strdepthrange + " Length= " + myListD.Length + Environment.NewLine;
			infostr +=strdepthdomain + " Length= " + myListD.Length + Environment.NewLine;

			infostr +=strDleaves + Environment.NewLine;


			return infostr;
		}

		//Reads a tree in preorder in recursive mode: See http://en.wikipedia.org/wiki/Tree_traversal
		//stores the leafs in myList
		static private string readTreePreorder(TreeNode node, ref int counter, ref int leftcounter, ref int rightcounter, bool currentNodeIsTheLeftChild, ref MyList myList, int depth, ref string strdepth) {
			if (node == null) return ".";
			string str = "";
			str += visit(node, str, ref counter, ref leftcounter, ref rightcounter, node.currentNodeIsTheLeftChild, ref myList, depth, ref strdepth);
			str += readTreePreorder(node.left, ref counter, ref leftcounter, ref rightcounter, currentNodeIsTheLeftChild, ref myList, depth + 1, ref strdepth);
			str += readTreePreorder(node.right, ref  counter, ref leftcounter, ref rightcounter, currentNodeIsTheLeftChild, ref myList, depth + 1, ref strdepth);
			return str + "COMPLETED(c,l,r)=(" + counter + "," + leftcounter + "," + rightcounter + ")" + Environment.NewLine;//Completion arrives when it reaches a leaf
		}

		static private string visit(TreeNode node, string str, ref int counter, ref int leftcounter, ref int rightcounter, bool currentNodeIsTheLeftChild, ref MyList myList, int depth, ref string strdepth) {
			if (node.left == null && node.right == null) {
				strdepth += depth + ",";
				str = "";
				if (currentNodeIsTheLeftChild == true) {
					str = "=LeftLeaf "; ++leftcounter; str += leftcounter;
					str += "`" + (leftcounter + rightcounter) + "'";
				}
				else {
					str = "=RightLeaf ";
					++rightcounter;
					str += rightcounter;
					str += "`" + (leftcounter + rightcounter) + "'";
				}
				node.leaves = leftcounter + rightcounter;
				myList.AddLast(node);
				return "[Node" + (++counter) + str + "]";
			}
			return "{Node" + (++counter) + "}";
		}

		//leaf 1 2 3 ... if myNode!=null
		/*static public int leafNumber(MyNode myNode) {
			MyNode cur = myNode;
			int count = 0;
			while (cur != null) {
				++count;
				cur = cur.Previous;
			}
			return count;
		}
		  */
		#endregion

		#region functions Ai Aiinv C D xi xiinv
		static public void functionA1inv(ref DoubleTree doubleTree) {
			functionCinv(ref doubleTree);
			functionDinv(ref doubleTree);
		}

		static public void functionA2inv(ref DoubleTree doubleTree) {
			functionCinv(ref doubleTree);
			functionDsqr(ref doubleTree);
		}

		static public void functionA3inv(ref DoubleTree doubleTree) {
			functionCinv(ref doubleTree);
			functionD(ref doubleTree);
		}

		static public void functionA4inv(ref DoubleTree doubleTree) {
			functionC(ref doubleTree);
			functionDinv(ref doubleTree);
		}

		static public void functionA5inv(ref DoubleTree doubleTree) {
			functionC(ref doubleTree);
			functionDsqr(ref doubleTree);
		}

		static public void functionA6inv(ref DoubleTree doubleTree) {
			functionC(ref doubleTree);
			functionD(ref doubleTree);
		}


		static public void functionA1toA6(ref DoubleTree dt, int n) {
			switch (n) {
				case 1: functionA1(ref dt); break;
				case 2: functionA2(ref dt); break;
				case 3: functionA3(ref dt); break;
				case 4: functionA4(ref dt); break;
				case 5: functionA5(ref dt); break;
				case 6: functionA6(ref dt); break;
			}
		}


		static public void functionA1(ref DoubleTree doubleTree) {
			functionD(ref doubleTree);
			functionC(ref doubleTree);
		}

		static public void functionA2(ref DoubleTree doubleTree) {
			functionDsqr(ref doubleTree);
			functionC(ref doubleTree);
		}

		static public void functionA3(ref DoubleTree doubleTree) {
			functionDinv(ref doubleTree);
			functionC(ref doubleTree);
		}

		static public void functionA4(ref DoubleTree doubleTree) {
			functionD(ref doubleTree);
			functionCinv(ref doubleTree);
		}

		static public void functionA5(ref DoubleTree doubleTree) {
			functionDsqr(ref doubleTree);
			functionCinv(ref doubleTree);
		}

		static public void functionA6(ref DoubleTree doubleTree) {
			functionDinv(ref doubleTree);
			functionCinv(ref doubleTree);
		}



		static public void functionX0(ref DoubleTree doubleTree) {
			//The action of X0 on a double tree with range:
			//     .
			//    / \ Ip
			//   /  /\
			//  I  II III
			// is the following:
			//     .
			//  n1/ \
			//   /\  \
			//  I II III

			//ALGORITHM:
			// (1) If (I,Ip) does not exist, then we have the identity and so we return just X0;
			// (2) If (II, III) dooes not exist then a caret is created on Ip.sigma which is the element in the domain mapping to Ip, which in this case is a leaf.
			//     If (I, II) and (I.sigma,II.sigma) form carets (hence they match; note: carets do not commute) then
			// (3)         create plain new node n1 with n1.sigma=I.sigma.parent; and remove caret (I.sigma,II.sigma)
			// (4) else create new node n1=(I,II);
			//     If (I,II) was reduced and III is a leaf (i.e. (n1,III) is a caret) and (n1.sigma,III.sigma) form a caret (hence they match) then
			// (5)             return the identity;
			// (6) else redefine range=(n1,III); 


			//(1) doubleTree is the identity
			if (doubleTree.range.left == null && doubleTree.domain.left == null) {
				doubleTree = new DoubleTree('0');//X0
				return;
			}

			TreeNode I = doubleTree.range.left;
			TreeNode Ip = doubleTree.range.right;

			//(2)
			if (Ip.left == null) {//add caret on leaf Ip and on leaf sigmainverse(Ip)
				TreeNode sigmainverseOfIp = Ip.sigma;
				sigmainverseOfIp.left = new TreeNode(parentNode: sigmainverseOfIp, currentNodeIsTheLeftChild: true);
				sigmainverseOfIp.right = new TreeNode(parentNode: sigmainverseOfIp, currentNodeIsTheLeftChild: false);
				Ip.left = new TreeNode(parentNode: Ip, currentNodeIsTheLeftChild: true, sigmaNode: sigmainverseOfIp.left);
				Ip.right = new TreeNode(parentNode: Ip, currentNodeIsTheLeftChild: false, sigmaNode: sigmainverseOfIp.right);
				Ip.sigma = null;
			}

			TreeNode II = doubleTree.range.right.left;
			TreeNode III = doubleTree.range.right.right;
			TreeNode n1 = null;
			bool pairIandIIwasReduced = false;
			if (I.left == null && II.right == null // (I,II) form a caret
				&& I.sigma.currentNodeIsTheLeftChild == true
				&& II.sigma.currentNodeIsTheLeftChild == false
				&& I.sigma.parent == II.sigma.parent // (I.sigma,II.sigma) for a caret
				) {//(3)
				n1 = new TreeNode(sigmaNode: I.sigma.parent);
				//removing bottom caret
				n1.sigma.left = null;
				n1.sigma.right = null;
				pairIandIIwasReduced = true;
			}
			else//(4)
				n1 = new TreeNode(leftNode: I, rightNode: II);

			if (pairIandIIwasReduced == true && III.left == null//(n1,III) form a caret
				&& n1.sigma.currentNodeIsTheLeftChild == true
				&& III.sigma.currentNodeIsTheLeftChild == false
				&& n1.sigma.parent == III.sigma.parent//(n1.sigma,III.sigma) form a caret
				) {//(5)
				doubleTree = new DoubleTree(); // The element is the identity
				return;
			}//6
			doubleTree.range = new TreeNode(leftNode: n1, rightNode: III);
			return;
		}


		static public void functionX0inv(ref DoubleTree doubleTree) {
			//The action of X0^-1 on a double tree with range:
			//     .
			//  Ip/ \
			//   /\  \
			//  I  II III
			// is the following:
			//     .
			//    / \n1
			//   /  /\
			//  I II III

			//ALGORITHM:
			// (1) If (Ip,III) does not exist, then we have the identity and so we return just X0^-1;
			// (2) If (I, II) dooes not exist then a caret is created on Ip.sigma which is the element in the domain mapping to Ip, which in this case is a leaf.
			//     If (II, III) and (II.sigma,III.sigma) form carets (hence they match; note: carets do not commute) then
			// (3)         create plain new node n1 with n1.sigma=II.sigma.parent; and remove caret (II.sigma,III.sigma)
			// (4) else create new node n1=(II,III);
			//     If (II,III) was reduced and I is a leaf (i.e. (I,n1) is a caret) and (I.sigma,n1.sigma) form a caret (hence they match) then
			// (5)             return the identity;
			// (6) else redefine range=(I,n1); 


			//(1) doubleTree is the identity
			if (doubleTree.range.left == null && doubleTree.domain.left == null) {
				doubleTree = new DoubleTree('2');//X0inv
				return;
			}

			TreeNode Ip = doubleTree.range.left;
			TreeNode III = doubleTree.range.right;

			//(2)
			if (Ip.left == null) {//add caret on leaf Ip and on leaf sigmainverse(Ip)
				TreeNode sigmainverseOfIp = Ip.sigma;
				sigmainverseOfIp.left = new TreeNode(parentNode: sigmainverseOfIp, currentNodeIsTheLeftChild: true);
				sigmainverseOfIp.right = new TreeNode(parentNode: sigmainverseOfIp, currentNodeIsTheLeftChild: false);
				Ip.left = new TreeNode(parentNode: Ip, currentNodeIsTheLeftChild: true, sigmaNode: sigmainverseOfIp.left);
				Ip.right = new TreeNode(parentNode: Ip, currentNodeIsTheLeftChild: false, sigmaNode: sigmainverseOfIp.right);
				Ip.sigma = null;
			}

			TreeNode I = doubleTree.range.left.left;
			TreeNode II = doubleTree.range.left.right;
			TreeNode n1 = null;
			bool pairIIandIIIwasReduced = false;
			if (II.left == null && III.right == null // (II,III) form a caret
				&& II.sigma.currentNodeIsTheLeftChild == true
				&& III.sigma.currentNodeIsTheLeftChild == false
				&& II.sigma.parent == III.sigma.parent // (II.sigma,III.sigma) for a caret
				) {//(3)
				n1 = new TreeNode(sigmaNode: II.sigma.parent);
				//removing bottom caret
				n1.sigma.left = null;
				n1.sigma.right = null;
				pairIIandIIIwasReduced = true;
			}
			else//(4)
				n1 = new TreeNode(leftNode: II, rightNode: III);

			if (pairIIandIIIwasReduced == true && I.left == null//(I,n1) form a caret
				&& I.sigma.currentNodeIsTheLeftChild == true
				&& n1.sigma.currentNodeIsTheLeftChild == false
				&& I.sigma.parent == n1.sigma.parent//(I.sigma,n1.sigma) form a caret
				) {//(5)
				doubleTree = new DoubleTree(); // The element is the identity
				return;
			}//6
			doubleTree.range = new TreeNode(leftNode: I, rightNode: n1);
			return;
		}


		static public void functionX1(ref DoubleTree doubleTree) {
			//The action of x1 on a double tree with range:
			//      /\
			//     /  \Ip
			//    /  / \Ipp
			//   /  /  /\
			//  I II III IV
			// is the following:
			//      /\
			//     /  \n2
			//    /n1/ \
			//   /  /\  \
			//  I II III IV

			//ALGORITHM:
			// (1) If (I,Ip) does not exist, then we have the identity and so we return just x1;
			// (2) If (II, Ipp) does not exist then create a caret on Ip.sigma which is the element in the domain that maps to Ip, which in this case is a leaf.
			// (3) If (III, IV) does not exist then create a caret on Ipp.sigma which is the element in the domain that maps to Ipp, which in this case is a leaf.
			//     If (II, III) and (II.sigma,III.sigma) form carets (hence they match; Note: carets do not commute ) then 
			// (4)      create a plain new node n1 with n1.sigma=II.sigma.parent and remove caret (II.sigma,III.sigma).
			// (5) else create node n1=(II,III)
			//     If (II,III) was reduced and IV is a leaf (i.e. (n1,IV) is a caret) and (n1.sigma,IV.sigma) is a caret 
			// (6)      create a plain new node n2 with n2.sigma=IV.sigma.parent and remove caret (n1.sigma,IV.sigma).
			// (7) else create node n2=(n1,IV)     
   			//     if (n1,IV) was reduced and I is a leaf (i.e. (I,n2) is a caret) and  (I.sigma,n2.sigma) is a caret then 
			// (8)     return the identity    
			// (9) else redefine range=(I,n2) and return


			//(1) doubleTree is the identity
			if (doubleTree.range.left == null && doubleTree.domain.left == null) {
				doubleTree = new DoubleTree('1');
				return;
			}

			TreeNode I = doubleTree.range.left;
			TreeNode Ip = doubleTree.range.right;

			//(2)
			if (Ip.left == null) {//add caret on leaf Ip and on leaf sigmainverse(Ip)
				TreeNode sigmainverseOfIp = Ip.sigma;
				sigmainverseOfIp.left = new TreeNode(parentNode: sigmainverseOfIp, currentNodeIsTheLeftChild: true);
				sigmainverseOfIp.right = new TreeNode(parentNode: sigmainverseOfIp, currentNodeIsTheLeftChild: false);
				Ip.left = new TreeNode(parentNode: Ip, currentNodeIsTheLeftChild: true, sigmaNode: sigmainverseOfIp.left);
				Ip.right = new TreeNode(parentNode: Ip, currentNodeIsTheLeftChild: false, sigmaNode: sigmainverseOfIp.right);
				Ip.sigma = null;
			}

			TreeNode II = Ip.left;
			TreeNode Ipp = Ip.right;
			//(3)
			if (Ipp.left == null) {//add caret on leaf Ipp and on leaf sigmainverse(Ipp)
				TreeNode sigmainverseOfIpp = Ipp.sigma;
				sigmainverseOfIpp.left = new TreeNode(parentNode: sigmainverseOfIpp, currentNodeIsTheLeftChild: true);
				sigmainverseOfIpp.right = new TreeNode(parentNode: sigmainverseOfIpp, currentNodeIsTheLeftChild: false);
				Ipp.left = new TreeNode(parentNode: Ipp, currentNodeIsTheLeftChild: true, sigmaNode: sigmainverseOfIpp.left);
				Ipp.right = new TreeNode(parentNode: Ipp, currentNodeIsTheLeftChild: false, sigmaNode: sigmainverseOfIpp.right);
				Ipp.sigma = null;
			}

			TreeNode III = Ipp.left;
			TreeNode IV = Ipp.right;
			TreeNode n1 = null;
			TreeNode n2 = null;
			bool pairIIandIIIwasReduced = false;
			bool pairn2andIVwasReduced = false;

			if (II.left == null && III.right == null // (II,III) form a caret
				&& II.sigma.currentNodeIsTheLeftChild == true
				&& III.sigma.currentNodeIsTheLeftChild == false
				&& II.sigma.parent == III.sigma.parent // (II.sigma,III.sigma) form a caret
				) {//(4)
				n1 = new TreeNode(sigmaNode: II.sigma.parent);
				//removing bottom caret
				n1.sigma.left = null;
				n1.sigma.right = null;
				pairIIandIIIwasReduced = true;
			}
			else//(5)
				n1 = new TreeNode(leftNode: II, rightNode: III);


			if (pairIIandIIIwasReduced == true && IV.left==null//(n1,IV) form a caret
				&& n1.sigma.currentNodeIsTheLeftChild == true
				&& IV.sigma.currentNodeIsTheLeftChild == false
				&& n1.sigma.parent == IV.sigma.parent//(n1.sigma,IV.sigma) form a caret
				) {//(6)
				n2 = new TreeNode(sigmaNode: IV.sigma.parent);
				//removing bottom caret
				n2.sigma.left = null;
				n2.sigma.right = null;
				pairn2andIVwasReduced = true;			
			}
			else//(7)
				n2 = new TreeNode(leftNode: n1, rightNode: IV);

			

			if (I.left == null && pairn2andIVwasReduced == true//(I,n2) form a caret
				&& I.sigma.currentNodeIsTheLeftChild == true
				&& n2.sigma.currentNodeIsTheLeftChild == false
				&& I.sigma.parent == n2.sigma.parent//(I.sigma,n2.sigma) form a caret
				) {//(8)
				doubleTree = new DoubleTree(); // The element is the identity
				return;
			}
			//(9)
			doubleTree.range = new TreeNode(leftNode: I, rightNode: n2);
			return;
		}

		static public void functionX1inv(ref DoubleTree doubleTree) {
			//The action of x1^-1 on a double tree with range:
			//       .
			//      / \
			//     /   \Ip
			//    /Ipp/ \
			//   /   /\  \
			//  I  II III IV
			// is the following:
			//      /\
			//     /  \n2
			//    /  / \n1
			//   /  /  /\
			//  I II III IV

			//ALGORITHM:
			// (1) If (I,Ip) does not exist, then we have the identity and so we return just x1;
			// (2) If (Ipp, IV) does not exist then create a caret on Ip.sigma which is the element in the domain that maps to Ip, which in this case is a leaf.
			// (3) If (II, III) does not exist then create a caret on Ipp.sigma which is the element in the domain that maps to Ipp, which in this case is a leaf.
			//     If (III, IV) and (III.sigma,IV.sigma) form carets (hence they match; Note: carets do not commute ) then 
			// (4)      create a plain new node n1 with n1.sigma=III.sigma.parent and remove caret (III.sigma,IV.sigma).
			// (5) else create node n1=(III,IV)
			//     If (III,IV) was reduced and II is a leaf (i.e. (II,n1) is a caret) and (II.sigma,n1.sigma) is a caret 
			// (6)      create a plain new node n2 with n2.sigma=II.sigma.parent and remove caret (II.sigma,n1.sigma).
			// (7) else create node n2=(II,n1)     
			//     if (II,n1) was reduced and I is a leaf (i.e. (I,n2) is a caret) and  (I.sigma,n2.sigma) is a caret then 
			// (8)     return the identity    
			// (9) else redefine range=(I,n2) and return


			//(1) doubleTree is the identity
			if (doubleTree.range.left == null && doubleTree.domain.left == null) {
				doubleTree = new DoubleTree('3');
				return;
			}

			TreeNode I = doubleTree.range.left;
			TreeNode Ip = doubleTree.range.right;

			//(2)
			if (Ip.left == null) {//add caret on leaf Ip and on leaf sigmainverse(Ip)
				TreeNode sigmainverseOfIp = Ip.sigma;
				sigmainverseOfIp.left = new TreeNode(parentNode: sigmainverseOfIp, currentNodeIsTheLeftChild: true);
				sigmainverseOfIp.right = new TreeNode(parentNode: sigmainverseOfIp, currentNodeIsTheLeftChild: false);
				Ip.left = new TreeNode(parentNode: Ip, currentNodeIsTheLeftChild: true, sigmaNode: sigmainverseOfIp.left);
				Ip.right = new TreeNode(parentNode: Ip, currentNodeIsTheLeftChild: false, sigmaNode: sigmainverseOfIp.right);
				Ip.sigma = null;
			}

			TreeNode Ipp = Ip.left;
			TreeNode IV = Ip.right;
			//(3)
			if (Ipp.left == null) {//add caret on leaf Ipp and on leaf sigmainverse(Ipp)
				TreeNode sigmainverseOfIpp = Ipp.sigma;
				sigmainverseOfIpp.left = new TreeNode(parentNode: sigmainverseOfIpp, currentNodeIsTheLeftChild: true);
				sigmainverseOfIpp.right = new TreeNode(parentNode: sigmainverseOfIpp, currentNodeIsTheLeftChild: false);
				Ipp.left = new TreeNode(parentNode: Ipp, currentNodeIsTheLeftChild: true, sigmaNode: sigmainverseOfIpp.left);
				Ipp.right = new TreeNode(parentNode: Ipp, currentNodeIsTheLeftChild: false, sigmaNode: sigmainverseOfIpp.right);
				Ipp.sigma = null;
			}

			TreeNode II = Ipp.left;
			TreeNode III = Ipp.right;
			TreeNode n1 = null;
			TreeNode n2 = null;
			bool pairIIIandIVwasReduced = false;
			bool pairIIandn1wasReduced = false;

			if (III.left == null && IV.right == null // (III,IV) form a caret
				&& III.sigma.currentNodeIsTheLeftChild == true
				&& IV.sigma.currentNodeIsTheLeftChild == false
				&& III.sigma.parent == IV.sigma.parent // (III.sigma,IV.sigma) form a caret
				) {//(4)
				n1 = new TreeNode(sigmaNode: III.sigma.parent);
				//removing bottom caret
				n1.sigma.left = null;
				n1.sigma.right = null;
				pairIIIandIVwasReduced = true;
			}
			else//(5)
				n1 = new TreeNode(leftNode: III, rightNode: IV);


			if (pairIIIandIVwasReduced == true && II.left == null//(II,n1) form a caret
				&& II.sigma.currentNodeIsTheLeftChild == true
				&& n1.sigma.currentNodeIsTheLeftChild == false
				&& II.sigma.parent == n1.sigma.parent//(II.sigma,n1.sigma) form a caret
				) {//(6)
				n2 = new TreeNode(sigmaNode: II.sigma.parent);
				//removing bottom caret
				n2.sigma.left = null;
				n2.sigma.right = null;
				pairIIandn1wasReduced = true;
			}
			else//(7)
				n2 = new TreeNode(leftNode: II, rightNode: n1);



			if (I.left == null && pairIIandn1wasReduced == true//(I,n2) form a caret
				&& I.sigma.currentNodeIsTheLeftChild == true
				&& n2.sigma.currentNodeIsTheLeftChild == false
				&& I.sigma.parent == n2.sigma.parent//(I.sigma,n2.sigma) form a caret
				) {//(8)
				doubleTree = new DoubleTree(); // The element is the identity
				return;
			}
			//(9)
			doubleTree.range = new TreeNode(leftNode: I, rightNode: n2);
			return;
		}


		static public void functionC(ref DoubleTree doubleTree) {
			//The action of C on a double tree with range:
			//     .
			//    / \Ip
			//   /  /\
			//  I  II III
			// is the following:
			//     .
			//    / \n1
			//   /  /\
			// II III I

			//ALGORITHM:
			// (1) If (I,Ip) does not exist, then we have the identity and so we return just C;
			// (2) If (II, III) dooes not exist then a caret is created on Ip.sigma which is the element in the domain mapping to Ip, which in this case is a leaf.
			//     If (III, I) and (III.sigma,I.sigma) form carets (hence they match; note: carets do not commute) then
			// (3)         create plain new node n1 with n1.sigma=III.sigma.parent; and remove caret (III.sigma,I.sigma)
			// (4) else create new node n1=(III,I);
			//     if (III,I) was reduced and II is a leaf (i.e. (II,n1) is a caret) and (II.sigma,n1.sigma) form a caret (hence they match) then
			// (5)             return the identity;
			// (6) else redefine range=(II,n1); 


			//(1) doubleTree is the identity
			if (doubleTree.range.left == null && doubleTree.domain.left == null) {
				doubleTree = new DoubleTree('C');
				return;
			}

			TreeNode I = doubleTree.range.left;
			TreeNode Ip = doubleTree.range.right;

			//(2)
			if (Ip.left == null) {//add caret on leaf Ip and on leaf sigmainverse(Ip)
				TreeNode sigmainverseOfIp = Ip.sigma;
				sigmainverseOfIp.left = new TreeNode(parentNode: sigmainverseOfIp, currentNodeIsTheLeftChild: true);
				sigmainverseOfIp.right = new TreeNode(parentNode: sigmainverseOfIp, currentNodeIsTheLeftChild: false);
				Ip.left = new TreeNode(parentNode: Ip, currentNodeIsTheLeftChild: true, sigmaNode: sigmainverseOfIp.left);
				Ip.right = new TreeNode(parentNode: Ip, currentNodeIsTheLeftChild: false, sigmaNode: sigmainverseOfIp.right);
				Ip.sigma = null;
			}

			TreeNode II = doubleTree.range.right.left;
			TreeNode III = doubleTree.range.right.right;
			TreeNode n1 = null;
			bool pairIIIandIwasReduced = false;
			if (III.left == null && I.right == null // (III,I) form a caret
				&& III.sigma.currentNodeIsTheLeftChild == true
				&& I.sigma.currentNodeIsTheLeftChild == false
				&& III.sigma.parent == I.sigma.parent // (III.sigma,I.sigma) for a caret
				) {//(3)
				n1 = new TreeNode(sigmaNode: III.sigma.parent);
				//removing bottom caret
				n1.sigma.left = null;
				n1.sigma.right = null;
				pairIIIandIwasReduced = true;
			}
			else//(4)
				n1 = new TreeNode(leftNode: III, rightNode: I);

			if (pairIIIandIwasReduced == true && II.left == null//(II,n1) form a caret
				&& II.sigma.currentNodeIsTheLeftChild == true
				&& n1.sigma.currentNodeIsTheLeftChild == false
				&& II.sigma.parent == n1.sigma.parent//(II.sigma,n1.sigma) form a caret
				) {//(5)
				doubleTree = new DoubleTree(); // The element is the identity
				return;
			}//6
			doubleTree.range = new TreeNode(leftNode: II, rightNode: n1);
			return;
		}


		static public void functionCinv(ref DoubleTree doubleTree) {
			//The action of C^-1 on a double tree with range:
			//     .
			//    / \Ip
			//   /  /\
			//  I  II III
			// is the following:
			//     .
			//    / \n1
			//   /  /\
			// III I II

			//ALGORITHM:
			// (1) If (I,Ip) does not exist, then we have the identity and so we return just C^-1;
			// (2) If (II, III) dooes not exist then a caret is created on Ip.sigma which is the element in the domain mapping to Ip, which in this case is a leaf.
			//     If (I, II) and (I.sigma,II.sigma) form carets (hence they match; note: carets do not commute) then
			// (3)         create plain new node n1 with n1.sigma=I.sigma.parent; and remove caret (I.sigma,II.sigma)
			// (4) else create new node n1=(I,II);
			//     if (I,II) was reduced and III is a leaf (i.e. (III,n1) is a caret) and (III.sigma,n1.sigma) form a caret (hence they match) then
			// (5)             return the identity;
			// (6) else redefine range=(III,n1); 


			//(1) doubleTree is the identity
			if (doubleTree.range.left == null && doubleTree.domain.left == null) {
				doubleTree = new DoubleTree('I');
				return;
			}

			TreeNode I = doubleTree.range.left;
			TreeNode Ip = doubleTree.range.right;

			//(2)
			if (Ip.left == null) {//add caret on leaf Ip and on leaf sigmainverse(Ip)
				TreeNode sigmainverseOfIp = Ip.sigma;
				sigmainverseOfIp.left = new TreeNode(parentNode: sigmainverseOfIp, currentNodeIsTheLeftChild: true);
				sigmainverseOfIp.right = new TreeNode(parentNode: sigmainverseOfIp, currentNodeIsTheLeftChild: false);
				Ip.left = new TreeNode(parentNode: Ip, currentNodeIsTheLeftChild: true, sigmaNode: sigmainverseOfIp.left);
				Ip.right = new TreeNode(parentNode: Ip, currentNodeIsTheLeftChild: false, sigmaNode: sigmainverseOfIp.right);
				Ip.sigma = null;
			}

			TreeNode II = doubleTree.range.right.left;
			TreeNode III = doubleTree.range.right.right;
			TreeNode n1 = null;
			bool pairIandIIwasReduced = false;
			if (I.left == null && II.right == null // (I,II) form a caret
				&& I.sigma.currentNodeIsTheLeftChild == true
				&& II.sigma.currentNodeIsTheLeftChild == false
				&& I.sigma.parent == II.sigma.parent // (I.sigma,II.sigma) for a caret
				) {//(3)
				n1 = new TreeNode(sigmaNode: I.sigma.parent);
				//removing bottom caret
				n1.sigma.left = null;
				n1.sigma.right = null;
				pairIandIIwasReduced = true;
			}
			else//(4)
				n1 = new TreeNode(leftNode: I, rightNode: II);

			if (pairIandIIwasReduced == true && III.left == null//(III,n1) form a caret
				&& III.sigma.currentNodeIsTheLeftChild == true
				&& n1.sigma.currentNodeIsTheLeftChild == false
				&& III.sigma.parent == n1.sigma.parent//(III.sigma,n1.sigma) form a caret
				) {//(5)
				doubleTree = new DoubleTree(); // The element is the identity
				return;
			}//(6)
			doubleTree.range = new TreeNode(leftNode: III, rightNode: n1);
			return;
		}

		static public void functionD(ref DoubleTree doubleTree) {
			//The action of D on a double tree with range:
			//      /\
			//     /  \
			// Ip /    \Ipp
			//   /\    /\
			//  I II III IV
			// is the following:
			//      /\
			//     /  \
			//  n1/    \n2
			//   /\    /\
			// II III IV I

			//ALGORITHM:
			// (1) If (Ip,Ipp) does not exist, then we have the identity and so we return just D;
			// (2) If (I, II) dooes not exist then create a caret on Ip.sigma which is the element in the domain that maps to Ip, which in this case is a leaf.
			// (3) If (III, IV) dooes not exist then create a caret on Ip.sigma which is the element in the domain that maps to Ipp, which in this case is a leaf.
			//     If (II, III) and (II.sigma,III.sigma) form carets (hence they match; Note: carets do not commute ) then 
			// (4)      create a plain new node n1 with n1.sigma=II.sigma.parent and remove caret (II.sigma,III.sigma).
			// (5) else create node n1=(II,III)
			//     If (IV,I) and (IV.sigma,I.sigma) form carets (hence they match ) then
			// (6)      create new node n2 with n2.sigma=IV.sigma.parent and remove caret: (IV.sigma, I.sigma), 
			// (7)    else create node n2=(IV,I)
			//     If both (II,III) and (IV,I) were reduced and (n1.sigma,n2.sigma) is a caret 
			// (8)                  return identity;
			// (9) redefine range=(n1,n2)  and return


			//(1) doubleTree is the identity
			if (doubleTree.range.left == null && doubleTree.domain.left == null) {
				doubleTree = new DoubleTree('D');
				return;
			}

			TreeNode Ip = doubleTree.range.left;
			TreeNode Ipp = doubleTree.range.right;

			//(2)
			if (Ip.left == null) {//add caret on leaf Ip and on leaf sigmainverse(Ip)
				TreeNode sigmainverseOfIp = Ip.sigma;
				sigmainverseOfIp.left = new TreeNode(parentNode: sigmainverseOfIp, currentNodeIsTheLeftChild: true);
				sigmainverseOfIp.right = new TreeNode(parentNode: sigmainverseOfIp, currentNodeIsTheLeftChild: false);
				Ip.left = new TreeNode(parentNode: Ip, currentNodeIsTheLeftChild: true, sigmaNode: sigmainverseOfIp.left);
				Ip.right = new TreeNode(parentNode: Ip, currentNodeIsTheLeftChild: false, sigmaNode: sigmainverseOfIp.right);
				Ip.sigma = null;
			}
			//(3)
			if (Ipp.left == null) {//add caret on leaf Ipp and on leaf sigmainverse(Ipp)
				TreeNode sigmainverseOfIpp = Ipp.sigma;
				sigmainverseOfIpp.left = new TreeNode(parentNode: sigmainverseOfIpp, currentNodeIsTheLeftChild: true);
				sigmainverseOfIpp.right = new TreeNode(parentNode: sigmainverseOfIpp, currentNodeIsTheLeftChild: false);
				Ipp.left = new TreeNode(parentNode: Ipp, currentNodeIsTheLeftChild: true, sigmaNode: sigmainverseOfIpp.left);
				Ipp.right = new TreeNode(parentNode: Ipp, currentNodeIsTheLeftChild: false, sigmaNode: sigmainverseOfIpp.right);
				Ipp.sigma = null;
			}

			TreeNode I = doubleTree.range.left.left;
			TreeNode II = doubleTree.range.left.right;
			TreeNode III = doubleTree.range.right.left;
			TreeNode IV = doubleTree.range.right.right;
			TreeNode n1 = null;
			TreeNode n2 = null;
			bool pairIIandIIIwasReduced = false;
			bool pairIVandIwasReduced = false;

			if (II.left == null && III.right == null // (II,III) form a caret
				&& II.sigma.currentNodeIsTheLeftChild == true
				&& III.sigma.currentNodeIsTheLeftChild == false
				&& II.sigma.parent == III.sigma.parent // (III.sigma,I.sigma) form a caret
				) {//(4)
				n1 = new TreeNode(sigmaNode: II.sigma.parent);
				//removing bottom caret
				n1.sigma.left = null;
				n1.sigma.right = null;
				pairIIandIIIwasReduced = true;
			}
			else//(5)
				n1 = new TreeNode(leftNode: II, rightNode: III);

			if (IV.left == null && I.right == null // (IV,I) form a caret
				&& IV.sigma.currentNodeIsTheLeftChild == true
				&& I.sigma.currentNodeIsTheLeftChild == false
				&& IV.sigma.parent == I.sigma.parent // (IV.sigma,I.sigma) form a caret
				) {//(6)
				n2 = new TreeNode(sigmaNode: IV.sigma.parent);
				//removing bottom caret
				n2.sigma.left = null;
				n2.sigma.right = null;
				pairIVandIwasReduced = true;
			}
			else//(7)
				n2 = new TreeNode(leftNode: IV, rightNode: I);

			if (pairIIandIIIwasReduced == true && pairIVandIwasReduced == true//(n1,n2) form a caret
				&& n1.sigma.currentNodeIsTheLeftChild == true
				&& n2.sigma.currentNodeIsTheLeftChild == false
				&& n1.sigma.parent == n2.sigma.parent//(n1.sigma,n2.sigma) form a caret
				) {//(8)
				doubleTree = new DoubleTree(); // The element is the identity
				return;
			}
			//(9)
			doubleTree.range = new TreeNode(leftNode: n1, rightNode: n2);
			return;
		}


		static public void functionDinv(ref DoubleTree doubleTree) {
			//The action of D^-1 on a double tree with range:
			//      /\
			//     /  \
			// Ip /    \Ipp
			//   /\    /\
			//  I II III IV
			// is the following:
			//      /\
			//     /  \
			//  n1/    \n2
			//   /\    /\
			//  IV I II III 

			//ALGORITHM:
			// (1) If (Ip,Ipp) does not exist, then we have the identity and so we return just D^-1;
			// (2) If (I, II) dooes not exist then create a caret on Ip.sigma which is the element in the domain that maps to Ip, which in this case is a leaf.
			// (3) If (III, IV) dooes not exist then create a caret on Ip.sigma which is the element in the domain that maps to Ipp, which in this case is a leaf.
			//     If (IV, I) and (IV.sigma,I.sigma) form carets (hence they match; Note: carets do not commute ) then 
			// (4)      create a plain new node n1 with n1.sigma=IV.sigma.parent and remove caret (IV.sigma,I.sigma).
			// (5) else create node n1=(IV,I)
			//     If (II,III) and (II.sigma,III.sigma) form carets (hence they match ) then
			// (6)      create new node n2 with n2.sigma=II.sigma.parent and remove caret: (II.sigma, III.sigma), 
			// (7)    else create node n2=(II,III)
			//     If both (IV,I) and (II,III) were reduced and (n1.sigma,n2.sigma) is a caret 
			// (8)                  return identity;
			// (9) redefine range=(n1,n2)  and return


			//(1) doubleTree is the identity
			if (doubleTree.range.left == null && doubleTree.domain.left == null) {
				doubleTree = new DoubleTree('J');
				return;
			}

			TreeNode Ip = doubleTree.range.left;
			TreeNode Ipp = doubleTree.range.right;

			//(2)
			if (Ip.left == null) {//add caret on leaf Ip and on leaf sigmainverse(Ip)
				TreeNode sigmainverseOfIp = Ip.sigma;
				sigmainverseOfIp.left = new TreeNode(parentNode: sigmainverseOfIp, currentNodeIsTheLeftChild: true);
				sigmainverseOfIp.right = new TreeNode(parentNode: sigmainverseOfIp, currentNodeIsTheLeftChild: false);
				Ip.left = new TreeNode(parentNode: Ip, currentNodeIsTheLeftChild: true, sigmaNode: sigmainverseOfIp.left);
				Ip.right = new TreeNode(parentNode: Ip, currentNodeIsTheLeftChild: false, sigmaNode: sigmainverseOfIp.right);
				Ip.sigma = null;
			}
			//(3)
			if (Ipp.left == null) {//add caret on leaf Ipp and on leaf sigmainverse(Ipp)
				TreeNode sigmainverseOfIpp = Ipp.sigma;
				sigmainverseOfIpp.left = new TreeNode(parentNode: sigmainverseOfIpp, currentNodeIsTheLeftChild: true);
				sigmainverseOfIpp.right = new TreeNode(parentNode: sigmainverseOfIpp, currentNodeIsTheLeftChild: false);
				Ipp.left = new TreeNode(parentNode: Ipp, currentNodeIsTheLeftChild: true, sigmaNode: sigmainverseOfIpp.left);
				Ipp.right = new TreeNode(parentNode: Ipp, currentNodeIsTheLeftChild: false, sigmaNode: sigmainverseOfIpp.right);
				Ipp.sigma = null;
			}

			TreeNode I = doubleTree.range.left.left;
			TreeNode II = doubleTree.range.left.right;
			TreeNode III = doubleTree.range.right.left;
			TreeNode IV = doubleTree.range.right.right;
			TreeNode n1 = null;
			TreeNode n2 = null;
			bool pairIVandIwasReduced = false;
			bool pairIIandIIIwasReduced = false;


			if (IV.left == null && I.right == null // (IV,I) form a caret
				&& IV.sigma.currentNodeIsTheLeftChild == true
				&& I.sigma.currentNodeIsTheLeftChild == false
				&& IV.sigma.parent == I.sigma.parent // (IV.sigma,I.sigma) form a caret
				) {//(4)
				n1 = new TreeNode(sigmaNode: IV.sigma.parent);
				//removing bottom caret
				n1.sigma.left = null;
				n1.sigma.right = null;
				pairIVandIwasReduced = true;
			}
			else//(5)
				n1 = new TreeNode(leftNode: IV, rightNode: I);

			if (II.left == null && III.right == null // (II,III) form a caret
				&& II.sigma.currentNodeIsTheLeftChild == true
				&& III.sigma.currentNodeIsTheLeftChild == false
				&& II.sigma.parent == III.sigma.parent // (III.sigma,I.sigma) form a caret
				) {//(6)
				n2 = new TreeNode(sigmaNode: II.sigma.parent);
				//removing bottom caret
				n2.sigma.left = null;
				n2.sigma.right = null;
				pairIIandIIIwasReduced = true;
			}
			else//(7)
				n2 = new TreeNode(leftNode: II, rightNode: III);

			if (pairIVandIwasReduced == true && pairIIandIIIwasReduced == true//(n1,n2) form a caret
				&& n1.sigma.currentNodeIsTheLeftChild == true
				&& n2.sigma.currentNodeIsTheLeftChild == false
				&& n1.sigma.parent == n2.sigma.parent//(n1.sigma,n2.sigma) form a caret
				) {//(8)
				doubleTree = new DoubleTree(); // The element is the identity
				return;
			}
			//(9)
			doubleTree.range = new TreeNode(leftNode: n1, rightNode: n2);
			return;
		}


		static public void functionDsqr(ref DoubleTree doubleTree) {
			//The action of D^2 on a double tree with range:
			//     .
			//    / \
			//   I  II 
			// is the following:
			//     .
			//    / \
			//   II  I

			//ALGORITHM:
			// (1) If (I,II) does not exist, then we have the identity and so we return just D^2;
			//     If (II, I) and (II.sigma,I.sigma) form carets (hence they match; note: carets do not commute) then
			// (2)     return identity;
			// (3) else redefine range=(II,n1); 

			//(1) doubleTree is the identity
			if (doubleTree.range.left == null && doubleTree.domain.left == null) {
				doubleTree = new DoubleTree('K');
				return;
			}

			TreeNode I = doubleTree.range.left;
			TreeNode II = doubleTree.range.right;


			if (II.left == null && I.right == null // (II,I) form a caret
				&& II.sigma.currentNodeIsTheLeftChild == true
				&& I.sigma.currentNodeIsTheLeftChild == false
				&& II.sigma.parent == I.sigma.parent // (II.sigma,I.sigma) form a caret
				) {//(2)
				doubleTree = new DoubleTree(); // doubleTree is assigned the identity
				return;
			}
			//(3)

			doubleTree.range = new TreeNode(leftNode: II, rightNode: I);

			return;
		}



		static public void functionXn(ref DoubleTree doubleTree,int n) {// x0 x1 x2 x3....
				if (n == 0) { functionX0(ref doubleTree); return; }
				if (n == 1) { functionX1(ref doubleTree); return; }
				for (int i = 1; i <= n - 1; i++) { functionX0(ref doubleTree); }
				functionX1(ref doubleTree);
				for (int i = 1; i <= n - 1; i++) { functionX0inv(ref doubleTree); }
		}

		static public void functionXninv(ref DoubleTree doubleTree, int n) {//x0^-1 x1^-1 x2^-1...
				if (n == 0) { functionX0inv(ref doubleTree); return; }
				if (n == 1) { functionX1inv(ref doubleTree); return; }
				for (int i = 1; i <= n - 1; i++) { functionX0(ref doubleTree); }
				functionX1inv(ref doubleTree);
				for (int i = 1; i <= n - 1; i++) { functionX0inv(ref doubleTree); }
			}

		#endregion



		//*** Ai,xi,C,D-functions on tuple of integers 0 to 5 **
		#region TupleOnDoubleTree read in traditioanal way
		//applies tuple to doubleTree. i.e. if tuple=100 then doubleTree is changed to x1x0x0(f).
		//NOTICE THAT the Ai-functions ARE APPLIED TO doubleTree FROM RIGHT TO LEFT i.e in traditional way.
		static public void functionA0toA5onTuple(ref byte[] tuple, ref DoubleTree doubleTree) {
			for (int k = tuple.Length - 1; k >= 0; k--) {
				if (tuple[k] == 0) functionA1(ref doubleTree); // functions: 0=A1 1=A2 2=A3 3=A4 4=A5  5=A6
				if (tuple[k] == 1) functionA2(ref doubleTree);
				if (tuple[k] == 2) functionA3(ref doubleTree);
				if (tuple[k] == 3) functionA4(ref doubleTree);
				if (tuple[k] == 4) functionA5(ref doubleTree);
				if (tuple[k] == 5) functionA6(ref doubleTree);
			}

		}
		//overload of previous function TupleOnDoubleTree receiving string instead of array of bytes.
		static public void functionA0toA5onTuple(string tuplestr, ref DoubleTree doubleTree) {
			char[] tuplechar = tuplestr.ToCharArray();
			byte[] tuple = new byte[tuplechar.Length];
			for (int i = 0; i < tuplechar.Length; i++) {
				tuple[i] = (byte)char.GetNumericValue(tuplechar[i]);
			}
			// Console.WriteLine("tuplestr to byte[] is {"+string.Join(",",tuple)+"}");
			functionA0toA5onTuple(ref tuple, ref doubleTree);
		}


		//applies tuple to doubleTree. i.e. if tuple=100 then doubleTree is changed to A1inv A0inv A1inv(doubletree).
		//NOTICE THAT the Ainv functions ARE APPLIED TO doubleTree FROM LEFT TO Right i.e 
		//in traditional way (a1a2)^-1=a2^-1a1^-1 thus you apply first a1inv then a2inv etc
		static public void functionA0invtoA5invOnTuple(ref byte[] tuple, ref DoubleTree doubleTree) {
			for (int k = 0; k < tuple.Length; k++) {
				if (tuple[k] == 0) functionA1inv(ref doubleTree); // functions: 0=A1inv 1=A2inv 2=A3inv 3=A4inv 4=A5inv  5=A6inv
				if (tuple[k] == 1) functionA2inv(ref doubleTree);
				if (tuple[k] == 2) functionA3inv(ref doubleTree);
				if (tuple[k] == 3) functionA4inv(ref doubleTree);
				if (tuple[k] == 4) functionA5inv(ref doubleTree);
				if (tuple[k] == 5) functionA6inv(ref doubleTree);
			}

		}
		//overload of previous function TupleOnDoubleTree receiving string instead of array of bytes.
		static public void functionA0invtoA5invOnTuple(string tuplestr, ref DoubleTree doubleTree) {
			char[] tuplechar = tuplestr.ToCharArray();
			byte[] tuple = new byte[tuplechar.Length];
			for (int i = 0; i < tuplechar.Length; i++) {
				tuple[i] = (byte)char.GetNumericValue(tuplechar[i]);
			}
			// Console.WriteLine("tuplestr to byte[] is {"+string.Join(",",tuple)+"}");
			functionA0invtoA5invOnTuple(ref tuple, ref doubleTree);
		}



		//***CD-functions to tuple of integers 0 to 5  where 0=C 1=D 2=C^-1 3=D^2  4=D^-1**
		//applies tuple to doubleTree. i.e. if tuple=100 then doubleTree is changed to x1x0x0(f).
		static public void functionCDonTuple(ref byte[] tuple, ref DoubleTree doubleTree) {
			for (int k = tuple.Length - 1; k >= 0; k--) {
				if (tuple[k] == 0) functionC(ref doubleTree); // functions:  0=C 1=D 2=C^-1 3=D^2  4=D^-1
				if (tuple[k] == 1) functionD(ref doubleTree);
				if (tuple[k] == 2) functionCinv(ref doubleTree);
				if (tuple[k] == 3) functionDsqr(ref doubleTree);
				if (tuple[k] == 4) functionDinv(ref doubleTree);
			}

		}
		//overload of previous function TupleOnDoubleTree receiving string instead of array of bytes.
		static public void functionCDonTuple(string tuplestr, ref DoubleTree doubleTree) {
			char[] tuplechar = tuplestr.ToCharArray();
			byte[] tuple = new byte[tuplechar.Length];
			for (int i = 0; i < tuplechar.Length; i++) {
				tuple[i] = (byte)char.GetNumericValue(tuplechar[i]);
			}
			// Console.WriteLine("tuplestr to byte[] is {"+string.Join(",",tuple)+"}");
			functionCDonTuple(ref tuple, ref doubleTree);
		}



		//***AB-functions to tuple of integers 0 to 5  where 0=x0 1=x1 2=x0^-1 3=x1^-1 **
		//applies tuple to doubleTree. i.e. if tuple=100 then doubleTree is changed to x1x0x0(f).
		static public void functionX0X1onTuple(ref byte[] tuple, ref DoubleTree doubleTree) {
			for (int k = tuple.Length - 1; k >= 0; k--) {
				if (tuple[k] == 0) functionX0(ref doubleTree); // functions:  0=x0 1=x1 2=x0^-1 3=x1^-1
				if (tuple[k] == 1) functionX1(ref doubleTree);
				if (tuple[k] == 2) functionX0inv(ref doubleTree);
				if (tuple[k] == 3) functionX1inv(ref doubleTree);
			}

		}
		//overload of previous function TupleOnDoubleTree receiving string instead of array of bytes.
		static public void functionX0X1onTuple(string tuplestr, ref DoubleTree doubleTree) {
			char[] tuplechar = tuplestr.ToCharArray();
			byte[] tuple = new byte[tuplechar.Length];
			for (int i = 0; i < tuplechar.Length; i++) {
				tuple[i] = (byte)char.GetNumericValue(tuplechar[i]);
			}
			// Console.WriteLine("tuplestr to byte[] is {"+string.Join(",",tuple)+"}");
			functionX0X1onTuple(ref tuple, ref doubleTree);
		}

		#endregion


		#region DoubleTreeInverse, DoubleTree_DepthOfLeavesStr, DoubleTreeCarets
		//inverse of doubleTrees of T. The doubletree entered gets modified so it is fast
		static public void inverse_ofDoubleTreeT(ref DoubleTree doubleTree) {
			TreeLeavesOfRange(doubleTree.range);//adds links to the domains of the leaves
			doubleTree = new DoubleTree(rangeTree: doubleTree.domain, domainTree: doubleTree.range);
		}
		//TreeLeavesOfRange makes the leaves of the domain point to the range. It is used in DoubleTreeInverse
		static private void TreeLeavesOfRange(TreeNode node) {
			if (node.left == null) { node.sigma.sigma = node; return; }//We have reached a leaf.
			TreeLeavesOfRange(node.left);
			TreeLeavesOfRange(node.right);
		}

		//returns the depth of the leaves of the domain and the range of the double tree (STR means outputs a string)
		static public string DoubleTree_DepthOfLeavesSTR(DoubleTree doubleTree) {
			string depths_range = "";
			string depths_domain = "";
			int[] depthOfLeaves = DoubleTree_DepthOfLeaves(doubleTree);
			int numberofleaves = (depthOfLeaves.Length) / 2;

			for (int i = 0; i < numberofleaves; i++) {
				int depth = depthOfLeaves[i];
				depths_range += depth;
				depth = depthOfLeaves[numberofleaves + i];
				depths_domain += depth;
				if (i + 1 < numberofleaves) { depths_range += ","; depths_domain += ","; }
			}
			return "( " + depths_range + " ) ( " + depths_domain + " )";
		}
		//called by DoubleTree_DepthOfLeavesSTR
		static public int[] DoubleTree_DepthOfLeaves(DoubleTree doubleTree) {
			MyList leaves = new MyList();
			TreeLeaves(doubleTree.range, ref leaves);
			MyNode cur = leaves.First;
			int nrange = 0;
			int ndomain = 0;
			int[] depths = new int[2 * leaves.Length];

			while (cur != null) {
				depths[nrange] = LeafDepth(cur.Value);
				depths[leaves.Length + ndomain] = LeafDepth(cur.Value.sigma);
				++nrange;
				++ndomain;
				cur = cur.Next;
			}

			return depths;
		}
		//called by DoubleTree_DepthOfLeaves
		static public void TreeLeaves(TreeNode node, ref MyList mylist) {
			if (node.left == null) { mylist.AddLast(node); return; }//We have reached a leaf.
			TreeLeaves(node.left, ref mylist);
			TreeLeaves(node.right, ref mylist);
		}
		//called from DoubleTree_DepthOfLeaves
		static public int LeafDepth(TreeNode node) {
			int depth = 0;
			TreeNode cur = node;
			while (cur != null) {
				++depth;
				cur = cur.parent;
			}
			return depth - 1;

		}


		// suppose the range of the double tree looks like:
		//    /\
		//   / /\
		//   1 2 3     the leaves are numbered from left to right starting with 1
		//  (xlist_range[1],ylist_range[1])=(1,3)
		//  (xlist_range[2],ylist_range[2])=(2,3)

		//returns the beginning of the caret of the markers of the range and domain trees
		//it returns {x,y,numberofleaves} from which the carets are (x,numberofleafs), (y,numberofleafs)
		static public int[] DoubleTreeCarets(DoubleTree doubleTree, ref LinkedList<int> xlist_range, ref LinkedList<int> ylist_range, ref LinkedList<int> xlist_domain, ref LinkedList<int> ylist_domain, ref LinkedList<int> sigmaRangeToDomainList) {
			MyList leaves = new MyList();
			TreeLeaves(doubleTree.range, ref leaves);
			MyNode cur = leaves.First;
			int i = 0;
			//asigning 0 1 2 ...n-1 to the leaves of the range of the doubletree
			while (cur != null) {
				cur.Value.leaves = i;
				cur.Value.sigma.leaves = i;//For trees in F this would  be more convenient
				cur = cur.Next;
				i++;
			}

			leaves = new MyList();
			TreeLeaves(doubleTree.domain, ref leaves);
			cur = leaves.First;
			i = 0;
			//asigning 0 1 2 ...n-1 to the leaves of the domain  of the doubletree
			while (cur != null) {
				sigmaRangeToDomainList.AddLast(cur.Value.leaves);
				cur.Value.leaves = i;
				cur = cur.Next;
				i++;
			}



			readTreePreorder_Caret(doubleTree.range, ref xlist_range, ref ylist_range);
			readTreePreorder_Caret(doubleTree.domain, ref xlist_domain, ref ylist_domain);

			//reading the beginning of the caret of the markers of the range forest and domain forest
			//it is assumed it is not the identity
			TreeNode treeNode = doubleTree.range.right;
			int marker_range = -1, marker_domain = -1;
			while (treeNode != null) {//finding the last left leaf from the current node;
				if (treeNode.left == null) marker_range = treeNode.leaves;//treenode.leaves is the n-th leaf of the tree.
				treeNode = treeNode.left;
			}

			treeNode = doubleTree.domain.right;
			while (treeNode != null) {//finding the last left leaf from the current node;
				if (treeNode.left == null) marker_domain = treeNode.leaves;//treenode.leaves is the n-th leaf of the tree.
				treeNode = treeNode.left;
			}
			return new int[] { marker_range, marker_domain, leaves.Length };

		}

		//This function is called from DoubleTreeCarets. It is assumed that the leaf number is saved in the int treenode.leaves;
		static public void readTreePreorder_Caret(TreeNode node, ref LinkedList<int> xlist, ref LinkedList<int> ylist) {
			if (node == null) return;
			if (node.left != null) {// if the node is not a leaf 
				int x = 0, y = 0;
				TreeNode tempnode = node;
				//finding the last left leaf from the current node;
				while (tempnode != null) {
					if (tempnode.left == null) x = tempnode.leaves;//tempnode.leaves is the n-th leaf of the tree.
					tempnode = tempnode.left;
				}
				//finding the last right leaf from the current node;
				tempnode = node;
				while (tempnode != null) {
					if (tempnode.right == null) y = tempnode.leaves;//tempnode.leaves is the n-th leaf of the tree.
					tempnode = tempnode.right;
				}
				//note if x or y are zero then something went wrong
				xlist.AddLast(x);
				ylist.AddLast(y);
			}
			readTreePreorder_Caret(node.left, ref  xlist, ref ylist);
			readTreePreorder_Caret(node.right, ref  xlist, ref  ylist);
			return;
		}


		#endregion

		#region serialization  DoubleTreeToDoubleStringBin





		//Serializes a doubleTree together with its inverse:
		//1)doubleTrees in F: rangeTree+'z'+domainTree, where the trees are encoded in basetogether with its inverse in base64: '0' - 'o'
		//The root is not encoded.
		//If the double tree is in T then 'z' 
		static public void DoubleTreeToStringAndinvStringBin(DoubleTree doubleTree, ref string s, ref string sInverse) {


			TreeNode cur = doubleTree.range;
			TreeNode domainSigma = null;
			while (cur != null) {// First leaf in range maps to domainSigma 
				domainSigma = cur.sigma;
				cur = cur.left;
			}

			string p = "";//p is the path of the leaf sigma(0) 
			while (domainSigma.parent != null) {//we do not count the root.
				if (domainSigma.currentNodeIsTheLeftChild == true)
					p = "0" + p;
				else
					p = "1" + p;
				domainSigma = domainSigma.parent;
			}
			p = p.TrimEnd('0');
			//Console.WriteLine("domain: p= " + p);
			if (p != "") p = "z" + base4toBase64(base2toBase4(p));//if p="" then the element is in F and z is not recorded.  base64: '0' - 'o'


			TreeNode domainFirstLeaf = null;
			cur = doubleTree.domain;
			while (cur != null) {// First leaf in domain is found
				domainFirstLeaf = cur;
				cur = cur.left;
			}


			TreeNode rangeSigma = null;
			string r = TreeToStringNewWithInversePointer(doubleTree.range, domainFirstLeaf, ref rangeSigma); //domainSigma stores first leaf of domain


			string q = "";
			while (rangeSigma.parent != null) {//we do not count the root.
				if (rangeSigma.currentNodeIsTheLeftChild == true)
					q = "0" + q;
				else
					q = "1" + q;
				rangeSigma = rangeSigma.parent;
			}
			q = q.TrimEnd('0');
			//Console.WriteLine("range: q= " + q);
			if (q != "") q = "z" + base4toBase64(base2toBase4(q));


			string d = TreeToString(doubleTree.domain);

			s = base4toBase64(base2toBase4(r.TrimEnd('0'))) + "z" + base4toBase64(base2toBase4(d.TrimEnd('0'))) + p;
			sInverse = base4toBase64(base2toBase4(d.TrimEnd('0'))) + "z" + base4toBase64(base2toBase4(r.TrimEnd('0'))) + q;//domain and range are swapped with range carrying the pointer

		}

		//T::functionCDonTuple 0=C 1=D 2=C^-1 3=D^2  4=D^-1

		static public void test() {
			DoubleTree dt = new DoubleTree();
			functionCDonTuple("010", ref dt);
			
			Console.WriteLine(Draw(dt));
			DoubleTree dt1 = reflect_doubleTreeT(dt);
			Console.WriteLine(Draw(dt1));

			DoubleTree dt2 = reverseInverse_ofdoubleTreeT(dt);
			Console.WriteLine(Draw(dt2));

			Console.ReadKey();
		}


		static public void DoubleTreeToStringAndinvString(DoubleTree doubleTree, ref string s, ref string sInverse) {


			TreeNode cur = doubleTree.range;
			TreeNode domainSigma = null;
			while (cur != null) {// First leaf in range maps to domainSigma 
				domainSigma = cur.sigma;
				cur = cur.left;
			}

			string p = "";//p is the path of the leaf sigma(0) 
			while (domainSigma.parent != null) {//we do not count the root.
				if (domainSigma.currentNodeIsTheLeftChild == true)
					p = "0" + p;
				else
					p = "1" + p;
				domainSigma = domainSigma.parent;
			}
			p = p.TrimEnd('0');
			//Console.WriteLine("domain: p= " + p);
			if (p != "") p = "z" + p;//if p="" then the element is in F and z is not recorded.  base64: '0' - 'o'


			TreeNode domainFirstLeaf = null;
			cur = doubleTree.domain;
			while (cur != null) {// First leaf in domain is found
				domainFirstLeaf = cur;
				cur = cur.left;
			}


			TreeNode rangeSigma = null;
			string r = TreeToStringNewWithInversePointer(doubleTree.range, domainFirstLeaf, ref rangeSigma); //domainSigma stores first leaf of domain


			string q = "";
			while (rangeSigma.parent != null) {//we do not count the root.
				if (rangeSigma.currentNodeIsTheLeftChild == true)
					q = "0" + q;
				else
					q = "1" + q;
				rangeSigma = rangeSigma.parent;
			}
			q = q.TrimEnd('0');
			//Console.WriteLine("range: q= " + q);
			if (q != "") q = "z" + q;


			string d = TreeToString(doubleTree.domain);

			s = r + "z" + d + p;
			sInverse = d + "z" + r+ q;//domain and range are swapped with range carrying the pointer

		}

		static public void DoubleTreeToStringAndinvStringVSLOW(DoubleTree doubleTree, ref string s, ref string sInverse) {
			string sn="",si="";
			DoubleTreeToStringAndinvString(doubleTree, ref sn, ref si);
			
			string r = TreeToString(doubleTree.range);
			string d = TreeToString(doubleTree.domain);
			string sigma = strSigmaV(doubleTree);
			string sigmainv = strSigmaV(StringToDoubleTreeT(si));
			s=r+"z"+d+"z"+sigma;
			sInverse = d + "z" + r + "z" + sigmainv;
		}


		//T:: can be improved by directly read string and not convert it to tree first.
		static public DoubleTree reflect_doubleTreeT(string str) {
			return reflect_doubleTreeT(StringToDoubleTreeT(str));
		}


		static public DoubleTree reflect_doubleTreeT(DoubleTree dt_unchanged) {
			string doubleTreeReflected = treeToStringReflected(dt_unchanged.range) + 'z' + treeToStringReflected(dt_unchanged.domain) +
				'z' + strPathReflectedOfLeafNode(lastLeafNode(dt_unchanged.range).sigma);
			//reversedOrder(readSigma(doubleTree));
			return StringToDoubleTreeT(doubleTreeReflected);
		}

		//T:: reverseinverse(l1 l2 l3 l4 ) = l1^-1 l2^-1 l3^-1 l4^-1 where li is C or D . To reflect it needs to serialize it first so it leaves dt unchcahgned which makes it slower. it also assumes w=w^-1
		static public DoubleTree reverseInverse_ofSelfAdjointdoubleTreeT(DoubleTree dt_unchanged) {
			DoubleTree reverseInv_dt=reflect_doubleTreeT(dt_unchanged);
			functionDsqr(ref reverseInv_dt);
			inverse_ofDoubleTreeT(ref reverseInv_dt);
			functionDsqr(ref reverseInv_dt);

			return reverseInv_dt;

		}

		//T:: reverseinverse(l1 l2 l3 l4 ) = l1^-1 l2^-1 l3^-1 l4^-1 where li is C or D . To reflect it needs to serialize it first so it leaves dt unchcahgned which makes it slower.

		static public DoubleTree reverseInverse_ofdoubleTreeT(DoubleTree dt_unchanged) {
			DoubleTree reversei_dt = reflect_doubleTreeT(dt_unchanged);//w
			inverse_ofDoubleTreeT(ref reversei_dt);//w^-1
			functionDsqr(ref reversei_dt);// D^2 w^-1
			inverse_ofDoubleTreeT(ref reversei_dt);// w D^2
			functionDsqr(ref reversei_dt);// D^2 w D^2

			return reversei_dt;

		}



		static public string strSigmaV(DoubleTree doubleTree) {
			return string.Join(",",readSigmaV(doubleTree));
		}

		//V::returns the permutation sigma:rangeLeaves->domainLeaves as an array of integers  i.e. if the array starts with n then sigmaPermutation(0 leaf)= n leaf;
		//it  uses node.leaves.
		static public int[] readSigmaV(DoubleTree doubleTree) {
			MyList leavesRange = new MyList();
			MyList leavesDomain = new MyList();
			LeavesV_copySigma(doubleTree.range, ref leavesRange);
			LeavesV(doubleTree.domain, ref leavesDomain);

			MyNode cur = leavesDomain.First;
			int i = 0;
			while (cur != null) {
				cur.Value.sigma.leaves = i++;
				cur = cur.Next;
			}
			i = 0;
			cur = leavesRange.First;
			int[] sigma_array = new int[leavesRange.Length];
			while (cur != null) {
				sigma_array[i++] = cur.Value.leaves;
				cur = cur.Next;
			}
			return sigma_array;
		}

		//V::Creates a list with the leafs of a nonempty tree (i.e. at least the identity).
		static private void LeavesV(TreeNode node, ref MyList leaves_list) {
			if (node.left == null) { leaves_list.AddLast(node); return; }// a leaf has been found
			LeavesV(node.left, ref leaves_list);
			LeavesV(node.right, ref leaves_list);
		}
		//V::Assuming a doubleTree here. Good for inverses. node=doubleTree.range. Creates a list with leafs of a nonempty tree and copies sigma to the leaves of domain (i.e. at least the idenitity).
		static private void LeavesV_copySigma(TreeNode node, ref MyList leaves_list) {
			if (node.left == null) { leaves_list.AddLast(node); node.sigma.sigma = node; return; }//We have reached a leaf.
			LeavesV_copySigma(node.left, ref leaves_list);
			LeavesV_copySigma(node.right, ref leaves_list);
		}



		//T::The three is read postorder (e.g. from right to left and not from left to right as usual)
		static private string treeToStringReflected(TreeNode node) {
			if (node == null) return "";
			if (node.left == null) return "0";//We have reached a leaf.
			return "1" + treeToStringReflected(node.right) + treeToStringReflected(node.left);
		}


		//T: returns the node of the last leaf of the tree. The tree has to be at least the identity.
		static private TreeNode lastLeafNode(TreeNode node) {
			TreeNode curnode = node;
			while (curnode.right != null) {
				curnode = curnode.right;
			}
			return curnode;
		}

		//T: returns the node of the first leaf of the tree. The tree has to be at least the identity.
		static private TreeNode firstLeafNode(TreeNode node) {
			TreeNode curnode = node;
			while (curnode.left != null) {
				curnode = curnode.left;
			}
			return curnode;
		}


		//T::returns the path (=a string of 0's and 1's) from the current node up to the root (which is not recorded) 0=leftedge 1=rightedge
		static public string strPathOfLeafNode(TreeNode node) {
			TreeNode curnode = node;
			string str = "";
			while (curnode.parent != null) {
				if (curnode.currentNodeIsTheLeftChild == true) str = "0" + str; else str = "1" + str;
				curnode = curnode.parent;
			}
			return str;
		}

		//T::returns the path (=a string of 0's and 1's) from the current node up to the root (which is not recorded) 1=leftedge 0=rightedge
		static public string strPathReflectedOfLeafNode(TreeNode node) {
			TreeNode curnode = node;
			string str = "";
			while (curnode.parent != null) {
				if (curnode.currentNodeIsTheLeftChild == true) str = "1" + str; else str = "0" + str;
				curnode = curnode.parent;
			}
			return str;
		}


		static public void DoubleTreeToDoubleStringBinCompareWithSoren(DoubleTree doubleTree, ref string s, ref string sInverse) {


			TreeNode cur = doubleTree.range;
			TreeNode domainSigma = null;
			while (cur != null) {// First leaf in range maps to domainSigma 
				domainSigma = cur.sigma;
				cur = cur.left;
			}

			string p = "";//p is the path of the leaf sigma(0) 
			while (domainSigma.parent != null) {//we do not count the root.
				if (domainSigma.currentNodeIsTheLeftChild == true)
					p = "0" + p;
				else
					p = "1" + p;
				domainSigma = domainSigma.parent;
			}
			p = p.TrimEnd('0');
			//Console.WriteLine("domain: p= " + p);
			if (p != "") p = "z" + base4toBase64(base2toBase4(p));//if p="" then the element is in F and z is not recorded.  base64: '0' - 'o'


			TreeNode domainFirstLeaf = null;
			cur = doubleTree.domain;
			while (cur != null) {// First leaf in domain is found
				domainFirstLeaf = cur;
				cur = cur.left;
			}


			TreeNode rangeSigma = null;
			string r = TreeToStringNewWithInversePointer(doubleTree.range, domainFirstLeaf, ref rangeSigma); //domainSigma stores first leaf of domain


			string q = "";
			while (rangeSigma.parent != null) {//we do not count the root.
				if (rangeSigma.currentNodeIsTheLeftChild == true)
					q = "0" + q;
				else
					q = "1" + q;
				rangeSigma = rangeSigma.parent;
			}
			q = q.TrimEnd('0');
			//Console.WriteLine("range: q= " + q);
			if (q != "") q = "z" + base4toBase64(base2toBase4(q));


			string d = TreeToString(doubleTree.domain);

			s = ((r.TrimEnd('0'))) + "z" + ((d.TrimEnd('0'))) + p;
			sInverse = ((d.TrimEnd('0'))) + "z" + ((r.TrimEnd('0'))) + q;//domain and range are swapped with range carrying the pointer

		}


		// Serialize doubleTree to string: rangeTree+' '+domainTree
		// the serialization is by preorder tree traversal , where 0=node of tree, 1= leaf of tree, 3="sigma(0) points to this leaf"
		static public string DoubleTreeToString_Format01Sigma0(DoubleTree doubleTree) {
			string r = "", d = "";
			r = TreeToString(doubleTree.range);

			TreeNode cur = doubleTree.range;
			TreeNode domainSigma = null;
			while (cur != null) {// First leaf in range maps to domainSigma 
				domainSigma = cur.sigma;
				cur = cur.left;
			}
			d = TreeToStringNewWithPointer(doubleTree.domain, domainSigma);
			//Console.WriteLine(r.TrimEnd('0') + "3" + d.TrimEnd('0'));
			//			Console.WriteLine(r + "3" + d);
			//3 = |
			//return base4toBase64(r.TrimEnd('0') + "3" + d.TrimEnd('0'));
			return r + " " + d;
		}

		// Serialize doubleTree to string: rangeTree+'3'+domainTree and converted to base64
		// the serialization is by preorder tree traversal , where 0=node of tree, 1= leaf of tree, 3="sigma(0) points to this leaf"
		// Note: first 3 means division of range and  domain tree; second 3 means "sigma(0) points to this leaf".
		static public string DoubleTreeToStringBin(DoubleTree doubleTree) {
			string r = "", d = "";
			r = TreeToString(doubleTree.range);

			TreeNode cur = doubleTree.range;
			TreeNode domainSigma = null;
			while (cur != null) {// First leaf in range maps to domainSigma 
				domainSigma = cur.sigma;
				cur = cur.left;
			}
			d = TreeToStringNewWithPointer(doubleTree.domain, domainSigma);
			//Console.WriteLine(r.TrimEnd('0') + "3" + d.TrimEnd('0'));
//			Console.WriteLine(r + "3" + d);
			//3 = |
			return base4toBase64(r.TrimEnd('0') + "3" + d.TrimEnd('0'));
			//return r + "|3|" + d;
		}

		static public string DoubleTreeToDoubleStringBinOnFfirstHalf(DoubleTree doubleTree) {
			string r = TreeToString(doubleTree.range);
			string d = TreeToString(doubleTree.domain);

			if (string.CompareOrdinal(r, d) > 0) {//r>d
				return "";// base4toBase64(base2toBase4(d.TrimEnd('0'))) + "z" + base4toBase64(base2toBase4(r.TrimEnd('0')));
			}
			//r<=d
			return base4toBase64(base2toBase4(r.TrimEnd('0'))) + "z" + base4toBase64(base2toBase4(d.TrimEnd('0')));
		}

		// Serialize doubleTree to string
		static public string DoubleTreeToForestToString(DoubleTree doubleTree) {
			TreeNode leftmostLeaf = null;
			string r = TreeToForestToString(doubleTree.range, ref leftmostLeaf);
			string d = TreeToForestToStringWithPointer(doubleTree.domain, leftmostLeaf.sigma);
			//3 = |    
			//return base4toBase64(r+ "3" + d);//first 3 means pointer of range, second 3 means starting range, if third 3 is single (i.e. no 33) then it means the range.sigma is on the leftside of the domain of the pointer. 33 means right side of the domain of the pointer.
			return r + "|" + d;
		}

		static public string DoubleTreeToInverseToForestToString(DoubleTree doubleTree) {
			TreeNode leftmostLeaf = null;
			string d = TreeToForestToString(doubleTree.domain, ref leftmostLeaf);
			string r=TreeToForestToStringWithInversePointer(doubleTree.range,leftmostLeaf);
			//3 = |
			return base4toBase64(d + "3" + r);//domain and range are swapped with range carrying the pointer
			//return d + "|" + r;
		}

		//called from DoubleTreeToForestToString and DoubleTreeToInverseToForestToString
		static private string TreeToForestToString(TreeNode tree, ref TreeNode leftmostLeaf) {
			string r = "";
			//a tree is seen as a sequence of subtrees along the main arc /\ of the tree. A trivial subtree is a leaf.
			//reading left side of the range.
			//READING THE LEFT SIDE OF THE TREE
			TreeNode cur = tree.left;//tree cannot be null. it has to be at least the identity.
			leftmostLeaf = tree;
			while (cur != null) {//if range is a leaf the loop does nothing. if range is a caret the loop gives nothing. first leaf is not recorded
				if (cur.left != null) { //cur is not the leftmost leaf.
					if (cur.right.left == null) //tree is of depth 0  so a 2=, is written
						r = "2" + r;
					else
						r = TreeToString(cur.right).TrimEnd('0') + "2" + r;
				}
				leftmostLeaf = leftmostLeaf.left;
				cur = cur.left;
			}
			//READING THE RIGHT SIDE OF THE TREE
			cur = tree.right;
			r = r + "3";//adding the pointer at 1/2
			while (cur != null) {//if range is a leaf the loop does nothing. if range is a caret the loop gives nothing. last leaf is not recorded
				if (cur.right != null) {// cur is not the rightmost leaf
					if (cur.left.left == null) //tree is of depth 0  so a 2=, is written
						r = r + "2";
					else
						r = r + TreeToString(cur.left).TrimEnd('0') + "2";
				}
				cur = cur.right;
			}//format for range: tree1, tree2, tree3, | tree 4, tree5  where ,=2 and |=3.
			//r = r.Trim('2');// remove trivial trees from left and right. CANNOT DO THIS! Then A2 and A4 have same forest representation!
			//if range=id: r=|
			//if range=caret: r=|
			//if range=vine: r=|
			return r;
		}

		static private string TreeToForestToStringWithPointer(TreeNode tree, TreeNode sigma) {
			string d = "";
			//a tree is seen as a sequence of subtrees along the main arc /\ of the tree. A trivial subtree is a leaf.
			//reading left side of the range.
			//READING THE LEFT SIDE OF THE TREE
			TreeNode cur = tree.left;//tree cannot be null. it has to be at least the idenity.
			while (cur != null) {//if range is a leaf the loop does nothing. if range is a caret the loop gives nothing. first leaf is not recorded
				if (cur.left != null) { //cur is not the leftmost leaf.
					if (cur.right.left == null) { //tree is of depth 0  so a 2=, is written
						if (cur.right == sigma) d = "32" + d;
						else
							d = "2" + d;
					}
					else
						d = TreeToStringNewWithPointer(cur.right, sigma).TrimEnd('0') + "2" + d;
				}
				else {
					if (cur == sigma) d = "32" + d;
				}
				cur = cur.left;
			}//if the pointer was found, then it will be before a 2. 
			//READING THE RIGHT SIDE OF THE RANGE
			cur = tree.right;
			d = d + "33";//adding the pointer at 1/2 
			while (cur != null) {//if range is a leaf the loop does nothing. if range is a caret the loop gives nothing. last leaf is not recorded
				if (cur.right != null) {// cur is not the rightmost leaf
					if (cur.left.left == null) { //tree is of depth 0  so a 2=, is written
						if (cur.left == sigma) d = d + "32";
						else
							d = d + "2";
					}
					else
						d = d + TreeToStringNewWithPointer(cur.left, sigma).TrimEnd('0') + "2";
				}
				else {
					if (cur == sigma) d = d + "32";
				}
				cur = cur.right;
			}//if a pointer is found on the righthandside then if a 333 occur then the first 33 mean the pointer at 1/2 and the other tree means sigma
			//format for range: tree1, tree2, tree3, | tree 4, tree6  where ,=2 and |=3.
			
			//if range=id: r=|
			//if range=caret: r=|
			//if range=vine: r=|
			return d;
		}

		static private string TreeToForestToStringWithInversePointer(TreeNode tree, TreeNode domainSigma) {
			string r = "";
			//a tree is seen as a sequence of subtrees along the main arc /\ of the tree. A trivial subtree is a leaf.
			//READING THE LEFT SIDE OF THE TREE
			
			TreeNode cur = tree.left;//tree cannot be null. it has to be at least the idenity.
			while (cur != null) {//if range is a leaf the loop does nothing. if range is a caret the loop gives nothing. first leaf is not recorded
				if (cur.left != null) { //cur is not the leftmost leaf.
					if (cur.right.left == null) {//tree is of depth 0  so a 2=, is written
						if (cur.right.sigma == domainSigma) r = "32" + r;
						else
							r = "2" + r;
					}
					else
						r = TreeToStringNewWithInversePointer(cur.right, domainSigma).TrimEnd('0') + "2" + r;
				}
				else {//cur is the leftmost leaf;
					if (cur.sigma == domainSigma) r = "32" + r;
				}
				cur = cur.left;
			}//if the pointer was found, then it will be before a 2. 
			//READING THE RIGHT SIDE OF THE RANGE
			cur = tree.right;
			r = r + "33";//adding the pointer at 1/2 
			while (cur != null) {//if range is a leaf the loop does nothing. if range is a caret the loop gives nothing. last leaf is not recorded
				if (cur.right != null) {// cur is not the rightmost leaf
					if (cur.left.left == null) { //tree is of depth 0  so a 2=, is written
						if (cur.left.sigma == domainSigma) r = r + "32";
						else
							r = r + "2";
					}
					else
						r = r + TreeToStringNewWithInversePointer(cur.left, domainSigma).TrimEnd('0') + "2";
				}
				else {
					if (cur.sigma == domainSigma) r = r + "32";
				}
				cur = cur.right;
			}//if a pointer is found on the righthandside then if a 333 occur then the first 33 mean the pointer at 1/2 and the other tree means sigma
			//format for range: tree1, tree2, tree3, || tree 4, tree5  where ,=2 and |=3.

			//if range=id: r=|
			//if range=caret: r=|
			//if range=vine: r=|
			return r;
		}
		 

		// Serialize the inverse of the given doubleTree to string
		static public string DoubleTreeToInverseToString(DoubleTree doubleTree) {

			TreeNode domainFirstLeaf = null;
			TreeNode cur = doubleTree.domain;
			while (cur != null) {// First leaf in domain is domainSigma
				domainFirstLeaf = cur;
				cur = cur.left;
			}

			string r = "", d = "";
			r = TreeToStringNewWithInversePointer(doubleTree.range, domainFirstLeaf); //domainSigma stores first leaf of domain
		
			d = TreeToString(doubleTree.domain);
			//Console.WriteLine(d.TrimEnd('0') + "3" + r.TrimEnd('0'));
			//Console.WriteLine(d + "3" + r);
			//3 = |
			return base4toBase64(d.TrimEnd('0') + "3" + r.TrimEnd('0'));//domain and range are swapped with range carrying the pointer
			//return d + "|3|" + r;
		}

		static private string TreeToString(TreeNode node) {
			if (node == null) return "";
			if (node.left == null) return "0";//We have reached a leaf.
			return "1" + TreeToString(node.left) + TreeToString(node.right);
		}



	
		//reads a tree and saves the trees along the main left caret from center to left, and then 
		//the trees from the main right caret from center to right.
		//it is good for any tree of  a doubleTree (F,T,V).
		static private string TreeToForestToString(TreeNode treeNode) {
			if (treeNode == null) return "";
			//tree is at least the identity
			if (treeNode.left == null) return "6";
			//tree is not a leaf
			return TreeToForestAlongLeftSideToString(treeNode) + "." + TreeToForestAlongRightSideToString(treeNode);
		}

		//called from TreeToForestToString
		//reads the branches along the left main caret (reads from center to left and ignores the first tree which corresponds to the right side_
		static private string TreeToForestAlongLeftSideToString(TreeNode node) {
			if (node.left == null) return "7";//We have reached the first leaf of the tree.
			return "2" + TreeToString(node.left.right) + TreeToForestAlongLeftSideToString(node.left);
		}

		//called from TreeToForestToString
		//reads the branches along the right main caret (reads from the center to the right and ignores the first tree which corresponds to the left side)
		static private string TreeToForestAlongRightSideToString(TreeNode node) {
			if (node.left == null) return "8";//We have reached the last leaf of the tree.
			return "2" + TreeToString(node.right.left) + TreeToForestAlongRightSideToString(node.right);
		}


		static private string TreeToStringNewWithPointer(TreeNode node, TreeNode Pointer) {
			if (node.left == null) {//We have reached a leaf.
				if (node == Pointer) return "3";//The first leaf in range maps to Pointer in domain where a 3 is written instead of a 0.
				return "0";
			}

			return "1" + TreeToStringNewWithPointer(node.left, Pointer) + TreeToStringNewWithPointer(node.right, Pointer);
		}

		//overload:it is called with the range tree, firstleaf of domain and returns sigma.domain
		static private string TreeToStringNewWithInversePointer(TreeNode node, TreeNode firstLeaf, ref TreeNode sigmaOfFirstLeaf) {
			if (node.left == null) {//We have reached a leaf.
				if (node.sigma == firstLeaf) sigmaOfFirstLeaf=node;//The first leaf in domain maps to current node in range where a 3 is written instead of a 0.
				return "0";
			}

			return "1" + TreeToStringNewWithInversePointer(node.left, firstLeaf,ref sigmaOfFirstLeaf) + TreeToStringNewWithInversePointer(node.right, firstLeaf,ref sigmaOfFirstLeaf);
		}

		static private string TreeToStringNewWithInversePointer(TreeNode node,  TreeNode domainSigma) {
			if (node.left == null) {//We have reached a leaf.
				if (node.sigma == domainSigma) return "3";//The first leaf in domain maps to current node in range where a 3 is written instead of a 0.
				return "0";
			}

			return "1" + TreeToStringNewWithInversePointer(node.left, domainSigma) + TreeToStringNewWithInversePointer(node.right, domainSigma);
		}

		#endregion

		#region deserialization StringBinToDoubleTree

		//str=serialized range in base64 + 'z' + serialized domain in base 64 +('z'+path of sigma(0) in domain)
		//its inverse is the function DoubleTreeToStringBin
		static public DoubleTree StringBinToDoubleTreeT(string str) {
			string[] words = str.Split('z');
			string r = base4toBase2(base64toBase4(words[0]));//need to convert to base 4 and then to base 2;
			string d = base4toBase2(base64toBase4(words[1]));
			string p = "";

			if (words.Length == 3) { p = base4toBase2(base64toBase4(words[2])); }
			//Console.WriteLine("r=" + words[0] + " d=" + words[1] + " p=" + p);
			//Console.WriteLine("r=" + r + " d=" + d + " p=" + p);
			MyList leaves = new MyList();
			TreeNode domain = null;
			StringToTreeReadLeafs(node: ref domain, str: ref d, myList_leafs: ref leaves);
			TreeNode pointer = null, cur = domain;
			int pos = 0;
			while (cur != null) {
				pointer = cur;
				char ch = '0';
				if (pos < p.Length) ch = p[pos];
				pos++;
				if (ch == '0') cur = cur.left;
				else cur = cur.right;

			}
			//Console.WriteLine("nUMBER OF LEAVES " + leaves.Length);
			//MyNode myNode = leaves.First;
			//for (int i = 0; i < leaves.Length;i++ ) {
			//	Console.WriteLine(""+i+" "+myNode.Value.currentNodeIsTheLeftChild);
			//	myNode=myNode.Next;
			//}

			leaves.NewBeginning(pointer);
			TreeNode range = null;
			StringToTreeWriteLeafs(node: ref range, str: ref r, myList_leafs: ref leaves);

			//Console.ReadKey();

			return new DoubleTree(rangeTree: range, domainTree: domain);

		}

		// Deserialize tree from string. It saves the leafs in the myList_leafs list.
		static private int StringToTreeReadLeafs(ref TreeNode node, ref string str, ref MyList myList_leafs, int pos = 0, TreeNode parent = null, bool currentNodeIsTheLeftChild = true) {
			char ch = '0'; // since zeros may have been trimmed from the end, we assume zero, if we reached the end of string
			if (pos < str.Length) ch = str[pos];
			pos++;
			node = new TreeNode(parentNode: parent, currentNodeIsTheLeftChild: currentNodeIsTheLeftChild);
			if (ch == '1') {
				pos = StringToTreeReadLeafs(node: ref node.left, str: ref str, myList_leafs: ref myList_leafs, pos: pos, parent: node, currentNodeIsTheLeftChild: true);
				pos = StringToTreeReadLeafs(node: ref node.right, str: ref str, myList_leafs: ref myList_leafs, pos: pos, parent: node, currentNodeIsTheLeftChild: false);
			}
			else {
				myList_leafs.AddLast(node);
			}
			return pos;
		}



		// Deserialize tree from string. It writes the leafs in the myList_leafs  to the sigmaleafs of the tree.
		static private int StringToTreeWriteLeafs(ref TreeNode node, ref string str, ref MyList myList_leafs, int pos = 0, TreeNode parent = null, bool currentNodeIsTheLeftChild = true) {
			char ch = '0'; // since zeros may have been trimmed from the end, we assume zero, if we reached the end of string
			if (pos < str.Length) ch = str[pos];
			pos++;
			if (ch == '0') {
				TreeNode sigmaNode = myList_leafs.ReadNode();
				node = new TreeNode(parentNode: parent, currentNodeIsTheLeftChild: currentNodeIsTheLeftChild, sigmaNode: sigmaNode);
				//if (sigmaNode == null) Console.WriteLine("FOUND NULL AT STRINGTOTREEwriteLEAF"); else Console.WriteLine("SO FAR SO GOOD" + currentNodeIsTheLeftChild);

			}
			else
				node = new TreeNode(parentNode: parent, currentNodeIsTheLeftChild: currentNodeIsTheLeftChild);
			if (ch == '1') {
				pos = StringToTreeWriteLeafs(node: ref node.left, str: ref str, myList_leafs: ref myList_leafs, pos: pos, parent: node, currentNodeIsTheLeftChild: true);
				pos = StringToTreeWriteLeafs(node: ref node.right, str: ref str, myList_leafs: ref myList_leafs, pos: pos, parent: node, currentNodeIsTheLeftChild: false);
			}
			return pos;
		}

		//T::
		static public DoubleTree StringToDoubleTreeT(string str) {
			string[] words = str.Split('z');
			string r = words[0];
			string d = words[1];
			string p = "";

			if (words.Length == 3) { p = words[2]; }
			//Console.WriteLine("r=" + words[0] + " d=" + words[1] + " p=" + p);
			//Console.WriteLine("r=" + r + " d=" + d + " p=" + p);
			MyList leaves = new MyList();
			TreeNode domain = null;
			StringToTreeReadLeafs(node: ref domain, str: ref d, myList_leafs: ref leaves);
			TreeNode pointer = null, cur = domain;
			int pos = 0;
			while (cur != null) {
				pointer = cur;
				char ch = '0';
				if (pos < p.Length) ch = p[pos];
				pos++;
				if (ch == '0') cur = cur.left;
				else cur = cur.right;

			}
			//Console.WriteLine("nUMBER OF LEAVES " + leaves.Length);
			//MyNode myNode = leaves.First;
			//for (int i = 0; i < leaves.Length;i++ ) {
			//	Console.WriteLine(""+i+" "+myNode.Value.currentNodeIsTheLeftChild);
			//	myNode=myNode.Next;
			//}

			leaves.NewBeginning(pointer);
			TreeNode range = null;
			StringToTreeWriteLeafs(node: ref range, str: ref r, myList_leafs: ref leaves);

			//Console.ReadKey();

			return new DoubleTree(rangeTree: range, domainTree: domain);

		}

		#endregion

		#region Leaf Labels, Length Function on F,




		// Leaf labels of a DoubleTree
		static public int DoubleTreeLength(DoubleTree doubleTree) {
			string r = TreeLeafLabelsAsForest(doubleTree.range);
			string d = TreeLeafLabelsAsForest(doubleTree.domain);
			if (r == "") return 0;// doubleTree is the identity
			//The second leftmost leaf is not used in the labelling because that corresponds to a gap outside the support of the forest.
			r = r.Substring(1);
			d = d.Substring(1);

			//endtrimming the common 'R's of r and d. 
			int minLength = r.Length < d.Length ? r.Length : d.Length;
			int counter = 0;			
			for (int i = 0; i < minLength; i++) {
				//Console.WriteLine("r=" + r + " d=" + minLength+"counter=" +counter);
				if (r[r.Length-1 - counter] != 'R' || d[r.Length-1 - counter] != 'R') break;
				++counter;
			}
			if (counter > 0 && counter < minLength) { r = r.Substring(0, r.Length - counter); d = d.Substring(0, d.Length - counter); }

			return WordLength(r, d);


		}

		// Leaf labels of a DoubleTree used to compute the length
		static public string DoubleTreeLeafLabels_length(DoubleTree doubleTree) {
			string r = TreeLeafLabelsAsForest(doubleTree.range);
			string d = TreeLeafLabelsAsForest(doubleTree.domain);
			if (r == "") return "";// doubleTree is the identity
			//The second leftmost leaf is not used in the labelling because that corresponds to a gap outside the support of the forest.
			r = r.Substring(1);
			d = d.Substring(1);

			//endtrimming the common 'R's of r and d. 
			int minLength = r.Length < d.Length ? r.Length : d.Length;
			int counter = 0;
			for (int i = 0; i < minLength; i++) {
				//Console.WriteLine("r=" + r + " d=" + minLength+"counter=" +counter);
				if (r[r.Length - 1 - counter] != 'R' || d[r.Length - 1 - counter] != 'R') break;
				++counter;
			}
			if (counter > 0 && counter < minLength) { r = r.Substring(0, r.Length - counter); d = d.Substring(0, d.Length - counter); }

			return r + "\n" + d + "\n" + WordColumns_numbers(r, d);

		}

		static public string DoubleTreeLeafLabels(DoubleTree doubleTree) {
			string r = TreeLeafLabelsAsForest(doubleTree.range);
			string d=TreeLeafLabelsAsForest(doubleTree.domain);
			if (r.Length > 0 && (r[r.Length - 1] != 'R' || d[d.Length-1]!='R')) {//we assume r and d are of same length
				r += 'R';
				d += 'R';
			}
			string str0to9 = "";
			for (int i = 0; i < r.Length; i++) str0to9 += (i % 10);
			return str0to9 + "\n" + r + "\n" + d + "\n" + WordColumns_numbers(r, d);
		}

		static private string WordColumns_numbers(string r, string d) {
			int[,] mtx = new int[,] { { 2, 4, 2, 1, 3 }, { 4, 4, 2, 3, 3 }, { 2, 2, 2, 1, 1 }, { 1, 3, 1, 2, 2 }, { 3, 3, 1, 2, 2 } };
			string wordcolums_numbers = "";

			for (int i = 0; i < r.Length; i++) {
				wordcolums_numbers += mtx[GapLabels(r[i]), GapLabels(d[i])];
			}

			return wordcolums_numbers;
		}


		static private int WordLength(string r, string d) {
			int[,] mtx=new int[,] {{2,4,2,1,3},{4,4,2,3,3},{2,2,2,1,1},{1,3,1,2,2},{3,3,1,2,2}};
			int length = 0;

			for (int i = 0; i < r.Length; i++) {
				length += mtx[GapLabels(r[i]), GapLabels(d[i])];
			}

				return length;
		}
		static private int GapLabels(char ch) {
			switch (ch) {
				case 'I': return 0;
				case 'N': return 1;
				case 'L': return 2;
				case 'R': return 3;
				case 'X': return 4;					
			}
			return -1;
		}


		static public void TreeLeafLabels(TreeNode node, ref string treeLeafLabels) {
			if (node.left == null) {//We have reached a leaf.
				if (node.currentNodeIsTheLeftChild == true) treeLeafLabels += "N";
				else treeLeafLabels += "I";
				return;
			}
			TreeLeafLabels(node.left, ref treeLeafLabels);
			TreeLeafLabels(node.right, ref treeLeafLabels);
		}


		static public string TreeLeafLabelsAsForest(TreeNode tree) {
			//We think of the tree as the range of a doubleTree.
			string r = "";
			//a tree is seen as a sequence of subtrees along the main arc /\ of the tree. A trivial subtree is a leaf.
			//reading left side of the range.
			//READING THE LEFT SIDE OF THE TREE
			TreeNode cur = tree.left;//tree cannot be null. it has to be at least the identity.
			
			while (cur != null) {//if range is a leaf the loop does nothing. if range is a caret the loop gives nothing. first leaf is not recorded
				if (cur.left != null) {//cur is not the leftmost leaf.
				 
					if (cur.right.left == null) //tree is of depth 0  so an 'L' is written
						r = "L" + r;
					else {
						string treeLeafLabels = "";
						TreeLeafLabels(cur.right, ref treeLeafLabels);
						r = "L" + treeLeafLabels.Substring(1) + r;//the first letter of the subtree is changed from N to L.
					}
				}
				
				cur = cur.left;				
			}
			//READING THE RIGHT SIDE OF THE TREE
			cur = tree.right;
			//r = r + "|";//adding the pointer at 1/2
			int treecounter = 0;
			
			while (cur != null) {//if range is a leaf the loop does nothing. if range is a caret the loop gives nothing. last leaf is not recorded
				if (cur.right != null) {// cur is not the rightmost leaf
					if (cur.left.left == null) //tree is of depth 0  so a 'R' is written except if it is the 1/2 leaf
						r = r + (treecounter==0?'L':'R');
					else {
						string treeLeafLabels = "";
						 TreeLeafLabels(cur.left,ref treeLeafLabels);
						r = r + (treecounter==0?'L':'X') + treeLeafLabels.Substring(1); //the first letter of the subtree is changed from N to X.
					}
				}
				cur = cur.right;
				++treecounter;
			}//format for range: tree1, tree2, tree3, | tree 4, tree5  

			if (tree.right != null) {//adding the label of the rightmostleaf
				if (tree.right.left == null)
					r += "L";//pointer is on the rightmost leaf
				else r += 'R';//pointer is not on the rightmost leaf
			}
			return r;
		}


		#endregion


		#region DoubleTreeNormalFormStr 
		//Valid only for elements of F.
		static public string DoubleTreeNormalFormSTR(DoubleTree doubleTree) {
			if (doubleTree.isIdentity() == true) return "identity";
			string normalform_range = "";
			string normalform_domain = "";
			int[] exponentsOfLeaves = DoubleTreeNormalFormExp(doubleTree);
			int numberofleaves = (exponentsOfLeaves.Length) / 2;

			for (int i = 0; i < numberofleaves; i++) {
				int exp = exponentsOfLeaves[i];
				if (exp > 0) {
					if (exp == 1) normalform_range += "x_" + i;
					if (exp != 1) normalform_range += "x_" + i + "^" + exp;
				}
				exp = exponentsOfLeaves[numberofleaves + i];
				if (exp > 0) {
					if (exp == 1) normalform_domain += "x_" + i;
					if (exp != 1) normalform_domain += "x_" + i + "^" + exp;
				}
			}

			if (normalform_domain == "") return normalform_range;
			return normalform_range + " I( " + normalform_domain + " )";

		}

		static public int[] DoubleTreeNormalFormExp(DoubleTree doubleTree) {
			MyList leaves = new MyList();
			TreeLeaves(doubleTree.range, ref leaves);
			MyNode cur = leaves.First;
			int nrange = 0;
			int ndomain = 0;
			int[] exponents = new int[2 * leaves.Length];

			//The subtrees on the right branch of the range/domain are made not LeftChildren i.e. they are made  RightChildren
			TreeNode treeNodeR = doubleTree.range.right;
			TreeNode treeNodeD = doubleTree.domain.right;
			while (treeNodeR.right != null) { treeNodeR.left.currentNodeIsTheLeftChild = false; treeNodeR = treeNodeR.right; }
			while (treeNodeD.right != null) { treeNodeD.left.currentNodeIsTheLeftChild = false; treeNodeD = treeNodeD.right; }

			while (cur != null) {
				exponents[nrange] = LeafLeftDepth(cur.Value);
				exponents[leaves.Length + ndomain] = LeafLeftDepth(cur.Value.sigma);
				++nrange;
				++ndomain;
				cur = cur.Next;
			}

			//The subtrees on the right branch of the range/domain are made  LeftChildren i.e. we undo the previous task.
			treeNodeR = doubleTree.range.right;
			treeNodeD = doubleTree.domain.right;
			while (treeNodeR.right != null) { treeNodeR.left.currentNodeIsTheLeftChild = true; treeNodeR = treeNodeR.right; }
			while (treeNodeD.right != null) { treeNodeD.left.currentNodeIsTheLeftChild = true; treeNodeD = treeNodeD.right; }


			exponents[0] -= 2;//we subtract 1 twice: the first time is because we count the number of currentNodeIsTheLeftChild==true and the zero-leaf is counted. 
			//the second time is because the normal form reduces the left-path by one when the path ends on the main arc of the tree.
			exponents[leaves.Length] -= 2;// same deal for thedomain.
			return exponents;

		}
		//called from DoubleTreeNormalFormExp
		static public int LeafLeftDepth(TreeNode node) {
			int depth = 0;
			TreeNode cur = node;
			while (cur != null) {
				if (cur.currentNodeIsTheLeftChild == false) break;
				++depth;
				cur = cur.parent;
			}
			return depth;

		}


		#endregion

		//It reads the leaves and saves the left-depths of each leaf, i.e. the leaf is the left child of its parent and the parent is the left child of its parents.
		//double tree cannot be the identity



		#region Change of base 2,4,64


		static public string base2toBase4(string input) {
			string output = "";
			input = input + "00";
			string segment;
			for (int i = 0; i < input.Length - 2; i += 2) {
				segment = input.Substring(i, 2);
				output += TupleBase2To4(ref segment);
			}
			return output;
		}


		static private char TupleBase2To4(ref string tupleBase4) {
			char digit4='0';
			switch (tupleBase4) {
				case "00": digit4 = '0'; break;
				case "01": digit4 = '1'; break;
				case "10": digit4 = '2'; break;
				case "11": digit4 = '3'; break;
				default: Console.WriteLine("Error:Base2to4 not such 2 digits in base 4"); break;
			}
			return digit4;
		}

		static public string base4toBase2(string input) {
			string output = "";

			for (int i = 0; i < input.Length; i++) {
				output += Base4ToTupleBase2(input[i]);
			}
			return output;
		}


		static private string Base4ToTupleBase2(char digitBase4) {
			string  tupleBase2 = "";
			switch (digitBase4) {
				case '0': tupleBase2 = "00"; break;
				case '1': tupleBase2 = "01"; break;
				case '2': tupleBase2 = "10"; break;
				case '3': tupleBase2 = "11"; break;
				default: Console.WriteLine("Error:Base2to4 not such 2 digits in base 4"); break;
			}
			return tupleBase2;
		}

		static private string base4toBase64(string input) {
			string output = "";
			input = input + "000";
			string segment;
			for (int i = 0; i < input.Length - 3; i += 3) {
				segment = input.Substring(i, 3);
				output += TripleBase4to64(ref segment);
			}
			return output;
		}



		static private char TripleBase4to64(ref string tripleBase4)//base4=d1d2d3 where the digits are between 0 and 3. 
	{
			char digit64 = '0';
			switch (tripleBase4) {
				case "000": digit64 = '0'; break;
				case "001": digit64 = '1'; break;
				case "002": digit64 = '2'; break;
				case "003": digit64 = '3'; break;
				case "010": digit64 = '4'; break;
				case "011": digit64 = '5'; break;
				case "012": digit64 = '6'; break;
				case "013": digit64 = '7'; break;
				case "020": digit64 = '8'; break;
				case "021": digit64 = '9'; break;
				case "022": digit64 = ':'; break;
				case "023": digit64 = ';'; break;
				case "030": digit64 = '<'; break;
				case "031": digit64 = '='; break;
				case "032": digit64 = '>'; break;
				case "033": digit64 = '?'; break;
				case "100": digit64 = '@'; break;
				case "101": digit64 = 'A'; break;
				case "102": digit64 = 'B'; break;
				case "103": digit64 = 'C'; break;
				case "110": digit64 = 'D'; break;
				case "111": digit64 = 'E'; break;
				case "112": digit64 = 'F'; break;
				case "113": digit64 = 'G'; break;
				case "120": digit64 = 'H'; break;
				case "121": digit64 = 'I'; break;
				case "122": digit64 = 'J'; break;
				case "123": digit64 = 'K'; break;
				case "130": digit64 = 'L'; break;
				case "131": digit64 = 'M'; break;
				case "132": digit64 = 'N'; break;
				case "133": digit64 = 'O'; break;
				case "200": digit64 = 'P'; break;
				case "201": digit64 = 'Q'; break;
				case "202": digit64 = 'R'; break;
				case "203": digit64 = 'S'; break;
				case "210": digit64 = 'T'; break;
				case "211": digit64 = 'U'; break;
				case "212": digit64 = 'V'; break;
				case "213": digit64 = 'W'; break;
				case "220": digit64 = 'X'; break;
				case "221": digit64 = 'Y'; break;
				case "222": digit64 = 'Z'; break;
				case "223": digit64 = '['; break;
				case "230": digit64 = '\\'; break;
				case "231": digit64 = ']'; break;
				case "232": digit64 = '^'; break;
				case "233": digit64 = '_'; break;
				case "300": digit64 = '`'; break;
				case "301": digit64 = 'a'; break;
				case "302": digit64 = 'b'; break;
				case "303": digit64 = 'c'; break;
				case "310": digit64 = 'd'; break;
				case "311": digit64 = 'e'; break;
				case "312": digit64 = 'f'; break;
				case "313": digit64 = 'g'; break;
				case "320": digit64 = 'h'; break;
				case "321": digit64 = 'i'; break;
				case "322": digit64 = 'j'; break;
				case "323": digit64 = 'k'; break;
				case "330": digit64 = 'l'; break;
				case "331": digit64 = 'm'; break;
				case "332": digit64 = 'n'; break;
				case "333": digit64 = 'o'; break;
				default: Console.WriteLine("Error:Base4to64 not such 3 digits in base 4"); break;
			}

			return digit64;
		}



		static private string base64toBase4(string input) {
			string output = "";
			for (int i = 0; i < input.Length; ++i) {
				output += Base64ToTripleBase4(input[i]);
			}
			return output;
		}

		static private string Base64ToTripleBase4(char digitBase64)//base4=d1d2d3 where the digits are between 0 and 3. 
	{
			string tripleBase4 = "";
			switch (digitBase64) {
				case '0': tripleBase4 = "000"; break;
				case '1': tripleBase4 = "001"; break;
				case '2': tripleBase4 = "002"; break;
				case '3': tripleBase4 = "003"; break;
				case '4': tripleBase4 = "010"; break;
				case '5': tripleBase4 = "011"; break;
				case '6': tripleBase4 = "012"; break;
				case '7': tripleBase4 = "013"; break;
				case '8': tripleBase4 = "020"; break;
				case '9': tripleBase4 = "021"; break;
				case ':': tripleBase4 = "022"; break;
				case ';': tripleBase4 = "023"; break;
				case '<': tripleBase4 = "030"; break;
				case '=': tripleBase4 = "031"; break;
				case '>': tripleBase4 = "032"; break;
				case '?': tripleBase4 = "033"; break;
				case '@': tripleBase4 = "100"; break;
				case 'A': tripleBase4 = "101"; break;
				case 'B': tripleBase4 = "102"; break;
				case 'C': tripleBase4 = "103"; break;
				case 'D': tripleBase4 = "110"; break;
				case 'E': tripleBase4 = "111"; break;
				case 'F': tripleBase4 = "112"; break;
				case 'G': tripleBase4 = "113"; break;
				case 'H': tripleBase4 = "120"; break;
				case 'I': tripleBase4 = "121"; break;
				case 'J': tripleBase4 = "122"; break;
				case 'K': tripleBase4 = "123"; break;
				case 'L': tripleBase4 = "130"; break;
				case 'M': tripleBase4 = "131"; break;
				case 'N': tripleBase4 = "132"; break;
				case 'O': tripleBase4 = "133"; break;
				case 'P': tripleBase4 = "200"; break;
				case 'Q': tripleBase4 = "201"; break;
				case 'R': tripleBase4 = "202"; break;
				case 'S': tripleBase4 = "203"; break;
				case 'T': tripleBase4 = "210"; break;
				case 'U': tripleBase4 = "211"; break;
				case 'V': tripleBase4 = "212"; break;
				case 'W': tripleBase4 = "213"; break;
				case 'X': tripleBase4 = "220"; break;
				case 'Y': tripleBase4 = "221"; break;
				case 'Z': tripleBase4 = "222"; break;
				case '[': tripleBase4 = "223"; break;
				case '\\': tripleBase4 = "230"; break;
				case ']': tripleBase4 = "231"; break;
				case '^': tripleBase4 = "232"; break;
				case '_': tripleBase4 = "233"; break;
				case '`': tripleBase4 = "300"; break;
				case 'a': tripleBase4 = "301"; break;
				case 'b': tripleBase4 = "302"; break;
				case 'c': tripleBase4 = "303"; break;
				case 'd': tripleBase4 = "310"; break;
				case 'e': tripleBase4 = "311"; break;
				case 'f': tripleBase4 = "312"; break;
				case 'g': tripleBase4 = "313"; break;
				case 'h': tripleBase4 = "320"; break;
				case 'i': tripleBase4 = "321"; break;
				case 'j': tripleBase4 = "322"; break;
				case 'k': tripleBase4 = "323"; break;
				case 'l': tripleBase4 = "330"; break;
				case 'm': tripleBase4 = "331"; break;
				case 'n': tripleBase4 = "332"; break;
				case 'o': tripleBase4 = "333"; break;

				default: Console.WriteLine("Error:Base4to64 not such 3 digits in base 4"); break;
			}

			return tripleBase4;
		}




        #endregion


	}

}
