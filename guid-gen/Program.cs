using System;

namespace TemplateGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0) {
                int.TryParse(args[0], out int times);
                for (var x = 0; x < times; x++)
                {
                    Console.WriteLine(Guid.NewGuid());
                }
            } else {
                Console.WriteLine(Guid.NewGuid());
            }
        }
    }
}
