using PigeOnlineWebAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace PigeOnlineWebAPI
{
    public class PigeOnlineService : IPigeOnlineService
    {
        private readonly PigeOnlineWebAPIContext _context;

        public PigeOnlineService(PigeOnlineWebAPIContext context)
        {
            _context = context;
        }
        public async Task<User> AddNewContact(string currentUser, string newUser, string server)
        {
            User current = await _context.User.FindAsync(currentUser);
            User toAdd = await _context.User.FindAsync(newUser); // FindAsync not case sensitive.

            // This check applied in the client side.
            /*
            if(current.Chats.Find(e => e.ChatWith == newUser) == null) // != null
            {
                return 1;
            }
            */
            if (toAdd == null || toAdd.Username != newUser)
            {
                return null;
            }

            //int Id = _context.Chat.Max(e => e.Id) + 1;
            Chat fromCurrentToUser = new Chat();
            fromCurrentToUser.ServerURL = server;
            fromCurrentToUser.ChatWith = newUser;
            fromCurrentToUser.DisplayName = toAdd.DisplayName;
            fromCurrentToUser.Date = "";
            fromCurrentToUser.LastMessage = "";
            fromCurrentToUser.Image = toAdd.Image;
            fromCurrentToUser.chatOwner = current;
            current.Chats.Add(fromCurrentToUser);
            try
            {
                _context.Chat.Add(fromCurrentToUser);
            }
            catch (DbUpdateConcurrencyException)
            {
                return null;
            }
            await _context.SaveChangesAsync();
            // add code to invite the contact in his server(invitation method)
            return toAdd;

        }

        public void CreateMessageByUsername(string currentUser, string username, string message)
        {
            throw new NotImplementedException();
        }

        public void DeleteContactByUsername(string currentUser, string username)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteMessageById(int messageID)
        {
            var message = await _context.Message.FindAsync(messageID);
            if (message == null)
            {
                return 1;
            }

            _context.Message.Remove(message);
            await _context.SaveChangesAsync();

            return 0;
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

        public void SendInvitation(string currentUser, string username, string url)
        {
            throw new NotImplementedException();
        }

        public void Transfer(string from, string to, string content)
        {
            throw new NotImplementedException();
        }

        public void UpdateContactByUsername(string currentUser, string username)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UpdateMessageById(int messageID, Message message)
        {
            if (messageID != message.Id)
            {
                return 1;
            }

            _context.Entry(message).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Message.Any(e => e.Id == messageID))
                {
                    return 2;
                }
                else
                {
                    return 3;
                }
            }

            return 0;
        }

        public async Task<User> GetContactByUsername(string currentUser, string username)
        {
            throw new NotImplementedException();
        }

        public async Task<List<User>> GetContactsByUserName(string currentUser)
        {
            throw new NotImplementedException();
        }

        public async Task<Message> GetMessageById(int messageID)
        {
            var message = await _context.Message.FindAsync(messageID);

            if (message == null)
            {
                return null;
            }

            return message;
        }

        public async Task<List<Message>> GetMessagesByUsername(string currentUser, string username)
        {
            throw new NotImplementedException();
        }

        ////////////////////////////////////////////////////////

        public async Task<User> GetUser(string id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null || user.Username != id) // Find is not case sensitive.
            {
                return null;
            }

            return user;
        }

        public async Task<int> PostUser(User user)
        {
            _context.User.Add(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // If already exist.
                if (_context.User.Any(e => e.Username == user.Username))
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            return 0;
        }

        public async Task<int> DeleteUser(string id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return 1;
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return 0;
        }

        public async Task<List<Chat>> GetChatsByUsername(string currentUser)
        {
            var user = await _context.User.FindAsync(currentUser);
            if(user == null)
            {
                return null;
            }
            return user.Chats;
        }


        ////////////////////////////////////////////////////////


    }
}
