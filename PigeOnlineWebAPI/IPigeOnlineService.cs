

namespace PigeOnlineWebAPI;

public interface IPigeOnlineService
{
    Task<Chat> GetChatByUsername(string currentUser, string username);
    Task<List<Chat>> GetChatsByUsername(string currentUser);
    Task<int> AddNewContact(string currentUser, string newUser, string name, string server);
    Task<int> handleInvitation(string from, string to, string server);
    Task<int> UpdateContactByUsername(string currentUser, string id, string server, string name);
    Task<int> DeleteContactByUsername(string currentUser, string username);
    Task<List<Message>> GetMessagesWithContact(string currentUser, string username);
    Task<Message> GetMessageById(int messageID);

    Task<int> postMessage(string currentUser, string contact, string content);

    Task<int> UpdateMessageById(int messageID, String content);
    Task<int> DeleteMessageById(int messageID);
    Task<int> Transfer(string from, string to, string content);

    ////////////////////////////////////////

    Task<User> GetUser(string id);
    Task<int> PostUser(User user);

    ////////////////////////////////////////
    
}
