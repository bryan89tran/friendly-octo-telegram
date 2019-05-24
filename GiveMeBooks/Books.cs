using System;
using System.Collections.Generic;

namespace GiveMeBooks
{
    public class rootObject
    {
        public string Kind { get; set; }
        public int TotalItems { get; set; }
        public List<Item> Items { get; set; }
    }

    public class Item
    {
        public string Kind { get; set; }
        public string Id { get; set; }
        public string Etag { get; set; }
        public VolumeInfo VolumeInfo { get; set; }
    }

    public class VolumeInfo
    {
        public string Title { get; set; }
        public List<string> Authors { get; set; }
        public string Publisher { get; set; }
        public List<IndustryIdentifier> IndustryIdentifiers { get; set; }
        public int PageCount { get; set; }
        public ImageLinks ImageLinks { get; set; }
    }

    public class IndustryIdentifier
    {
        public string Type { get; set; }
        public string Identifier { get; set; }
    }

    public class ImageLinks
    {
        public string SmallThumbnail { get; set; }
        public string Thumbnail { get; set; }
    }

}