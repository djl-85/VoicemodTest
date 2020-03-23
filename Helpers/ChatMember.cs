/// <summary>
/// The chat member base class.
/// </summary>
namespace VoiceModChat.Helpers
{
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class ChatMember : IChatMember
    {
        #region Constants

        /// <summary>
        /// The unknown user name.
        /// </summary>
        private const string UnknownUserName = "Unknown";
        
        #endregion

        #region Fields

        /// <summary>
        /// The user name.
        /// </summary>
        private string userName;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName => this.userName;

        /// <summary>
        /// Gets a value indicating whether this instance is a server.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is a server; otherwise, <c>false</c>.
        /// </value>
        protected bool IsServer { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatMember"/> class.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        protected ChatMember(string userName, bool isServer)
        {
            this.userName = userName.IsNullOrEmpty() ? UnknownUserName : userName;
            this.IsServer = isServer;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts the chat member.
        /// </summary>
        /// <returns>
        /// The task related to the start process.
        /// </returns>
        public abstract Task StartMember();

        /// <summary>
        /// Stops the chat member.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The task related to the stop procedure.
        /// </returns>
        public abstract void StopMember(CancellationToken cancellationToken);

        /// <summary>
        /// Sends the given message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        /// The task related to the message sending.
        /// </returns>
        public Task SendAsync(string message) => this.SendAsyncInternal(this.GetFullMessage(message));

        /// <summary>
        /// Sends the given message
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        /// The task related to the message sending.
        /// </returns>
        protected abstract Task SendAsyncInternal(string message);

        /// <summary>
        /// Gets the full message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>The user name with the given message appended.</returns>
        private string GetFullMessage(string message) => $"{this.UserName}: {message}";

        #endregion

    }
}
