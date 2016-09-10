﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalCore
{
    public class Class1
    {
        public string lowCal(string math)
        {
            try
            {
                int symMath = 0;
                if (math == "")//输入值为空，返回0
                {
                    return "0";
                }
                else
                {
                    if (math.Substring(0, 1) != "+" && math.Substring(0, 1) != "-")//开头补符号
                    {
                        math = "+" + math;
                    }
                    string mathCache = math;
                    while (mathCache != "")//检测数值个数
                    {
                        if (mathCache.Substring(0, 1) == "+" || mathCache.Substring(0, 1) == "-")
                        {
                            symMath++;
                        }
                        mathCache = mathCache.Substring(1, mathCache.Length - 1);
                    }
                    double[] eachMath;//每个数值相加得结果
                    eachMath = new double[symMath];//初始化数组
                    mathCache = math;//将"草稿math"值进行抄写
                    string numNC = "";//数值暂储
                    int numAC = 0;//累计加减次数
                    string sym = "";//符号变量初始化
                    while (mathCache != "")//填充
                    {
                        if (mathCache.Substring(0, 1) == "+" || mathCache.Substring(0, 1) == "-")
                        {
                            if (sym == "")//符号
                            {
                                sym = mathCache.Substring(0, 1);
                            }
                            else
                            {
                                eachMath[numAC] = Convert.ToDouble(sym + numNC);
                                numNC = "";
                                numAC++;
                                sym = mathCache.Substring(0, 1);
                            }
                        }
                        else
                        {
                            numNC += mathCache.Substring(0, 1);
                        }
                        mathCache = mathCache.Substring(1, mathCache.Length - 1);
                    }
                    eachMath[numAC] = Convert.ToDouble(sym + numNC);

                    //叠加所有数值
                    double calCache = 0;
                    for (int i = 0; i != symMath; i++)//有问题，暂留
                    {
                        calCache += eachMath[i];
                    }
                    return calCache.ToString();
                }
            }
            catch
            {
                return "Error";//返回报告未知错误
            }
        }

        public string Multiply(string formula)
        {
            try
            {
                ////////////////检测开头有没有符号，没有就加上
                if (formula.Substring(0, 1) != "+" && formula.Substring(0, 1) != "-")
                {
                    formula = "+" + formula;
                }

                ///////////////记乘除号
                string formulaTry = formula;
                int mulNum = 0;//乘除号个数
                while (formulaTry != "")
                {
                    if (formulaTry.Substring(0, 1) == "*" || formulaTry.Substring(0, 1) == "/")
                    {
                        mulNum++;
                    }
                    formulaTry = formulaTry.Substring(1, formulaTry.Length - 1);
                }
                int mulN = mulNum;//后面乘除法做准备

                ///////////////分组【先读数据后加计数】
                /*
                +12+23*34-45/-56=+12+23*+34-45/-56
                [+12],[+23],[*](标记),[+34],[-45],[/](标记),[-56]
                */
                string[] grp;//组
                grp = new string[99999];//上限，99999个数组
                int[] muNum;//乘除号所在组号
                muNum = new int[mulNum];
                formulaTry = formula;//算式的备份
                bool firstChk = false;//第一次分组
                string saveCache = "";//暂存变量
                int gn = 0;//组数
                mulNum = 0;//记乘号位置
                           //while (formulaTry.Substring(formulaTry.IndexOf("*"),1)!="+"&& formulaTry.Substring(formulaTry.IndexOf("*"), 1) != "-")//检测*/后面的符号
                while (formulaTry != "")//分组
                {
                    if (firstChk == false)//第一次的符号或者数字
                    {
                        firstChk = true;
                        saveCache += formulaTry.Substring(0, 1);
                        formulaTry = formulaTry.Substring(1, formulaTry.Length - 1);//去掉第一个字符
                    }
                    if (formulaTry.Substring(0, 1) == "*" || formulaTry.Substring(0, 1) == "/")
                    {//检测是否乘除号，是的话先 【检测*/后面是否+-号，否则加上+】
                     //存前面的，再存自己，true=false，计数++,乘除数计数器++，计入数组

                        //是乘号
                        if (formulaTry.Substring(1, 1) != "+" && formulaTry.Substring(1, 1) != "-")//后面有没有+-，没有？
                        {//*5
                            formulaTry = formulaTry.Substring(0, 1) + "+" + formulaTry.Substring(1, formulaTry.Length - 1);
                        }
                        grp[gn] = saveCache;
                        saveCache = "";
                        gn++;//等会需要加入组，组数先加1【计数++】
                        muNum[mulNum] = gn;
                        mulNum++;
                        grp[gn] = formulaTry.Substring(0, 1);
                        gn++;
                        firstChk = false;
                    }
                    else
                    {
                        if ((formulaTry.Substring(0, 1) == "-" || formulaTry.Substring(0, 1) == "+"))//检测是否加减号,是的话先把前面的cache存入数组再把自己加入cache，计数++
                        {
                            grp[gn] = saveCache;
                            gn++;//等会需要加入组，组数先加1【计数++】
                            saveCache = formulaTry.Substring(0, 1);
                        }
                        else
                        {//数字或小数点
                            saveCache += formulaTry.Substring(0, 1);
                        }
                    }
                    formulaTry = formulaTry.Substring(1, formulaTry.Length - 1);//去掉第一个字符
                }
                grp[gn] = saveCache;
                ////////////////做乘除法
                int doMul = 0;
                while (doMul < mulNum)
                {
                    grp[muNum[doMul] + 1] = calMu(grp[muNum[doMul] - 1], grp[muNum[doMul]], grp[muNum[doMul] + 1]);
                    grp[muNum[doMul] - 1] = "+0"; grp[muNum[doMul]] = "+0";
                    doMul++;
                }

                ///////////////滚雪球
                doMul = 1;
                while (doMul <= gn)
                {
                    grp[doMul] = grp[doMul - 1] + grp[doMul];
                    grp[doMul - 1] = "0";
                    doMul++;
                }
                return lowCal(grp[gn]);
            }
            catch
            {
                return "Error";
            }
        }


        private string calMu(string fir, string sym, string sec)
        {
            string res = "";
            if (sym == "*")
            {
                res = (Convert.ToDouble(fir) * Convert.ToDouble(sec)).ToString();
            }
            else
            {
                res = (Convert.ToDouble(fir) / Convert.ToDouble(sec)).ToString();
            }
            return res;
        }


        public string plus(string fir,string sym,string sec)
        {
            switch (sym)
            {
                case "*":
                    return (Convert.ToDouble(fir) * Convert.ToDouble(sec)).ToString();

                case "/":
                    return (Convert.ToDouble(fir) / Convert.ToDouble(sec)).ToString();
            }
            return "0";
        }
    }
}