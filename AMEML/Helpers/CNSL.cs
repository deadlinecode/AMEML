namespace AMEML
{
    internal class CNSL
    {
        public static void ClearLine()
        {
            Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
        }

        public static void Info(params string[] msgs)
        {
            foreach (var msg in msgs)
            {
                if (msg.Trim().Length > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write(" INFO ");
                    Console.ResetColor();
                    Console.Write(" ");
                }
                Console.WriteLine(msg);
            }
        }

        public static Task<string> ConsoleMenu(string question, params string[] options)
        {
            var taskCompletionSource = new TaskCompletionSource<string>();

            Task.Run(() =>
            {
                int activeItem = 0;
                ConsoleKey key = ConsoleKey.Enter;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("? ");
                Console.ResetColor();
                Console.WriteLine(question);
                int top = Console.GetCursorPosition().Top;

                do
                {
                    for (int i = 0; i < options.Length; i++)
                    {
                        string option = options[i];
                        if (i == activeItem)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write(">");
                            Console.ResetColor();
                        }
                        else Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine($" {option}");
                        Console.ResetColor();
                    }

                    key = Console.ReadKey().Key;
                    if (key == ConsoleKey.UpArrow) activeItem--;
                    else if (key == ConsoleKey.DownArrow) activeItem++;
                    activeItem = activeItem < 0 ? 0 : activeItem > options.Length - 1 ? options.Length - 1 : activeItem;
                    for (int i = 0; i < options.Length; i++)
                    {
                        Console.SetCursorPosition(0, top + i);
                        ClearLine();
                    }
                    Console.SetCursorPosition(0, top);
                } while (key != ConsoleKey.Enter);

                Console.SetCursorPosition(question.Length + 4, top - 1);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(options[activeItem]);
                Console.ResetColor();

                taskCompletionSource.SetResult(options[activeItem]);
            });

            return taskCompletionSource.Task;
        }
    }
}