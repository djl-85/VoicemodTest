
/// <summary>
/// The chat member interface.
/// </summary>
namespace VoiceModChat.Helpers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Class which defines the interface for any chat member.
    /// </summary>
    public interface IChatMember
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        string UserName { get; }

        /// <summary>
        /// Starts the chat member.
        /// </summary>
        /// <returns>
        /// The task related to the start process.
        /// </returns>
        Task StartMember();

        /// <summary>
        /// Stops the chat member.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The task related to the stop procedure.
        /// </returns>
        void StopMember(CancellationToken cancellationToken);

        /// <summary>
        /// Sends the given message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="webSocketConnection">The web socket connection.</param>
        /// <returns>
        /// The task related to the message sending.
        /// </returns>
        Task SendAsync(string message);
    }
}
