using GameOfLife.Interfaces;

namespace GameOfLife.Infrastructure
{
    internal class UserInputHandler : IInputHandler
    {
        public int GetFieldSize()
        {
            int[] presetSizes = { 10, 20, 30 };
            string[] options = presetSizes.Select(size => $"{size}x{size}").Concat(new[] {"Custom"}).ToArray();
            int selectedIndex = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Select field size:");
                for (int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine((i == selectedIndex ? ">> " : "   ") + options[i]);
                }

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.UpArrow)
                    selectedIndex = (selectedIndex - 1 + options.Length) % options.Length;
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                    selectedIndex = (selectedIndex + 1) % options.Length;
                else if (keyInfo.Key == ConsoleKey.Enter)
                    break;
            }

            return selectedIndex == options.Length - 1 ? GetCustomSize() : presetSizes[selectedIndex];

        }

        private static int GetCustomSize()
        {
            int customSize;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Enter custom field size (positive integer):");
                if (int.TryParse(Console.ReadLine(), out customSize) && customSize > 0)
                {
                    Console.Clear();
                    return customSize;
                }

            }
        }
    }
}
