using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Reflection;
using Model;
using Core;
using Cache;

namespace ConsoleTest
{
    class Program
    {

        static void Main(string[] args)
        {
            
            Program p = new Program();
            p.Play();

        }

        public void Play() {
            
            var r = UserCache.GetInstance.Get("FF");
            Console.WriteLine(r);
            Console.ReadKey();
        }
    }
}
