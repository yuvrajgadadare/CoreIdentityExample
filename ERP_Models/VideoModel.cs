using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_Models
{
    public class VideoModel
    {
        public int content_video_id {  get; set; }
        public int topic_id {  get; set; }
        public string folder_id {  get; set; }
        public string topic_name {  get; set; }
        public string video_file_id {  get; set; }
        public string video_title {  get; set; }

        public string VideoId { get; set; }
        public string Title { get; set; }
        public string ThumbnailUrl { get; set; }

    }
}
