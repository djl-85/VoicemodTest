/// <summary>
/// The chat client development.
/// </summary>
namespace VoiceModChat.Helpers
{
    using System;
    using System.Linq;
    using System.Net.WebSockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// This class implements the functionality related to a client.
    /// </summary>
    public class ChatClient : ChatMember, IDisposable
    {
        #region Constants

        /// <summary>
        /// The receive buffer size (1 MB).
        /// </summary>
        private const int ReceiveBufferSize = 1048576;

        #endregion

        #region Fields

        /// <summary>
        /// The server address.
        /// </summary>
        private string serverAddress;

        /// <summary>
        /// The client web socket.
        /// </summary>
        private ClientWebSocket clientWebSocket;

        /// <summary>
        /// The cancellation token source.
        /// </summary>
        private CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// The closing cancellation token.
        /// </summary>
        private CancellationToken closingCancellationToken;

        /// <summary>
        /// The disposed flag.
        /// </summary>
        private bool disposed;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatClient" /> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="serverToConnect">The server to connect.</param>
        /// <exception cref="ArgumentNullException">serverToConnect</exception>
        public ChatClient(string userName, string serverToConnect)
            : base(userName, false)
        {
            if (serverToConnect == null)
            {
                throw new ArgumentNullException(nameof(serverToConnect));
            }

            this.serverAddress = serverToConnect;
            this.clientWebSocket = new ClientWebSocket();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the cancellation token source.
        /// </summary>
        /// <value>
        /// The cancellation token source.
        /// </value>
        private CancellationTokenSource CancellationTokenSource 
        {
            get => this.cancellationTokenSource;
            set
            {
                this.cancellationTokenSource?.Dispose();
                this.cancellationTokenSource = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts the chat member.
        /// </summary>
        public override async Task StartMember()
        {
            this.CancellationTokenSource = new CancellationTokenSource();

            // Sets as header the user name in order to provide it to the server.
            this.clientWebSocket.Options.SetRequestHeader(nameof(this.UserName), this.UserName);

            await this.clientWebSocket.ConnectAsync(new Uri(this.serverAddress), this.CancellationTokenSource.Token);

            this.ReveiveAsync();
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
        /// Sends the given message.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        /// <returns>The task related to the send procedure.</returns>
        /// <exception cref="ArgumentNullException">message</exception>
        protected override async Task SendAsyncInternal(string message)
        {
            if (this.clientWebSocket.State != WebSocketState.Closed
                && this.clientWebSocket.State != WebSocketState.CloseReceived)
            {
                await this.clientWebSocket.SendAsync(
                    new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)),
                    WebSocketMessageType.Text,
                    true,
                    this.CancellationTokenSource.Token);
            }
        }
        /// <summary>
        /// Stops the chat client member.
        /// </summary>
        /// <returns>
        /// The task related to the stop procedure.
        /// </returns>
        public override void StopMember(CancellationToken cancellationToken)
        {
            this.closingCancellationToken = cancellationToken;
            this.CancellationTokenSource?.Cancel();
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
                this.CancellationTokenSource = null;
                this.clientWebSocket.Dispose();
            }
        }

        /// <summary>
        /// Reveives messages.
        /// </summary>
        /// <returns>The task that receives messages.</returns>
        private async Task ReveiveAsync()
        {
            bool isCloseRequestedFromServer = false;

            CancellationToken cancellationToken = this.CancellationTokenSource.Token;
            ArraySegment<byte> receiveBuffer = new ArraySegment<byte>(new byte[ReceiveBufferSize]);

            try
            {
                while (this.clientWebSocket.State == WebSocketState.Open || this.clientWebSocket.State == WebSocketState.CloseSent)
                {
                    WebSocketReceiveResult receiveResult = await this.clientWebSocket.ReceiveAsync(receiveBuffer, cancellationToken);

                    cancellationToken.ThrowIfCancellationRequested();

                    isCloseRequestedFromServer = receiveResult.MessageType == WebSocketMessageType.Close;

                    if (isCloseRequestedFromServer)
                    {
                        // The server sent the socket close.
                        break;
                    }

                    byte[] messageBytes = receiveBuffer.Skip(receiveBuffer.Offset).Take(receiveResult.Count).ToArray();
                    string receivedMessage = Encoding.UTF8.GetString(messageBytes);

                    Console.WriteLine(receivedMessage);

                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
            catch (OperationCanceledException)
            {
                // The task of receiving has been canceled.
            }

            if (!isCloseRequestedFromServer)
            {
                await this.clientWebSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "End session", this.closingCancellationToken);
            }
        }

        #endregion

    }
}
