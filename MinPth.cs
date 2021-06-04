using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Матмоделирование_практика
{
    class MinPth
    {
        string s = "";
        public MinPth(string path)
        {
            List<Sturla> ls = Flrd(path);
            List<List<Sturla>> fnlcn = new List<List<Sturla>>();
            List<Sturla> ret = ls.FindAll(x => x.point1 == ls[Minel(ls)].point1);
            foreach (Sturla rb in ret)
            {
                Mv(ls, rb);
                fnlcn.Add(RtPrs(ls, s));
                s = "";
            }
            int max = 2147483647, maxind = 0;
            foreach (List<Sturla> st in fnlcn)
            {
                if (FnlMv(st) <= max)
                {
                    max = FnlMv(st);
                    maxind = fnlcn.IndexOf(st);
                }
            }
            using (StreamWriter sr = new StreamWriter("КороткаяДорога.txt"))
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
        struct Sturla
        {
            public int point1;
            public int point2;
            public int length;
        }
        int Maxel(List<Sturla> ls)
        {
            int min = ls[0].point2, maxind = 0;
            foreach (Sturla rb in ls)
            {
                if (rb.point2 >= min)
                {
                    min = rb.point1;
                    maxind = ls.IndexOf(rb);
                }
            }
            return maxind;
        }
        int Mv(List<Sturla> ls, Sturla minel)
        {
            int ret = 0;
            Sturla rb = ls.Find(x => x.point1 == minel.point1 && x.point2 == minel.point2);
            s += rb.point1.ToString() + "-" + rb.point2.ToString();
            if (rb.point2 == ls[Maxel(ls)].point2)
            {
                s += ";";
                return rb.length;
            }
            else
            {
                for (int i = 0; i < ls.Count; i++)
                {
                    if (ls[i].point1 == rb.point2)
                    {
                        s += ",";
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
            for (int i = 0; i < ret.Count; i++)
            {
                if (i > 0)
                {
                    if (ret[i][0].point1 != ret[i][ret[i].Count - 1].point2)
                    {
                        ret[i].InsertRange(0, ret[i - 1].FindAll(x => ret[i - 1].IndexOf(x) <= ret[i - 1].FindIndex(y => y.point2 == ret[i][0].point1)));
                    }
                }
            }
            int max = 2147483647, maxind = 0;
            foreach (List<Sturla> st in ret)
            {
                if (FnlMv(st) <= max)
                {
                    max = FnlMv(st);
                    maxind = ret.IndexOf(st);
                }
            }
            return ret[maxind];
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
                    string[] str1 = sr.ReadLine().Split(';');
                    string[] str2 = str1[0].Split('-');
                    ret.Add(new Sturla { point1 = Convert.ToInt32(str2[0]), point2 = Convert.ToInt32(str2[1]), length = Convert.ToInt32(str1[1]) });
                }
            }
            return ret;
        }
    }
}
