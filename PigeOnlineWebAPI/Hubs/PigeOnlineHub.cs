using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace PigeOnlineWebAPI.Hubs

{
    [Authorize]
    public class PigeOnlineHub:Hub
    {
        private IPigeOnlineService _service;
        

        public PigeOnlineHub(IPigeOnlineService service)
        {
            _service = service;
        }

        public async Task MessageSent(TransferParams details)
        {
            User user = await _service.GetUser(details.To);
            if(user.ConnectionId != null)
            {
                await Clients.User(user.ConnectionId).SendAsync(details.From, details.To, details.Content);
            }
            
        }

        public async Task NewChat(InvitationParams details)
        {
            User user = await _service.GetUser(details.To);
            if (user.ConnectionId != null)
            {
                await Clients.User(user.ConnectionId).SendAsync(details.From, details.To, details.Server);
            }

        }

        public async Task DeclareOnline(string username)
        {
            await _service.InsertConnectionId(username, Context.ConnectionId.ToString());

        }
    }
}
