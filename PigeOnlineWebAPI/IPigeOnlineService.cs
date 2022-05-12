

namespace PigeOnlineWebAPI;

public interface IPigeOnlineService
{
    Task<Chat> GetChatByUsername(string currentUser, string username);
    Task<List<Chat>> GetChatsByUsername(string currentUser);
    Task<List<User>> GetContactsByUserName(string currentUser);
    Task<int> AddNewContact(string currentUser, string newUser, string server);
    Task<User> GetContactByUsername(string currentUser, string username);
    void UpdateContactByUsername(string currentUser, string username);
    void DeleteContactByUsername(string currentUser, string username);
    Task<List<Message>> GetMessagesByUsername(string currentUser, string username);
    void CreateMessageByUsername(string currentUser, string username, string message);
    Task<Message> GetMessageById(int messageID);
    Task<int> UpdateMessageById(int messageID, Message message);
    Task<int> DeleteMessageById(int messageID);
    void SendInvitation(string currentUser, string username, string url);
    void Transfer(string from, string to, string content);

    ////////////////////////////////////////


    Task<User> GetUser(string id);
    Task<int> PostUser(User user);
    Task<int> DeleteUser(string id);

    ////////////////////////////////////////
    
}
