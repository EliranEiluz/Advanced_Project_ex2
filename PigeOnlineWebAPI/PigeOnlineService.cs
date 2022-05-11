using PigeOnlineWebAPI.Data;

namespace PigeOnlineWebAPI
{
    public class PigeOnlineService : IPigeOnlineService
    {
        private readonly PigeOnlineWebAPIContext _context;

        public PigeOnlineService(PigeOnlineWebAPIContext context)
        {
            _context = context;
        }
        public void AddNewContact(string currentUser, string newUser)
        {
            throw new NotImplementedException();
        }

        public void CreateMessageByUsername(string currentUser, string username, string message)
        {
            throw new NotImplementedException();
        }

        public void DeleteContactByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public void DeleteMessageById(int messageID)
        {
            throw new NotImplementedException();
        }

        public async Task<Chat> GetChatByUsername(string currentUser, string username)
        {
            var user = await _context.User.FindAsync(currentUser);
            if(user != null)
            {
                var chat = user.Chats.Find(c => c.ChatWith == username);
                return chat;
            }
            return null;
            
        }

        public User GetContactByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public List<User> GetContactsByUserName(string currentUser)
        {
            throw new NotImplementedException();
        }

        public Message GetMessageById(int messageID)
        {
            throw new NotImplementedException();
        }

        public List<Message> GetMessagesByUsername(string currentUser, string username)
        {
            throw new NotImplementedException();
        }

        public void SendInvitation(string currentUser, string username, string url)
        {
            throw new NotImplementedException();
        }

        public void Transfer(string from, string to, string content)
        {
            throw new NotImplementedException();
        }

        public void UpdateContactByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public void UpdateMessageById(int messageID, string newContent)
        {
            throw new NotImplementedException();
        }
    }
}
