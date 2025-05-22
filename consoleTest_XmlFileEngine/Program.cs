using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlFileEngine;

namespace consoleTest_XmlFileEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            Commands commands = new Commands(@"D:\User\Darbvisma\file\TestFile.xml");

            commands.CreateDirectory("test");

            Console.ReadLine();
        }
    }
}
