using Newtonsoft.Json;
using System;
using System.IO;

namespace Cure_2._0
{
    public class UsageTracker
    {
        private const string UsageFile = "usage.json";
        private const int MaxChatMessages = 5;
        private const int MaxImageGenerations = 2;

        public int ChatMessageCount { get; private set; }
        public int ImageGenerationCount { get; private set; }
        public DateTime LastUsageDate { get; private set; }

        public UsageTracker()
        {
            LoadUsageData();
            if (LastUsageDate.Date != DateTime.Today)
            {
                ChatMessageCount = 0;
                ImageGenerationCount = 0;
                LastUsageDate = DateTime.Today;
                SaveUsageData();
            }
        }

        public bool IsChatLimitReached() => ChatMessageCount >= MaxChatMessages;
        public bool IsImageLimitReached() => ImageGenerationCount >= MaxImageGenerations;

        public void IncrementChatMessageCount()
        {
            ChatMessageCount++;
            SaveUsageData();
        }

        public void IncrementImageGenerationCount()
        {
            ImageGenerationCount++;
            SaveUsageData();
        }

        private void LoadUsageData()
        {
            if (File.Exists(UsageFile))
            {
                var json = File.ReadAllText(UsageFile);
                var data = JsonConvert.DeserializeObject<UsageData>(json);
                ChatMessageCount = data.ChatMessageCount;
                ImageGenerationCount = data.ImageGenerationCount;
                LastUsageDate = data.LastUsageDate;
            }
            else
            {
                LastUsageDate = DateTime.Today;
            }
        }

        private void SaveUsageData()
        {
            var data = new UsageData
            {
                ChatMessageCount = this.ChatMessageCount,
                ImageGenerationCount = this.ImageGenerationCount,
                LastUsageDate = this.LastUsageDate
            };
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(UsageFile, json);
        }

        private class UsageData
        {
            public int ChatMessageCount { get; set; }
            public int ImageGenerationCount { get; set; }
            public DateTime LastUsageDate { get; set; }
        }
    }
}
