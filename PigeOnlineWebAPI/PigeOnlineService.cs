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
        public async Task<int> AddNewContact(string currentUser, string newUser, string name, string server)
        {
            User current = await _context.User.FindAsync(currentUser);
            //User toAdd = await _context.User.FindAsync(newUser); // FindAsync not case sensitive.
            /*
            if (toAdd == null || toAdd.Username != newUser)
            {
                return null;
            }
            */

            //int Id = _context.Chat.Max(e => e.Id) + 1;
            Chat fromCurrentToUser = new Chat();
            fromCurrentToUser.ServerURL = server;
            fromCurrentToUser.ChatWith = newUser;
            fromCurrentToUser.DisplayName = name;
            fromCurrentToUser.Date = "";
            fromCurrentToUser.LastMessage = "";
            fromCurrentToUser.Image = "";
            fromCurrentToUser.chatOwner = current;
            try
            {
                _context.Chat.Add(fromCurrentToUser);
            }
            catch (DbUpdateConcurrencyException)
            {
                return 1;
            }
            await _context.SaveChangesAsync();
            // add code to invite the contact in his server(invitation method)
            return 0;

        }

        public async Task<int> handleInvitation(string from, string to, string server)
        {
            User current = await _context.User.FindAsync(to); // check if null.
            if(current == null)
            {
                return 1;
            }
            Chat newChat = new Chat();
            newChat.ServerURL = server;
            newChat.ChatWith = from;
            newChat.DisplayName = from;
            newChat.Date = "";
            newChat.LastMessage = "";
            newChat.Image = "";
            newChat.chatOwner = current;
            try
            {
                _context.Chat.Add(newChat);
            }
            catch (DbUpdateConcurrencyException)
            {
                return 1;
            }
            await _context.SaveChangesAsync();

            return 0;

        }


        public async Task<int> postMessage(string currentUser, string contact, Message message)
        {
            Chat chat = await GetChatByUsername(currentUser, contact);
            if(chat == null)
            {
                return 1;
            }
            message.chatOwnerId = chat.Id;
            _context.Message.Add(message);
            await _context.SaveChangesAsync();
            return 0;
        }

    public async Task<int> DeleteContactByUsername(string currentUser, string username)
        {
            var chat = await _context.Chat.Where(chat=>chat.chatOwner.Username == currentUser && chat.ChatWith == username).FirstAsync();
            if(chat == null) {
                return 1; 
            }
            _context.Chat.Remove(chat);
            await _context.SaveChangesAsync();
            return 0;
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
            var chat = await _context.Chat.Where(chat => chat.chatOwner.Username == currentUser && chat.ChatWith == username).FirstAsync();
            return chat;         
        }

        public void Transfer(string from, string to, string content)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UpdateContactByUsername(string currentUser, string id, string server, string name)
        {
            var chat = await _context.Chat.Where(chat => chat.chatOwner.Username == currentUser && chat.ChatWith == id).FirstAsync();
            if (chat == null)
            {
                return 1;
            }
            chat.ServerURL = server;
            chat.DisplayName = name;
            _context.Chat.Update(chat);
            await _context.SaveChangesAsync();
            return 0;
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


        public async Task<Message> GetMessageById(int messageID)
        {
            var message = await _context.Message.FindAsync(messageID);

            if (message == null)
            {
                return null;
            }

            return message;
        }

        public async Task<List<Message>> GetMessagesWithContact(string currentUser, string username)
        {
            var chat = await _context.Chat.Where(chat => chat.chatOwner.Username == currentUser && chat.ChatWith == username).FirstAsync();
            List<Message> messages = await _context.Message.Where(message => message.chatOwnerId == chat.Id).ToListAsync();
            return messages;
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
            List<Chat> lst = await _context.Chat.Where(a => a.chatOwner.Username == currentUser).ToListAsync();
            return lst;
        }


        ////////////////////////////////////////////////////////


    }
}
