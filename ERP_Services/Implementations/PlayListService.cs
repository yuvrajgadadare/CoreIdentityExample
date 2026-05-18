using Dapper;
using ERP_Models;
using ERP_Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Services.Implementations
{
    public class PlayListService : IPlaylistService
    {
        private readonly DapperContext _context;
        public PlayListService(DapperContext context)
        {
            _context = context;
        }

        public async Task AddPlayList(PlaylistModel model)
        {
            var query = $"insert into tblplaylists(PlayListTitle,PlayListKey) values(@PlayListTitle,@PlayListKey)";
            using (var connection = _context.CreateConnection())
            {
                var rows = connection.Execute(query, new { PlayListTitle = model.PlayListTitle, PlayListKey = model.PlayListKey });
            }
        }

        public async Task<PlaylistModel> GetPlayList(int Id)
        {
            var query = $"SELECT * FROM tblplaylists where PlayListId={Id}";
            using (var connection = _context.CreateConnection())
            {
                var channels = await connection.QueryFirstAsync<PlaylistModel>(query);
                return channels;
            }
        }

        public async Task<List<PlaylistModel>> GetPlayLists()
        {
            var query = "SELECT * FROM tblplaylists";
            using (var connection = _context.CreateConnection())
            {
                var channels = await connection.QueryAsync<PlaylistModel>(query);
                return channels.ToList();
            }
        }
    }
}
