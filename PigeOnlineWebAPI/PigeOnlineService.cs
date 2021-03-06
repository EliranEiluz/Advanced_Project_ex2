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


        // ***** Contacts & Chats *****


        /*
        * The function get username of contact and return the chat with him. 
        */
        public async Task<Chat> GetChatByUsername(string currentUser, string username)
        {
            var chat = await _context.Chat.Where(chat => chat.chatOwner.Username == currentUser && chat.ChatWith == username).FirstAsync();
            return chat;
        }

        /*
        * The function get username of current user and return his chats.
        */
        public async Task<List<Chat>> GetChatsByUsername(string currentUser)
        {
            List<Chat> lst = await _context.Chat.Where(a => a.chatOwner.Username == currentUser).ToListAsync();
            return lst;
        }

        /*
        * The function add new contact(chat) to the current user. 
        */
        public async Task<int> AddNewContact(string currentUser, string newUser, string name, string server)
        {
            User current = await _context.User.FindAsync(currentUser);

            Chat fromCurrentToUser = new Chat();
            fromCurrentToUser.ServerURL = server;
            fromCurrentToUser.ChatWith = newUser;
            fromCurrentToUser.DisplayName = name;
            fromCurrentToUser.Date = "";
            fromCurrentToUser.LastMessage = "";
            fromCurrentToUser.Image = "im3.jpg"; // from public dir.
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
            return 0;
        }

        /*
        * The function add new chat with the sender of invitation. 
        */
        public async Task<int> handleInvitation(string from, string to, string server)
        {
            User current = await _context.User.FindAsync(to);
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
            newChat.Image = "im3.jpg";
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

        /*
        * The function update chat details(display name / server) with contact. 
        */
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

        /*
        * The function delete contact(chat). 
        */
        public async Task<int> DeleteContactByUsername(string currentUser, string username)
        {
            var chat = await _context.Chat.Where(chat => chat.chatOwner.Username == currentUser && chat.ChatWith == username).FirstAsync();
            if (chat == null)
            {
                return 1;
            }
            _context.Chat.Remove(chat);
            await _context.SaveChangesAsync();
            return 0;
        }


        // ***** Messages *****


        /*
        * The function get messageID and return the message. 
        */
        public async Task<Message> GetMessageById(int messageID)
        {
            var message = await _context.Message.FindAsync(messageID);

            if (message == null)
            {
                return null;
            }
            return message;
        }

        /*
        * The function get contact name and return the messages of the current user with him. 
        */
        public async Task<List<Message>> GetMessagesWithContact(string currentUser, string username)
        {
            var chat = await _context.Chat.Where(chat => chat.chatOwner.Username == currentUser && chat.ChatWith == username).FirstAsync();
            List<Message> messages = await _context.Message.Where(message => message.chatOwnerId == chat.Id).ToListAsync();
            return messages;
        }

        /*
        * The function add new message which send by the current user. 
        */
        public async Task<int> postMessage(string currentUser, string contact, string content)
        {
            Chat chat = await GetChatByUsername(currentUser, contact);
            if(chat == null)
            {
                return 1;
            }
            Message message = new Message();
            message.From = currentUser;
            message.Content = content;
            message.Type = "text";
            message.Date = DateTime.Now.ToString();
            message.SenderPicture = "im3.jpg";
            message.chatOwnerId = chat.Id;
            _context.Message.Add(message);
            await _context.SaveChangesAsync();
            chat.LastMessage = content;
            chat.Date = DateTime.Now.ToString();
            _context.Chat.Update(chat);
            await _context.SaveChangesAsync();
            return 0;
        }

        /*
        * The function get transfer of message (and save the message). 
        */
        public async Task<int> Transfer(string from, string to, string content)
        {
            Chat chat = await GetChatByUsername(to, from);
            if(chat == null)
            {
                return 1;
            }
            Message message = new Message();
            message.From = from;
            message.Content = content;
            message.Type = "text";
            message.Date = DateTime.Now.ToString();
            message.SenderPicture = "im3.jpg";
            message.chatOwnerId = chat.Id;
            _context.Message.Add(message);
            await _context.SaveChangesAsync();
            chat.LastMessage = content;
            chat.Date = DateTime.Now.ToString();
            _context.Chat.Update(chat);
            await _context.SaveChangesAsync();
            return 0;
        }

        /*
        * The function get messageID and update the content. 
        */
        public async Task<int> UpdateMessageById(int messageID, string content)
        {
            var message = await _context.Message.FindAsync(messageID);
            if (message == null)
            {
                return 1;
            }
            message.Content = content;
            _context.Message.Update(message);
            await _context.SaveChangesAsync();
            return 0;
        }

        /*
        * The function get messageID and delete the message. 
        */
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


        // ***** Users (REGISTER/LOGIN/LOGOUT) *****


        /*
        * The function get username and return the user. 
        */
        public async Task<User> GetUser(string id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null || user.Username != id) // Find is not case sensitive.
            {
                return null;
            }

            return user;
        }

        /*
        * The function add new user (in REGISTER). 
        */
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

        /*
        * The function get username and add ConnectionId to the details of the user (in LOGIN). 
        */
        public async Task<int> InsertConnectionId(string username, string ConnectionId)
        {
            User user = await GetUser(username);
            if(user == null)
            {
                return 1;
            }
            user.ConnectionId = ConnectionId;
            _context.User.Update(user);
            await _context.SaveChangesAsync();
            return 0;
        }

        /*
        * The function get username and delete ConnectionId from the details of the user (in LOGOUT). 
        */
        public async Task<int> DeleteConnectionId(string username) 
        {
            User user = await GetUser(username);
            if(user == null)
            {
                return 1;
            }
            user.ConnectionId = null;
            _context.User.Update(user);
            await _context.SaveChangesAsync();
            return 0;
        }
        
    }
}
