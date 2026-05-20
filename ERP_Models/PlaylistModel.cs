using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Models
{
    public class PlaylistModel
    {

        public int PlayListId { get; set; }
        public int BatchId { get; set; }
        public string BatchName { get; set; }
        public string PlayListKey { get; set; }
        public string PlayListTitle { get; set; }
        public List<VideoModel> Videos { get; set; } = new List<VideoModel>();
    }
}
