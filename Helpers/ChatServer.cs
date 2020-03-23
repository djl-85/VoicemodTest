/// <summary>
/// The chat server development.
/// </summary>
namespace VoiceModChat.Helpers
{
    using Fleck;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// This class implements the functionality related to a server.
    /// </summary>
    public class ChatServer : ChatMember, IDisposable
    {
        #region Fields

        /// <summary>
        /// The web socket server.
        /// </summary>
        private IWebSocketServer webSocketServer;

        /// <summary>
        /// The clients connected to the server.
        /// </summary>
        private ConcurrentDictionary<Guid, IWebSocketConnection> clients;

        /// <summary>
        /// The disposed flag.
        /// </summary>
        private bool disposed;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatServer" /> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="port">The port.</param>
        /// <param name="serverAddress">The server address.</param>
        public ChatServer(string userName, string serverAddress)
            : base(userName, true)
        {
            if (serverAddress == null)
            {
                throw new ArgumentNullException(nameof(serverAddress));
            }

            this.webSocketServer = new WebSocketServer(serverAddress) { RestartAfterListenError = true };
            this.clients = new ConcurrentDictionary<Guid, IWebSocketConnection>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts the chat server.
        /// </summary>
        /// <returns>
        /// The task related to the start process.
        /// </returns>
        public override Task StartMember()
        {
            this.webSocketServer.Start(socket =>
            {
                socket.OnOpen = async () =>
                {
                    await this.SendAsyncInternal($"The user {socket.ConnectionInfo.Headers[nameof(this.UserName)]} has connected!");
                    this.clients.TryAdd(socket.ConnectionInfo.Id, socket);
                };

                socket.OnClose = async () =>
                {
                    this.clients.TryRemove(socket.ConnectionInfo.Id, out _);
                    await this.SendAsyncInternal($"The user {socket.ConnectionInfo.Headers[nameof(this.UserName)]} has disconnected!");
                };

                socket.OnError = e => Console.Out.WriteLine($"The user {socket.ConnectionInfo.Headers[nameof(this.UserName)]} has disconnected!");

                socket.OnMessage = async message => await this.SendAsyncInternal(message);
            });

            return Task.CompletedTask;
        }

        /// <summary>
        /// Stops the chat member.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The task related to the stop procedure.
        /// </returns>
        public override void StopMember(CancellationToken cancellationToken)
        {
            this.clients.ForEach(client => client.Value.Close());
            this.clients.Clear();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            this.Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Stops the server.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        /// <returns>The task related to the send process.</returns>
        /// <exception cref="ArgumentException">messageToBeSent</exception>
        protected override async Task SendAsyncInternal(string message)
        {
            // Is a message from the server.
            Console.Out.WriteLine(message);

            List<Task> sendTasks = new List<Task>();

            foreach (IWebSocketConnection client in this.clients.Select(c => c.Value).Where(conn => conn.IsAvailable))
            {
                sendTasks.Add(client.Send(message));
            }

            await Task.WhenAll(sendTasks.ToArray());
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.disposed = true;
                this.webSocketServer.Dispose();
            }
        }

        #endregion
    }
}
