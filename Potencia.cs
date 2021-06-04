using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Матмоделирование_практика
{
    class Potencia
    {
        public Potencia(string path)
        {
            int[,] matr_stoim;
            int[,] matr_zatr = new int[0,0];
            int[] vekt_pos, vekt_pok;
            Flrd(out matr_stoim, out vekt_pos, out vekt_pok, path);
            Potent(matr_stoim, ref matr_zatr, vekt_pos, vekt_pok);
            int F = 0;
            for (int i = 0; i < matr_stoim.GetLength(0); i++)
            {
                for (int j = 0; j < matr_stoim.GetLength(1); j++)
                {
                    if(matr_stoim[i,j] == 2147483647)
                    {
                        matr_stoim[i, j] = 0;
                    }
                }
            }
            for (int i = 0; i < matr_stoim.GetLength(0); i++)
            {
                for (int j = 0; j < matr_stoim.GetLength(1); j++)
                {
                    F += matr_stoim[i, j] * matr_zatr[i, j];
                }
            }
            using (StreamWriter sw = new StreamWriter("ПотенциальныйИтог.txt"))
            {
                sw.WriteLine("Итоговая матрица распределения: ");
                for(int i = 0; i<matr_zatr.GetLength(0); i++)
                {
                    for(int j = 0; j<matr_zatr.GetLength(1); j++)
                    {
                        sw.Write(matr_zatr[i, j] + "|");
                    }
                    sw.WriteLine();
                }
                sw.WriteLine("Матрица стоимостей: ");
                for (int i = 0; i < matr_stoim.GetLength(0); i++)
                {
                    for (int j = 0; j < matr_stoim.GetLength(1); j++)
                    {
                        sw.Write(matr_stoim[i, j] + "|");
                    }
                    sw.WriteLine();
                }
                sw.WriteLine("Сумма распределения: " + F);
            }
        }
        struct Delta
        {
            public int i;
            public int j;
            public int weigth;
        }
        void Flrd(out int[,] array1, out int[] vektor_postavok, out int[] vektor_pokupok, string path)
        {
            List<int> vctpzp = new List<int>();
            List<int> vctstvzp = new List<int>();
            using(StreamReader sr = new StreamReader(path))
            {
                string[] str1 = sr.ReadToEnd().Split('\n');
                for(int i = 0; i<str1.Length; i++)
                {
                    if(i == 0)
                    {
                        string[] str2 = str1[i].Split(';');
                        foreach(string st in str2)
                        {
                            if(st!="")
                            {
                                vctpzp.Add(Convert.ToInt32(st));
                            }
                        }
                    }
                    else
                    {
                        string[] str2 = str1[i].Split(';');
                        if (str2 != null && str2[0] != "")
                        {
                            vctstvzp.Add(Convert.ToInt32(str2[0]));
                        }
                    }
                }
                vektor_pokupok = vctpzp.ToArray();
                vektor_postavok = vctstvzp.ToArray();
                array1 = new int[vektor_pokupok.Length, vektor_postavok.Length];
                for(int i = 1; i<str1.Length; i++)
                {
                    string[] str2 = str1[i].Split(';');
                    for(int j = 1; j<str2.Length; j++)
                    {
                        array1[i - 1, j - 1] = Convert.ToInt32(str2[j]);
                    }
                }
                for(int i = 0; i<array1.GetLength(0); i++)
                {
                    for(int j = 0; j<array1.GetLength(1); j++)
                    {
                        if(array1[i,j] == 0)
                        {
                            array1[i, j] = 2147483647;
                        }
                    }
                }
            }
        }
        void MinElement(int[,] array, out int index1, out int index2, int[] vec1, int[] vec2)
        {
            index1 = 0;
            index2 = 0;
            int minElement = 2147483647;
            for (int i = 0; i < array.GetLength(0); i++)
            {
                if (vec1[i] != 0)
                {
                    for (int j = 0; j < array.GetLength(1); j++)
                    {
                        if (vec2[j] != 0)
                        {
                            if (minElement > array[i, j])
                            {
                                minElement = array[i, j];
                                index1 = i;
                                index2 = j;
                            }
                        }
                    }
                }
            }
        }
        void ItogovayaFunkciya(int[,] array1, out int[,] array2, int[] vektor_postavok, int[] vektor_pokupok)
        {
            array2 = new int[vektor_postavok.Length, vektor_pokupok.Length];
            int index1, index2;
            for (int i = 0; i < array2.GetLength(0); i++)
            {
                for (int j = 0; j < array2.GetLength(1); j++)
                {
                    MinElement(array1, out index1, out index2, vektor_postavok, vektor_pokupok);

                    if (vektor_postavok[index1] != 0 && vektor_pokupok[index2] != 0)
                    {
                        if (vektor_postavok[index1] >= vektor_pokupok[index2])
                        {
                            array2[index1, index2] = vektor_pokupok[index2];
                            vektor_postavok[index1] -= vektor_pokupok[index2];
                            vektor_pokupok[index2] = 0;
                        }
                        else
                        {
                            array2[index1, index2] = vektor_postavok[index1];
                            vektor_pokupok[index2] -= vektor_postavok[index1];
                            vektor_postavok[index1] = 0;
                        }
                    }
                }
            }
        }
        void Potent(int[,] array1, ref int[,] array2, int[] vektor_postavok, int[] vektor_pokupok)
        {
            ItogovayaFunkciya(array1, out array2, vektor_postavok, vektor_pokupok);
            do
            {
                int[] potV = new int[vektor_pokupok.Length];
                int[] potL = new int[vektor_postavok.Length];
                int indi = 2147483647, indj = 0, indjay = 0;
                if (Count(array2) > array1.GetLength(0) + array1.GetLength(1) - 1)
                {
                    for (int i = 0; i < array1.GetLength(0); i++)
                    {
                        for (int j = 0; j < array1.GetLength(1); j++)
                        {
                            if (array1[i, j] <= indi)
                            {
                                indi = array1[i, j];
                                indj = i;
                                indjay = j;
                            }
                        }
                    }
                }
                for (int i = 0; i < array1.GetLength(0); i++)
                {
                    for (int j = 0; j < array1.GetLength(1); j++)
                    {
                        if (array2[i, j] != 0)
                        {
                            if (i == 0)
                            {
                                potL[j] = array1[i, j];
                            }
                            else
                            {
                                if (potV[i] == 0)
                                {
                                    potV[i] = array1[i, j] - potL[j];
                                }
                                else if (potL[j] == 0)
                                {
                                    potL[j] = array1[i, j] - potV[i];
                                }
                            }
                        }
                        else if (i == indj && j == indjay)
                        {
                            if (potV[i] == 0)
                            {
                                potV[i] = array1[i, j] - potL[j];
                            }
                            else if (potL[j] == 0)
                            {
                                potL[j] = array1[i, j] - potV[i];
                            }
                        }
                    }
                }
                List<Delta> delta = new List<Delta>();
                for (int i = 0; i < array1.GetLength(0); i++)
                {
                    for (int j = 0; j < array1.GetLength(1); j++)
                    {
                        if (array2[i, j] == 0 && !(i == indj && j == indjay))
                        {
                            delta.Add(new Delta { i = i, j = j, weigth = potV[i] + potL[j] - array1[i, j] });
                        }
                    }
                }
                int dmax = 0, dmaxind = 0;
                foreach (Delta del in delta)
                {
                    if (del.weigth > 0)
                    {
                        if (del.weigth > dmax)
                        {
                            dmax = del.weigth;
                            dmaxind = delta.IndexOf(del);
                        }
                    }
                }
                if (dmax > 0)
                {
                    List<Delta> pererasp = new List<Delta>();
                    pererasp.Add(delta[dmaxind]);
                    int i = delta[dmaxind].i, j = delta[dmaxind].j;
                    bool vih = true;
                    //перераспределение должно быть тут
                    int mindel = 2147483647, mindelind = 0;
                    foreach (Delta d in pererasp)
                    {
                        if (d.weigth <= mindel)
                        {
                            mindel = d.weigth;
                            mindelind = pererasp.IndexOf(d);
                        }
                    }
                    for (int n = 0; n < array2.GetLength(0); n++)
                    {
                        for (int h = 0; h < array2.GetLength(1); h++)
                        {
                            foreach (Delta d in pererasp)
                            {
                                if (d.i == n && d.j == h)
                                {
                                    if (n % 2 == 0)
                                    {
                                        array2[n, h] += d.weigth;
                                    }
                                    else if (n % 2 != 0)
                                    {
                                        array2[n, h] -= d.weigth;
                                    }
                                }
                            }
                        }
                    }
                }
                else break;
            } while (true);
        }
        int Count(int[,] array2)
        {
            int ret = 0;
            foreach (int i in array2)
            {
                if (i != 0)
                {
                    ret++;
                }
            }
            return ret;
        }
    }
}