using System;
using System.IO;
using System.Threading;

namespace work2
{
    class Program
    {
        public static int count = 0;
        public static int newDelta = -1;
        public static int letters = 0;
        public static int firstLetter;
        public static int secondLetter;


        public static void FindSubString(char[] text, string StringToSearch, int delta)
        {
            StringToSearch = StringToSearch.Substring(letters);
            int j=0,i=0;
            if (secondLetter != 0)
            {
                if (StringToSearch[0] != text[10000 - secondLetter + newDelta])
                    return;
                i = 10000 - secondLetter + newDelta - 1;
            }
            else
            {
                if (StringToSearch[0] != text[newDelta])
                    return;
            }
            for (j = 1; j < StringToSearch.Length; j++)
            {
                if (i + delta + 1 >= text.Length)
                    return;
                if (text[i + delta + 1] != StringToSearch[j])
                    break;
                i += delta + 1;
                if (i >= text.Length)
                    break;
            }
            if (j==StringToSearch.Length) //if we found the string, we print it and exit the program
            {
                Console.WriteLine(firstLetter);
                System.Environment.Exit(0);
            }
        }

        public static void FindString(char[] text, string StringToSearch, int delta)
        {
            int i = 0,j = 0,index ;
            bool foundWord = false, secondFlag=false, firstFlag=false;

            if (newDelta != -1)//if part of the string is in the next buffer
            {
                FindSubString(text, StringToSearch, delta);
            }
            //reset the global varaibles
            newDelta = -1;
            secondLetter = 0;
            letters = 0;

            for (i = 0; i < text.Length; i++)
            {
                
                if (firstFlag==false && text[i]==StringToSearch[0])
                {
                    index = i;
                    for (j = 1; j < StringToSearch.Length; j++)
                    {
                        if (index + delta + 1 >= text.Length) //preper the variables to the next fuffer
                        {
                            letters = j;
                            if (delta % 10000 == 0 &&delta!=0) 
                                newDelta = index;
                            else if (delta > 10000)
                                newDelta = index + delta - text.Length;
                            else
                                newDelta = index + delta +1- text.Length;
                            firstLetter = count;
                            firstFlag = true;
                            break;
                        }
                        if (text[index + delta + 1] != StringToSearch[j])
                            break;
                        index += delta + 1;
                        
                    }
                }
                else if (secondFlag == false && firstFlag == true &&text[i] == StringToSearch[0] && i+delta+1>text.Length)
                {
                    secondFlag = true;
                    secondLetter = i;
                }
                if (j==StringToSearch.Length)
                {
                    foundWord = true;
                    break;
                }
                count++;
            }
            if (foundWord == true) //if we found the string, we print it and exit the program
            {
                Console.WriteLine(count);
                System.Environment.Exit(0);
            }
        }
        static void Main(string[] args)
        {
            //extract the args we given to the program
            String StringToSearch = args[1];
            int delta = int.Parse(args[3]);
            int numberThreads = int.Parse(args[2]);

            //create thread pool with the number of threads we given 
            ThreadPool.SetMinThreads(numberThreads, 0);
            ThreadPool.SetMaxThreads(numberThreads, 0);



            using (StreamReader r = new StreamReader(args[0]))
            {
                char[] buffer = new char[10000];
                while(!r.EndOfStream)
                {
                    if (newDelta!=-1&& secondLetter!=0)
                    {
                        int j = 0;
                        for(int i=secondLetter;i<buffer.Length;i++)
                        {
                            buffer[j] = buffer[i];
                            j++;
                            count--;
                        }
                    }
                    if (secondLetter==0)
                        r.ReadBlock(buffer, 0, buffer.Length);
                    else
                        r.ReadBlock(buffer, buffer.Length - secondLetter, secondLetter);
                    WaitCallback t = delegate { FindString(buffer, StringToSearch, delta); };
                    ThreadPool.QueueUserWorkItem(t);
                    Thread.Sleep(500);
                }
            }
            Console.WriteLine("not found");

        }
    }
}
