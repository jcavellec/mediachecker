using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using mediachecker.Models;

namespace mediachecker.Helpers
{
    public class FileHelper
    {
        private static string _name;
        
        public static List<MediaFileModel> GetContentFolder(string inputFolder)
        {
            var objs = Directory.GetFiles(inputFolder).Select(file =>
                new MediaFileModel
                {
                    Name = Path.GetFileNameWithoutExtension(file),
                    Extension = Path.GetExtension(file),
                    Size = new FileInfo(Path.GetFullPath(file)).Length / 1024 / 1024
                }).ToList();

            objs.AddRange(
                from folder in Directory.GetDirectories(inputFolder)
                from file in Directory.GetFiles(folder)
                select new MediaFileModel
                {
                    Name = Path.GetFileNameWithoutExtension(file),
                    Extension = Path.GetExtension(file),
                    Size = new FileInfo(Path.GetFullPath(file)).Length / 1024 / 1024
                });
            return objs;
        }

        public static void GetMetaDataFromList(List<MediaFileModel> mediaFiles)
        {
            if (mediaFiles == null) throw new ArgumentNullException(nameof(mediaFiles));
            foreach (var mediaFile in mediaFiles)
            {
                _name = mediaFile.Name.Trim();
                var dataArray = _name.Split('.');
                
                // TODO
                if (IsAudio(mediaFile))
                {
                                
                }
                
                
                if (IsMovie(mediaFile))
                {
                
                }
            }

        }

        private static bool IsAudio(MediaFileModel mediaFileModel)
        {
            return mediaFileModel.Extension switch
            {
                "mp3" => true,
                "aac" => true,
                "flac" => true,
                "wav" => true,
                "ogg" => true,
                "m4a" => true,
                _ => false
            };
        }
        
        private static bool IsMovie(MediaFileModel mediaFileModel)
        {
            return mediaFileModel.Extension switch
            {
                "mkv" => true,
                "mp4" => true,
                "avi" => true,
                "mpg" => true,
                "mpeg" => true,
                _ => false
            };
        }
    }
}