using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Program
    {
        private static Dictionary<char, int> occurences;
        private static Dictionary<char, string> codes = new Dictionary<char, string>();
        static void Main()
        {
            StreamReader sr = new StreamReader("geroi.txt");
            occurences = new Dictionary<char, int>();

            while(!sr.EndOfStream)
            {
                string str = sr.ReadLine();

                for (int i = 0; i <str.Length; i++)
                {
                    if (occurences.ContainsKey(str[i]))
                    {
                        occurences[str[i]] += 1;
                    }
                    else
                    {
                        occurences.Add(str[i], 1);
                    }
                }
            }
            StreamWriter sw = new StreamWriter("output.txt");

            List<KeyValuePair<char, int> list = occurences.OrderBy(o => o.Value).Reverse().ToList();
            forech (KeyValuePair<char, int> p in list)
            {
                sw.WriteLine("Symbol '" + p.Key + "':" + p.Value.ToString());
            }
            //Building Huffman Tree
            List<HuffmanTreeNode> treeList = new List<HuffmanTreeNode>();
            for (int i = 0; i < list.Count; i++)
            {
                HuffmanTreeNode node = new HuffmanTreeNode();
                node.symbol = list[i].Key;
                node.weight = list[i].Value;
                treeList.Add(node);
            }

            //build a Huffman tree
            while(treeList.Count > 1)
            {
                treeList = treeList.OrderBy(o => o.weight).ToList();
                HuffmanTreeNode parent = new HuffmanTreeNode();
                parent.leftChild = treeList[0];
                parent.rightChild = treeList[1];

                parent.weight = parent.leftChild.weight + parent.rightChild.weight;
                treeList.RemoveAt(0);
                treeList.RemoveAt(0);
                treeList.Add(parent);
            }
            printHuffmanTree(treeList[0], sw);

            buildHuffmanTree(treeList[0]. sw);

            StreamWriter sw = new StreamWriter("archive.ivt");
            StreamReader sr = new StreamReader("geroi.txt");
            sw.Close();
        }
        public static void printHuffmanTree(HuffmanTreeNode node, StreamWriter sw, string context = "")
        {
            if(node.leftChild == null)
            {
                sw.WriteLine("'" + node.symbol + "':" + context);
                return;
            }

            if(node.leftChild != null)
            {
                printHuffmanTree(node.leftChild, sw, context + "0");
            }

            if(node.rightChild != null)
            {
                printHuffmanTree(node.rightChild, sw, context + "1");
            }
        }
        // Huffman Tree
        public class HuffmanTreeNode
        {
            public char symbol;
            public HuffmanTreeNode parent;
            public HuffmanTreeNode leftChild;
            public HuffmanTreeNode rightChild;
            public int weight;
        }
    }
}
