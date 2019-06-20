using GoodDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoodDay.BLL.ViewModels
{
    public class BlockListViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string FriendId { get; set; }
        public string FriendName { get; set; }
        public string FriendSurname { get; set; }
        public string FriendEmail { get; set; }
        public string FriendPhone { get; set; }
        public string UserAvatar { get; set; }
        public BlockListViewModel(BlockList block)
        {
            if (block != null)
            {
                Id = block.Id;
                if (!String.IsNullOrEmpty(block.FriendId))
                {
                    FriendId = block.FriendId;
                }
                if (block.Friend != null)
                {
                    FriendName = block.Friend.Name;
                    FriendSurname = block.Friend.Surname;
                    FriendEmail = block.Friend.Email;
                    FriendPhone = block.Friend.Phone;
                    if (block.Friend.File != null)
                    {
                        UserAvatar = block.Friend.File.Path;
                    }
                    else UserAvatar = "\\Shared\\user.png";
                }
            }
        }
    }
}
