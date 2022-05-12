

namespace PigeOnlineWebAPI;

public interface IPigeOnlineService
{
    Task<Chat> GetChatByUsername(string currentUser, string username);
    Task<List<User>> GetContactsByUserName(string currentUser);
    void AddNewContact(string currentUser, string newUser);
    Task<User> GetContactByUsername(string username);
    void UpdateContactByUsername(string username);
    void DeleteContactByUsername(string username);
    Task<List<Message>> GetMessagesByUsername(string currentUser, string username);
    void CreateMessageByUsername(string currentUser, string username, string message);
    Task<Message> GetMessageById(int messageID);
    void UpdateMessageById(int messageID, string newContent);
    void DeleteMessageById(int messageID);
    void SendInvitation(string currentUser, string username, string url);
    void Transfer(string from, string to, string content);

    void AddNewUser(User user);
}
