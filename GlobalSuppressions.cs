// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Reliability", "CA2008:Do not create tasks without passing a TaskScheduler", Justification = "Not needed to specify.", Scope = "member", Target = "~M:VoiceModChat.Helpers.ChatClient.ReveiveAsync~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "It is good as it is.", Scope = "member", Target = "~M:VoiceModChat.Helpers.ChatClient.ReveiveAsync~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "It is good as it is.", Scope = "member", Target = "~M:VoiceModChat.Helpers.ChatClient.SendAsync(System.String)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "It is good as it is.", Scope = "member", Target = "~M:VoiceModChat.Helpers.ChatClient.Start(System.Threading.CancellationToken)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "It is good as it is.", Scope = "member", Target = "~M:VoiceModChat.Helpers.ChatClient.SendAsyncInternal(System.String)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "It is good as it is.", Scope = "member", Target = "~M:VoiceModChat.Helpers.ChatClient.Start")]
[assembly: SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "It is good as it is.", Scope = "member", Target = "~M:VoiceModChat.Helpers.ChatClient.StartMember")]
[assembly: SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "It is good as it is.", Scope = "member", Target = "~M:VoiceModChat.Helpers.ChatServer.SendAsyncInternal(System.String)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "It is good as it is.", Scope = "member", Target = "~M:VoiceModChat.Helpers.ChatServer.StartMember")]
[assembly: SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "It is good as it is.", Scope = "member", Target = "~M:VoiceModChat.Program.Main(System.String[])~System.Threading.Tasks.Task")]
