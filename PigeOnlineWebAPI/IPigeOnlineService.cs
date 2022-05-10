

namespace PigeOnlineWebAPI;

public interface IPigeOnlineService
{
    List<User> GetContactsByUserName(string currentUser);
    void AddNewContact(string currentUser, string newUser);
    User GetContactByUsername(string username);
    void UpdateContactByUsername(string username);
    void DeleteContactByUsername(string username);
    List<Message> GetMessagesByUsername(string currentUser, string username);
    void CreateMessageByUsername(string currentUser, string username, string message);
    Message GetMessageById(int messageID);
    void UpdateMessageById(int messageID, string newContent);
    void DeleteMessageById(int messageID);
    void SendInvitation(string currentUser, string username, string url);
    void Transfer(string from, string to, string content);
}
