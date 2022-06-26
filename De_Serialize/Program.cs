using System;
using System.Collections.Generic;
using System.IO;

namespace De_Serialize
{
    class ListNode
    {
        public ListNode Previous;
        public ListNode Next;
        public ListNode Random; // произвольный элемент внутри списка
        public string Data;
    }

    class ListRandom
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        public void Serialize(Stream s)
        {
            using StreamWriter SW = new StreamWriter(s);
            List<ListNode> arr = new List<ListNode>();
            ListNode Mark = Head;

            for(int i = 0; i < Count; i++)
            {
                arr.Add(Mark);
                Mark = Mark.Next;
            }

            Mark = Head;
            for (int i = 0; i < Count; i++)
            {
                SW.WriteLine(Mark.Data.ToString() + ":" + arr.IndexOf(Mark.Random).ToString());
                Mark = Mark.Next;
            }
        }

        public void Deserialize(Stream s)
        {
            using StreamReader SR = new StreamReader(s);
            List<ListNode> arr = new List<ListNode>();
            List<Int32> index = new List<int>();  
            ListNode Mark = new ListNode();
            Count = 0;
            string text;

            while ((text = SR.ReadLine()) != null)
            {
                if (text != "")
                {
                    ListNode node = new ListNode();
                    Mark.Data = text.Split(":")[0];
                    index.Add(Convert.ToInt32(text.Split(":")[1]));
                    Mark.Next = node;
                    node.Previous = Mark;
                    arr.Add(Mark);
                    Mark = node;
                    Count++;
                }
            }
            Head = arr[0];
            Tail = Mark.Previous;
            Tail.Next = null;

            for (int i = 0; i < Count; i++)
            {
                arr[i].Random = arr[index[i]];
            }
        }
    }

    class Program
    {
        static Random ran = new Random();
        static ListNode AddNode(ListNode tail)
        {
            ListNode Node = new ListNode();
            Node.Previous = tail;
            Node.Next = null;
            Node.Data = ran.Next(0, 100).ToString();
            tail.Next = Node;
            return Node;
        }
        static ListNode RandNode(ListNode head, int Len)
        {
            int r = ran.Next(0, Len);
            for(int i = 0; i < r; i++)
            {
                head = head.Next;
            }
            return head;
        }
        static void Main(string[] args)
        {
            int len = 10;// length list

            ListNode head = new ListNode();
            head.Data = ran.Next(0, 100).ToString();

            ListNode tail = head;
            for (int i = 1; i < len; i++)
                tail = AddNode(tail);

            ListNode Mark = head;
            for (int i = 0; i < len; i++)
            {
                Mark.Random = RandNode(head, len);
                Mark = Mark.Next;
            }

            ListRandom Ser = new ListRandom();
            Ser.Head = head;
            Ser.Tail = tail;
            Ser.Count = len;

            FileStream fs = new FileStream("test.dat", FileMode.OpenOrCreate);
            Ser.Serialize(fs);

            ListRandom DeSer = new ListRandom();
            fs = new FileStream("test.dat", FileMode.Open);
            DeSer.Deserialize(fs);

            if (Ser.Tail.Data == DeSer.Tail.Data && Ser.Tail.Random.Data == DeSer.Tail.Random.Data)
            {
                Console.WriteLine("Functions completed.");
            }
            Console.ReadLine();
        }
    }
}
