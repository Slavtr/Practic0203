using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;


namespace Матмоделирование_практика
{
    public class Dzhon
    {
        struct Dtl
        {
            public int T1;
            public int T2;
        }
        public Dzhon(string path)
        {
            ToCSV(FnlMv(Flrd(path)), "File.txt");
            Console.WriteLine(VrPrs(FnlMv(Flrd(path))));
        }
        List<Dtl> Flrd(string path)
        {
            List<Dtl> ret = new List<Dtl>();
            using (StreamReader sr = new StreamReader(path))
            {
                while (sr.EndOfStream != true)
                {
                    string str1 = sr.ReadLine();
                    string str2 = sr.ReadLine();
                    int[] ms1 = Array.ConvertAll(str1.Split(';'), int.Parse);
                    int[] ms2 = Array.ConvertAll(str2.Split(';'), int.Parse);
                    if (ms1.Length == ms2.Length)
                    {
                        for (int i = 0; i < ms1.Length; i++)
                        {
                            ret.Add(new Dtl { T1 = ms1[i], T2 = ms2[i] });
                        }
                    }
                }
            }
            foreach (Dtl dtl in ret)
            {
                Debug.WriteLine(dtl.T1.ToString() + " " + dtl.T2.ToString());
            }
            return ret;
        }
        List<Dtl> FnlMv(List<Dtl> ms)
        {
            Stack<Dtl> st = new Stack<Dtl>();
            Queue<Dtl> q = new Queue<Dtl>();
            Dtl min;

            while (ms.Count > 0)
            {
                min = ms[minel(ms)];
                ms.RemoveAt(minel(ms));

                if (min.T1 < min.T2)
                {
                    q.Enqueue(min);
                }
                else if (min.T2 < min.T1)
                {
                    st.Push(min);
                }
                else if (min.T1 == min.T2)
                {
                    q.Enqueue(min);
                }
            }
            List<Dtl> ret = new List<Dtl>();
            while (q.Count > 0)
            {
                ret.Add(q.Dequeue());
            }
            while (st.Count > 0)
            {
                ret.Add(st.Pop());
            }
            return ret;
        }
        int minel(List<Dtl> ms)
        {
            int min = ms[0].T1, minindex = 0;
            foreach (Dtl dt in ms)
            {
                if (dt.T1 < dt.T2)
                {
                    if (dt.T1 < min)
                    {
                        min = dt.T1;
                        minindex = ms.IndexOf(dt);
                    }
                }
                if (dt.T2 < dt.T1)
                {
                    if (dt.T2 < min)
                    {
                        min = dt.T2;
                        minindex = ms.IndexOf(dt);
                    }
                }
                if (dt.T2 == dt.T1)
                {
                    if (dt.T1 < min)
                    {
                        min = dt.T1;
                        minindex = ms.IndexOf(dt);
                    }
                }
            }
            return minindex;
        }
        void ToCSV(List<Dtl> ls, string path)
        {
            string fnstr = "";
            foreach (Dtl d in ls)
            {
                fnstr += d.T1.ToString() + ';';
            }
            fnstr += '\n';
            foreach (Dtl d in ls)
            {
                fnstr += d.T2.ToString() + ';';
            }
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine(fnstr);
            }
        }
        int VrPrs(List<Dtl> ls)
        {
            int max = 0;
            for (int j = 0; j < ls.Count; j++)
            {
                int sum1 = 0;
                int sum2 = 0;
                for (int i = 0; i <= j - 1; i++)
                {
                    sum1 += ls[i].T2;
                }
                for (int i = 0; i <= j; i++)
                {
                    sum2 += ls[i].T1;
                }
                Debug.WriteLine(sum2 - sum1);
                if ((sum2 - sum1) > max)
                {
                    max = sum2 - sum1;
                }
            }
            return max;
        }
    }
}
