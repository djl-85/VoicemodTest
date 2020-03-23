# VoicemodTest
Architecture explanation:

- Factory design pattern: The static class ChatMemberFactory implements this desing pattern, where you can provide with an enum which object type do you want with the same arguments for any kind.

- Dependency injection: The ChatSession class represents a chat session and in its constructor receives an IChatMember instance. This class does not need to know what kind of chat member is the provided instance (server or client). 
			It just calls the needed methods to work.

- Class inheritance: The ChatMember abstract class, implements the interface IChatMember and some common methods for all classes that inherit from this class.
		     public Task SendAsync(string message): calls the protected method SendAsyncInternal with the user name in order to send every message with the user name. Then, the server does not need to save the user name of every client.

- ChatClient: Represents a client in the chat and inherits from ChatMember. Due to the Fleck library does not have a websocket client, I used the one provided by the .NET framework ClientWebSocket.

- ChatServer: Represents a server in the chat and inherits from ChatMember, but for the rest of the users is just another member in the chat conversation.
	      When a client connects to the server, in the websocket header is sent the user name, just to allow the server to print on the console and send the user init session message with the user name to the other users.

- NetworkHelper: It is a static class which basically has a method to determine whether a net port is used or not in the machine. This is used by the ChatSession class to know what kind of chat member needs to be built when the CreateChatMember method of this class is called.

