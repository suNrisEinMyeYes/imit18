using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imit18
{
    class Program
    {
        static Random rnd = new Random();
        static int BusyWindowsCount = 0, ClientsInQue = 0, WindowsCount = 3, NumbOfMin, NumbOfExp, ClientsInBank = 0, ClientsTotal=0;
        static List<Agent> agents = new List<Agent>();
        static List<float> taus = new List<float>();
        static List<float> PosibilityTheo = new List<float>();
        static float  La = 0.7f, Mu = 0.8f;
        static Dictionary<int, int> Freq = new Dictionary<int, int>();
        static void Main(string[] args)
        {
            Console.Write("Input windows count: ");
            WindowsCount = Int32.Parse(Console.ReadLine());
            Console.Write("Input Agents count: ");
            int AgentsCount = Int32.Parse(Console.ReadLine());
            float min = 100;
            float Delta;
            
            for (int i = 0; i < AgentsCount; i++)
            {
                agents.Add(new Agent(La));//float.Parse(Console.ReadLine())
                taus.Add(agents[i].GetNextEvent());
                
                if (taus[i]<min)
                {
                    min = taus[i];
                    NumbOfMin = i;
                }
            }
            Console.Write("Input experiments count: ");
            NumbOfExp = Int32.Parse(Console.ReadLine());
            for (int i = 0; i < NumbOfExp; i++)
            {

                Delta = ExpRv(Mu * BusyWindowsCount);
                if (min < Delta)
                {
                    if (BusyWindowsCount < WindowsCount)
                    {
                        BusyWindowsCount++;
                    }
                    else
                    {
                        ClientsInQue++;
                    }
                    ClientsInBank++;
                    ClientsTotal++;
                }
                else
                {
                    
                    if (ClientsInQue == 0)
                    {
                        BusyWindowsCount--;
                    }
                    else
                    {
                        ClientsInQue--;
                    }
                    ClientsInBank--;
                }
                FreqAdder(ClientsInBank,1);
                min = 100;
                taus[NumbOfMin] = agents[NumbOfMin].GetNextEvent();
                for (int j = 0; j < AgentsCount; j++)
                {
                    if (taus[j] < min)
                    {
                        min = taus[j];
                        NumbOfMin = j;
                    }
                }
                PosibilityTheo.Add(StatTheo());
                Console.WriteLine("System State");
                Console.WriteLine("BusyWindows: " + BusyWindowsCount);
                Console.WriteLine("Agents in Queue: " + ClientsInQue);
                Console.WriteLine("Theoretical System State: " + PosibilityTheo[i]);
                Console.WriteLine("----------------------------------");
                
                

            }


            foreach (var item in Freq)
            {
                Console.WriteLine("Emperical System State: " + (float)item.Value / (float)ClientsTotal);
            }
            
            
            Console.ReadLine();
        }

        static float ExpRv(float x) => (float)(-Math.Log(rnd.NextDouble()) / x);
        

        static float Factor(int x)
        {
            int Mul = 1;
            if (x==0)
            {
                return 1;
            }
            else
            {
                while (x>0)
                {
                    Mul *= x;
                    x--;
                }
                return Mul;
            }
        }

        static float StatTheo()
        {
            float PT;
            if (ClientsInBank < WindowsCount)
            {
                float sum = 0;
                for (int i = 0; i < WindowsCount; i++)
                {
                    sum += ((float)Math.Pow(La / Mu, ClientsInBank) / Factor(ClientsInBank));
                }
                sum += ((float)Math.Pow(La / Mu, WindowsCount + 1) / (Factor(WindowsCount) * (WindowsCount - (La / Mu))));
                PT = ((float)Math.Pow(La / Mu, ClientsInBank) / Factor(ClientsInBank)) * sum;
            }
            else
            {
                float sum = 0;
                for (int i = 0; i < WindowsCount; i++)
                {
                    sum += ((float)Math.Pow(La / Mu, ClientsInBank) / Factor(ClientsInBank));
                }
                PT = ((float)Math.Pow(La / Mu, ClientsInBank) / (Factor(WindowsCount) * (float)Math.Pow(WindowsCount, ClientsInBank - WindowsCount))) * sum;
            }
            return PT;
        }

        static void FreqAdder(int Key,int Value)
        {
            if (Freq.ContainsKey(Key))
            {
                Freq[Key]++;
            }
            else
            {
                Freq.Add(Key, Value);
            }
        }
        
    }
}
