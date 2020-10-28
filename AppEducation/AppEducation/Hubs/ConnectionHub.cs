using AppEducation.Models;
using AppEducation.Models.Users;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppEducation.Hubs
{
    // connectionhub
    public class ConnectionHub : Hub<IConnectionHub>
    {
        private readonly List<Room> _rooms;
        private readonly AppIdentityDbContext _context;
        public ConnectionHub(List<Room> rooms, AppIdentityDbContext context)
        {
            _rooms = rooms;
            _context = context;
        }
        public async Task Join(string username, string classid)
        {
            User usr = new User { UserName = username, ConnectionID = Context.ConnectionId ,IsCaller =false,InCall= false};
            Classes clr = _context.Classes.Find(classid);
            if (clr == null)
            {
                //do something if no class has been found
                return;
            }
            else
            {
                Room rm = GetRoomByClassID(classid);
                if (rm == null)
                {
                    usr.IsCaller = true;
                    _rooms.Add(new Room
                    {
                        RoomIF = clr,
                        UserCall = new List<User> { usr }
                    });
                    await SendUserListUpdate(GetRoomByClassID(classid));
                    await Clients.Client(usr.ConnectionID).initDevices(usr);
                }
                else
                {
                    rm.UserCall.Add(usr);
                    await SendUserListUpdate(rm);
                    await Clients.Client(usr.ConnectionID).initDevices(usr);
                    rm.UserCall.ForEach(async u =>
                    {
                        if( u != usr)
                        {
                            await Clients.Client(u.ConnectionID).NotifyNewMember(usr);
                        }
                    });
                    
                }
            }
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Room callingRoom = GetRoomByConnectionID(Context.ConnectionId);
            // Hang up any calls the user is in
            await HangUp(); // Gets the user from "Context" which is available in the whole hub

            // Remove the user
            callingRoom.UserCall.RemoveAll(u => u.ConnectionID == Context.ConnectionId);

            // Send down the new user list to all clients
            await SendUserListUpdate(callingRoom);

            await base.OnDisconnectedAsync(exception);
        }


        public async Task CallUser(User targetUser)
        {
            Room callingRoom = GetRoomByConnectionID(Context.ConnectionId);
            User callingUser = callingRoom.UserCall.SingleOrDefault(u => u.ConnectionID == Context.ConnectionId);
            // Make sure the person we are trying to call is still here
            if (targetUser == null)
            {
                // If not, let the caller know
                await Clients.Caller.CallDeclined(targetUser,"The user you called has left.");
                return;
            }
            // They are here, so tell them someone wants to talk
            await Clients.Client(targetUser.ConnectionID).IncomingCall(callingUser);
        }

        public async Task AnswerCall(bool acceptCall, User targetConnectionId)
        {
            Room callingRoom = GetRoomByConnectionID(Context.ConnectionId);
            User callingUser = callingRoom.UserCall.SingleOrDefault(u => u.ConnectionID == Context.ConnectionId);
            var targetUser = callingRoom.UserCall.SingleOrDefault(u => u.ConnectionID == targetConnectionId.ConnectionID);

            // This can only happen if the server-side came down and clients were cleared, while the user
            // still held their browser session.
            if (callingUser == null)
            {
                return;
            }

            // Make sure the original caller has not left the page yet
            if (targetUser == null)
            {
                await Clients.Caller.CallEnded(targetConnectionId, "The other user in your call has left.");
                return;
            }

            // Send a decline message if the callee said no
            if (acceptCall == false)
            {
                await Clients.Client(targetConnectionId.ConnectionID).CallDeclined(callingUser, string.Format("{0} did not accept your call.", callingUser.UserName));
                return;
            }

            callingUser.InCall = true;
            targetUser.InCall = true;
            // Make sure there is still an active offer.  If there isn't, then the other use hung up before the Callee answered.
            // Tell the original caller that the call was accepted
            await Clients.Client(targetConnectionId.ConnectionID).CallAccepted(callingUser);

            // Update the user list, since thes two are now in a call
            //await SendUserListUpdate();
        }

        public async Task HangUp()
        {
            Room callingRoom = GetRoomByConnectionID(Context.ConnectionId);
            User callingUser = callingRoom.UserCall.SingleOrDefault(u => u.ConnectionID == Context.ConnectionId);
            // if room is mine . Remove all user in call
            if (callingRoom.UserCall.Count == 1)
            {
                // do something
                Room rm = GetRoomByConnectionID(callingUser.ConnectionID);
                _context.Classes.Remove(_context.Classes.Find(rm.RoomIF.ClassID));
            }
            // do something if not
            if (callingUser == null)
            {
                return;
            }

            // Send a hang up message to each user in the call, if there is one
            if (callingRoom != null)
            {
                foreach(User user in callingRoom.UserCall.Where(u => u.ConnectionID != callingUser.ConnectionID))
                {
                    await Clients.Client(user.ConnectionID).CallEnded(callingUser, string.Format("{0} has hung up.", callingUser.UserName));
                }
            }
            await SendUserListUpdate(callingRoom);
        }

        // WebRTC Signal Handler
        public async Task SendSignal(string signal, string targetConnectionId)
        {
            Room callingRoom = GetRoomByConnectionID(Context.ConnectionId);
            User callingUser = callingRoom.UserCall.SingleOrDefault(u => u.ConnectionID == Context.ConnectionId);
            User targetUser = callingRoom.UserCall.SingleOrDefault(u => u.ConnectionID == targetConnectionId);
            // Make sure both users are valid
            if (callingUser == null || targetUser == null)
            {
                return;
            }

            // These folks are in a call together, let's let em talk WebRTC
            await Clients.Client(targetConnectionId).ReceiveSignal(callingUser, signal);
        }

        #region Private Helpers

        private async Task SendUserListUpdate(Room rm)
        {
            foreach (User u in rm.UserCall)
            {
                await Clients.Client(u.ConnectionID).UpdateUserList(rm.UserCall);
            }
        }
        private Room GetRoomByConnectionID(string cid)
        {
            Room matchingRoom = _rooms.SingleOrDefault(r => r.UserCall.SingleOrDefault(u => u.ConnectionID == cid) != null);
            return matchingRoom;
        }
        private Room GetRoomByClassID(string classid)
        {
            Room matchingRoom = _rooms.SingleOrDefault<Room>(r => r.RoomIF.ClassID == classid);
            return matchingRoom;
        }

        #endregion
    }

    public interface IConnectionHub
    {
        Task CallAccepted(User callingUser);
        Task CallDeclined(User u, string v);
        Task CallEnded(User targetConnectionId, string v);
        Task IncomingCall(User callingUser);
        Task initDevices(User UserCall);
        Task NotifyNewMember(User usr);
        Task ReceiveSignal(User callingUser, string signal);
        Task UpdateUserList(List<User> userCall);
    }
}
