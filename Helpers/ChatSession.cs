/// <summary>
/// The chat session implementation.
/// </summary>
namespace VoiceModChat.Helpers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using VoiceModChat.Model;

    /// <summary>
    /// Represents a chat session.
    /// </summary>
    public class ChatSession
    {
        #region Constants

        /// <summary>
        /// The server address.
        /// </summary>
        private static string ServerAddress = "ws://0.0.0.0:{0}";

        /// <summary>
        /// The address to connect to, for clients.
        /// </summary>
        private static string AddressToConnectTo = "ws://localhost:{0}";

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatSession" /> class.
        /// </summary>
        /// <param name="chatMember">The chat member.</param>
        /// <exception cref="ArgumentNullException">chatMember</exception>
        public ChatSession(IChatMember chatMember)
        {
            if (chatMember == null)
            {
                throw new ArgumentNullException(nameof(chatMember));
            }

            this.ChatMember = chatMember;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the chat member.
        /// </summary>
        /// <value>
        /// The chat member.
        /// </value>
        public IChatMember ChatMember { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the chat member.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <returns>A chat member instance.</returns>
        public static IChatMember CreateChatMember(uint port, string nickname) =>
            NetworkHelper.CheckIfPortAvailable(port) ?
            ChatMemberFactory.GetChatMember(ChatMemberType.Server, nickname, ServerAddress.Format(port))
            : ChatMemberFactory.GetChatMember(ChatMemberType.Client, nickname, AddressToConnectTo.Format(port));

        /// <summary>
        /// Starts the chat session.
        /// </summary>
        /// <returns>
        /// The task related to the start chat session.
        /// </returns>
        public Task StartChatSession() => this.ChatMember?.StartMember();

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public Task SendMessageAsync(string message) => this.ChatMember?.SendAsync(message);

        /// <summary>
        /// Stops the chat session.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The task related to the stop procedure.
        /// </returns>
        public void StopChatSession(CancellationToken cancellationToken) => this.ChatMember?.StopMember(cancellationToken);

        #endregion
    }
}
