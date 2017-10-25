using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace archiver
{
    class Program
    {
        private static Dictionary<char, int> occurences;
        private static Dictionary<char, string> codes = new Dictionary<char, string>();
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("input.txt");
            StreamWriter sw = new StreamWriter("output.txt");

            count = new Dictionary<char, int>();

            while (!sr.EndOfStream)
            {
                string str = sr.ReadLine();
                str += "~";

                for (int i = 0; i < str.Length; i++)
                {
                    if (count.ContainsKey(str[i]))
                    {
                        count[str[i]] += 1;
                    }
                    else
                    {
                        count.Add(str[i], 1);
                    }
                }

            }

            List<KeyValuePair<char, int>> list = count.OrderBy(o => o.Value).Reverse().ToList();

            foreach (KeyValuePair<char, int> p in list)
            {
                sw.WriteLine(p.Key + ":" + p.Value.ToString());
            }


            List<HuffmanTreeNode> treeList = new List<HuffmanTreeNode>();
            for (int i = 0; i < list.Count; i++)
            {
                HuffmanTreeNode node = new HuffmanTreeNode();
                node.symbol = list[i].Key;
                node.weight = list[i].Value;
                treeList.Add(node);
            }

            while (treeList.Count > 1)
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

            string context = null;
            buildHuffmanTreeNode(treeList[0], codes, context);



            BinaryWriter ar = new BinaryWriter(new FileStream("archive.ivt", FileMode.CreateNew));
            ar.Write('i');
            ar.Write('v');
            ar.Write('t');
            ar.Write(((byte)codes.Count));

            foreach (KeyValuePair<char, string> kv in codes)
            {
                ar.Write(kv.Key);
                ar.Write((byte)kv.Value.Length);
                for (int i = 0; i < kv.Value.Length; i++)
                {
                    ar.Write(kv.Value[i] == '0' ? '0' : '1');
                }
            }

            StreamReader srr = new StreamReader("input.txt");

            while (!srr.EndOfStream)
            {
                string str = srr.ReadLine();
                str += "~";
                for (int i = 0; i < str.Length; i++)
                {
                    if (codes.ContainsKey(str[i]))
                    {
                        for (int j = 0; j < codes[str[i]].Length; j++)
                        {
                            ar.Write(codes[str[i]][j] == '0' ? '0' : '1');
                        }
                    }
                    else 
                    { 
                        new Exception("Ой, ошибочка :(");
                    }
                }
            }
            ar.Close();
            sw.Close();

            IVTarchiver.main();
        }


        public static void buildHuffmanTreeNode(HuffmanTreeNode node, Dictionary<char, string> codes, string context = "")
        {


            if (node.leftChild == null)
            {
                codes.Add(node.symbol, context);
            }


            if (node.leftChild != null)
            {
                buildHuffmanTreeNode(node.leftChild, codes, context + "0");

            } 

            if (node.rightChild != null)
            {
                buildHuffmanTreeNode(node.rightChild, codes, context + "1");
            }

        }


        public class HuffmanTreeNode
        {
            public char symbol;
            public HuffmanTreeNode parent;
            public HuffmanTreeNode leftChild;
            public HuffmanTreeNode rightChild;
            public int weight;
        }

        public static Dictionary<char, int> count { get; set; }
    }
}
