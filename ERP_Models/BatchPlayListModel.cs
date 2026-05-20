using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Models
{
    public class BatchPlayListModel
    {
        public int batch_id {  get; set; }
        public string batch_name {  get; set; }
        public string playlist_title {  get; set; }
        public string playlist_key {  get; set; }
        public List<VideoModel> Videos { get; set; } = new List<VideoModel>();

    }
}
