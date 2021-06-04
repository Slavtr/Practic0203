using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Матмоделирование_практика
{
    class Kmvzh
    {
        string s = "";
        List<Sturla> vih = new List<Sturla>();
        public Kmvzh(string path)
        {
            List<Sturla> ls = Flrd(path);
            List<List<Sturla>> fnlcn = new List<List<Sturla>>();
            foreach (Sturla rb in ls)
            {
                vih.Add(rb);
                Mv(ls, rb);
                fnlcn.Add(RtPrs(ls, s));
                s = "";
                vih.Clear();
            }
            int max = 2147483647, maxind = 0;
            foreach(List<Sturla> st in fnlcn)
            {
                if(AllPoints(ls, st))
                {
                    if(FnlMv(st)<=max)
                    {
                        max = FnlMv(st);
                        maxind = fnlcn.IndexOf(st);
                    }
                }
            }
            using (StreamWriter sr = new StreamWriter("ПутьКоммивояжёра.txt"))
            {
                sr.WriteLine("Путь: ");
                foreach (Sturla rb in fnlcn[maxind])
                {
                    sr.WriteLine(rb.point1 + " - " + rb.point2);
                }
                sr.WriteLine("Итог: ");
                sr.WriteLine(max);
            }
        }
        struct Sturla
        {
            public int point1;
            public int point2;
            public int length;
        }
        int Minel(List<Sturla> ls)
        {
            int min = ls[0].point1, minind = 0;
            foreach (Sturla rb in ls)
            {
                if (rb.point1 <= min)
                {
                    min = rb.point1;
                    minind = ls.IndexOf(rb);
                }
            }
            return minind;
        }
        int Mv(List<Sturla> ls, Sturla minel)
        {
            int ret = 0;
            Sturla rb = ls.Find(x => x.point1 == minel.point1 && x.point2 == minel.point2);
            s += rb.point1.ToString() + "-" + rb.point2.ToString();
            if (rb.point2 == vih[0].point1)
            {
                s += ";";
                return rb.length;
            }
            else
            {
                for (int i = 0; i < ls.Count; i++)
                {
                    List<Sturla> rt = vih.FindAll(x => x.point1 == ls[i].point2);
                    rt.Remove(vih[0]);
                    if (ls[i].point1 == rb.point2 && ls[i].point2 != rb.point1 && !vih.Intersect(rt).Any())
                    {
                        s += ",";
                        vih.Add(ls[i]);
                        ret = Mv(ls, ls[i]) + rb.length;
                    }
                }
            }
            return ret;
        }
        List<Sturla> RtPrs(List<Sturla> ls, string s)
        {
            List<List<Sturla>> ret = new List<List<Sturla>>();
            string[] str1 = s.Split(';');
            foreach (string st1 in str1)
            {
                if (st1 != "")
                {
                    ret.Add(new List<Sturla>());
                    string[] str2 = st1.Split(',');
                    foreach (string st2 in str2)
                    {
                        if (st2 != "")
                        {
                            string[] str3 = st2.Split('-');
                            ret[ret.Count - 1].Add(ls.Find(x => x.point1 == Convert.ToInt32(str3[0]) && x.point2 == Convert.ToInt32(str3[1])));
                        }
                    }
                }
            }
            for(int i = 0; i<ret.Count; i++)
            {
                if(i>0)
                {
                    if(ret[i][0].point1 != ret[i][ret[i].Count-1].point2)
                    {
                        ret[i].InsertRange(0, ret[i - 1].FindAll(x => ret[i - 1].IndexOf(x) <= ret[i - 1].FindIndex(y => y.point2 == ret[i][0].point1)));
                    }
                }
            }
            foreach(List<Sturla> rt in ret)
            {
                if(AllPoints(ls, rt))
                {
                    return rt;
                }
            }
            return ret[ret.Count - 1];
        }
        int FnlMv(List<Sturla> ls)
        {
            int ret = 0;
            foreach (Sturla rb in ls)
            {
                ret += rb.length;
            }
            return ret;
        }
        List<Sturla> Flrd(string path)
        {
            List<Sturla> ret = new List<Sturla>();
            using (StreamReader sr = new StreamReader(path))
            {
                while (sr.EndOfStream != true)
                {
                    string[] str1 = sr.ReadToEnd().Split('\n');
                    for (int i = 0; i < str1.Length; i++)
                    {
                        string[] str2 = str1[i].Split(';');
                        for (int j = 0; j < str2.Length; j++)
                        {
                            if (str2[j]!="" && Convert.ToInt32(str2[j]) != 0)
                            {
                                ret.Add(new Sturla { point1 = i+1, point2 = j+1, length = Convert.ToInt32(str2[j]) });
                            }
                        }
                    }
                }
            }
            return ret;
        }
        bool AllPoints(List<Sturla> ls, List<Sturla> ks)
        {
            bool ret = true;
            List<int> pointmas = new List<int>();
            foreach(Sturla st in ls)
            {
                if(!pointmas.Contains(st.point1))
                {
                    pointmas.Add(st.point1);
                }
                else if(!pointmas.Contains(st.point2))
                {
                    pointmas.Add(st.point2);
                }
            }
            foreach(int t in pointmas)
            {
                if (!ks.Contains(ks.Find(x => x.point1 == t || x.point2 == t)))
                    ret = false;
            }
            return ret;
        }
    }
}
