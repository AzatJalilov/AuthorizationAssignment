using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace AuthorizationAssignment
{
    public class SessionHub : Hub
    {
        public void Disconnect(string id)
        {
            Clients.Client(id).disconnect();
        }

        public static ConcurrentDictionary<string, MyUserType> MyUsers = new ConcurrentDictionary<string, MyUserType>();

        public override Task OnConnected()
        {
            MyUsers.TryAdd(Context.ConnectionId, new MyUserType() { ConnectionId = Context.ConnectionId, UserName = Context.User.Identity.Name });
            return base.OnConnected();
        }

        
        public override Task OnDisconnected(bool stopCalled)
        {
            MyUserType garbage;

            MyUsers.TryRemove(Context.ConnectionId, out garbage);
            
            return base.OnDisconnected(stopCalled);
        }

    }

    public class MyUserType
    {
        public string ConnectionId { get; set; }
        public string UserName { get; set; }
    }
}