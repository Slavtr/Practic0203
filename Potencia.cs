using System;
using System.Collections.Generic;
using System.Text;

namespace Матмоделирование_практика
{
    class Potencia
    {
        public Potencia(string path)
        {
            
        }
        struct Delta
        {
            public int i;
            public int j;
            public int weigth;
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
            int[] potV = new int[vektor_pokupok.Length];
            int[] potL = new int[vektor_postavok.Length];
            int indi = 2147483647, indj = 0, indjay = 0;
            if(Count(array2)>array1.GetLength(0)+array1.GetLength(1)-1)
            {
                for(int i = 0; i<array1.GetLength(0); i++)
                {
                    for(int j = 0; j<array1.GetLength(1); j++)
                    {
                        if(array1[i,j]<=indi)
                        {
                            indi = array1[i, j];
                            indj = i;
                            indjay = j;
                        }
                    }
                }
            }
            for(int i = 0; i<array1.GetLength(0); i++)
            {
                for(int j = 0; j<array1.GetLength(1); j++)
                {
                    if(array2[i,j] != 0)
                    {
                        if(potV[i]==0)
                        {
                            potL[j] = array1[i, j];
                        }
                        else
                        {
                            if(potV[i]==0)
                            {
                                potV[i] = array1[i, j] - potL[j];
                            }
                            else if(potL[j]==0)
                            {
                                potL[j] = array1[i, j] - potV[i];
                            }
                        }
                    }
                    else if(i == indj && j == indjay)
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
            foreach(Delta del in delta)
            {
                if(del.weigth>0)
                {
                    if(del.weigth>dmax)
                    {
                        dmax = del.weigth;
                        dmaxind = delta.IndexOf(del);
                    }
                }
            }
            if (dmax > 0) 
            {
                List<Delta> pererasp = new List<Delta>();
                pererasp.Add(delta[dmax]);
                int i = delta[dmaxind].i, j = delta[dmaxind].j;
                do
                {
                    for (int g = 0; g < array2.GetLength(0); g++)
                    {
                        for (int f = 0; f < array2.GetLength(1); f++)
                        {
                            if ((i == g && f != j) || (i != g && f == j))
                            {
                                pererasp.Add(new Delta { i = g, j = f, weigth = array2[g, f] });
                                i = g;
                                j = f;
                            }
                        }
                    }
                    if(i == delta[dmaxind].i && j == delta[dmaxind].j)
                    {
                        break;
                    }
                } while (true);
                int mindel = 2147483647, mindelind = 0;
                foreach (Delta d in pererasp)
                {
                    if(d.weigth<=mindel)
                    {
                        mindel = d.weigth;
                        mindelind = pererasp.IndexOf(d);
                    }
                }
                for(int n = 0; n<array2.GetLength(0); n++)
                {
                    for(int h = 0; h<array2.GetLength(1);h++)
                    {
                        foreach(Delta d in pererasp)
                        {
                            if(d.i == n && d.j == h)
                            {
                                if(n%2==0)
                                {
                                    array2[n,h]+=
                                }
                            }
                        }
                    }
                }
            }
        }
        int Count(int[,] array2)
        {
            int ret = 0;
            foreach(int i in array2)
            {
                if(i!=0)
                {
                    ret++;
                }
            }
            return ret;
        }
    }
}