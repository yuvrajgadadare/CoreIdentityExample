using ERP_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Services.Interfaces
{
    public interface IPlaylistService
    {
        Task AddPlayList(PlaylistModel model);
        Task<List<PlaylistModel>> GetPlayLists();
        Task<PlaylistModel> GetPlayList(int Id);
    }
}
