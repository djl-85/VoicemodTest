/// <summary>
/// The chat factory development.
/// </summary>
namespace VoiceModChat.Helpers
{
    using VoiceModChat.Model;

    /// <summary>
    /// Factory class for ChatMember classes.
    /// </summary>
    public static class ChatMemberFactory
    {
        /// <summary>
        /// Gets the chat member.
        /// </summary>
        /// <param name="chatMemberType">Type of the chat member.</param>
        /// <param name="nickname">The nickname.</param>
        /// <param name="address">The address.</param>
        /// <returns>
        /// A chat member instance.
        /// </returns>
        public static IChatMember GetChatMember(ChatMemberType chatMemberType, string nickname, string address)
        {
            switch (chatMemberType)
            {
                case ChatMemberType.Client:
                    return new ChatClient(nickname, address);
                case ChatMemberType.Server:
                    return new ChatServer(nickname, address);
                default:
                    return default;
            }
        }
    }
}
