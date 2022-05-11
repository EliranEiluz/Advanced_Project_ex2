

namespace PigeOnlineWebAPI;

public interface IPigeOnlineService
{
    async Task<Chat> GetChatByUsername(string currentUser, string username);
    async Task<List<User>> GetContactsByUserName(string currentUser);
    void AddNewContact(string currentUser, string newUser);
    async Task<User> GetContactByUsername(string username);
    void UpdateContactByUsername(string username);
    void DeleteContactByUsername(string username);
    async Task<List<Message>> GetMessagesByUsername(string currentUser, string username);
    void CreateMessageByUsername(string currentUser, string username, string message);
    async Task<Message> GetMessageById(int messageID);
    void UpdateMessageById(int messageID, string newContent);
    void DeleteMessageById(int messageID);
    void SendInvitation(string currentUser, string username, string url);
    void Transfer(string from, string to, string content);
}
