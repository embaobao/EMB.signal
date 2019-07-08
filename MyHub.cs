using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace EMB.signal
{
    public class MyHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
        public void Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, message);
            SignalrServerToClient.BroadcastMessage("haha");
        }

        public void UpdateModel(ShapeModel clientModel)
        {
            clientModel.LastUpdatedBy = Context.ConnectionId;
            Clients.AllExcept(clientModel.LastUpdatedBy).updateShape(clientModel);
        }
    }
    public class SignalrServerToClient
    {
        static readonly IHubContext _myHubContext = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
        public static void BroadcastMessage(string message)
        {
            _myHubContext.Clients.All.broadcastMessage("服务器", message);
        }
    }

    public class ShapeModel
    {
        // We declare Left and Top as lowercase with 
        // JsonProperty to sync the client and server models
        [JsonProperty("left")]
        public double Left { get; set; }
        [JsonProperty("top")]
        public double Top { get; set; }
        // We don't want the client to get the "LastUpdatedBy" property
        [JsonIgnore]
        public string LastUpdatedBy { get; set; }
    }
}