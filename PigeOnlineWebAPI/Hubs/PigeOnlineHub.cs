using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace PigeOnlineWebAPI.Hubs

{
    public class PigeOnlineHub:Hub
    {
        private IPigeOnlineService _service;
        

        public PigeOnlineHub(IPigeOnlineService service)
        {
            _service = service;
        }


        /*
        * The function called when user send message -> and notify to 'MessageRecived' function of the target. 
        */
        public async Task MessageSent(TransferParams details)
        {
            User user = await _service.GetUser(details.To);
            if(user.ConnectionId != null)
            {
               await Clients.Client(user.ConnectionId).SendAsync("MessageRecived",details.From, details.To, details.Content);
            }
            
        }

        /*
        * The function called when user is online and insert his ConnectionId. 
        */
        public async Task DeclareOnline(string username)
        {
            await _service.InsertConnectionId(username, Context.ConnectionId);

        }

        /*
        * The function called when user is logout and delete his ConnectionId. 
        */
        public async Task LogOut(string username)
        {
            await _service.DeleteConnectionId(username);
        }
    }
}
