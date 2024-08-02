namespace WhatIfSports.Models
{
    public class Player
    {
        public int id { get; set; }
        public required string first_name { get; set; }
        public required string last_name { get; set; }
        public required string position { get; set; }
        public int? age { get; set; }
        public string stub => first_name?[0] + last_name;
        public int? age_diff { get; set; }
    }
}
