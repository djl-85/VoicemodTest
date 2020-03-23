/// <summary>
/// The file that holds the entry point of the program.
/// </summary>
namespace VoiceModChat
{
    using System;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using VoiceModChat.Helpers;

    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        #region Constants

        /// <summary>
        /// The end session command.
        /// </summary>
        private const string EndSessionCommand = "close";

        #endregion

        #region Methods

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments given to the program.</param>
        /// <returns>The task related to the Main call.</returns>
        /// <exception cref="ArgumentNullException">args</exception>
        public static async Task Main(string[] args)
        {
            using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource())
            {
                if (args == null)
                {
                    throw new ArgumentNullException(nameof(args));
                }

                if (args.Length != 1)
                {
                    Console.Out.WriteLine("The given arguments are wrong, you must write the port number where the app should connect");
                    Console.Out.WriteLine($"Example: {Assembly.GetExecutingAssembly().GetName().Name} port");

                    return;
                }

                bool success = uint.TryParse(args[0], out uint port);

                if (!success)
                {
                    Console.Out.WriteLine("The port you provided is not a positive number!");
                    return;
                }

                Console.Out.WriteLine("Please, write your nick (if no nick provided, your name will be Unknown): ");
                string nickname = Console.In.ReadLine();

                // Creates a chat session.
                ChatSession chatSession = new ChatSession(ChatSession.CreateChatMember(port, nickname));

                await chatSession.StartChatSession();

                Console.Out.WriteLine("Connected!");

                string command = Console.ReadLine().Trim();
                while (!command.Equals(EndSessionCommand, StringComparison.Ordinal))
                {
                    await chatSession.SendMessageAsync(command);
                    command = Console.ReadLine();
                }

                chatSession.StopChatSession(cancellationTokenSource.Token);
            }
        }

        #endregion
    }
}
