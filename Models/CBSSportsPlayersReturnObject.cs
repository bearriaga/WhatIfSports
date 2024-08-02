namespace WhatIfSports.Models
{
    public class CBSSportsPlayersReturnObject
    {
        public string uri { get; set; }
        public int statusCode { get; set; }
        public CBSSportsPlayersReturnObjectBody body { get; set; }
        public string uriAllias { get; set; }
        public string statusMessage { get; set; }
    }

    public class CBSSportsPlayersReturnObjectBody
    {
        public IEnumerable<CBSSportsPlayer> players { get; set; }
    }

    public class CBSSportsPlayer
    {
        public int id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string position { get; set; }
        public int? age { get; set; }
    }
}
