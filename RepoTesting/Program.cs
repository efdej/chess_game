using System;

namespace RepoTesting
{
    class Program
    {
        static void Main()
        {
            var test = new TestClass.TestClass();
            Console.WriteLine(test.Hello());
            Console.WriteLine("hi");
        }
    }
}
