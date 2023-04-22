using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 三角函数练习
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double 顶角 = 2;
            double 腰长 = 20;
            //等腰三角形，已知腰边和顶角，求底边长度
            double 底角 = (180 - 顶角) / 2;
            double 底边长度 = 2 * 腰长 * (Math.Cos(底角*Math.PI/180));
            Console.WriteLine(底边长度);
            Console.ReadKey();
          
            

        }
    }
}
