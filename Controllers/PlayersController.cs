using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WhatIfSports.Models;

namespace WhatIfSports.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly WhatIfSportsContext _context;

        public PlayersController(WhatIfSportsContext context)
        {
            _context = context;
        }

        // GET: api/Players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers(string lastName = "", string position = "", int age = 0, int ageMin = 0, int ageMax = 0)
        {
            var query = _context.Players.AsQueryable();
            if (!string.IsNullOrWhiteSpace(lastName))
                query = query.Where(p => p.last_name.ToLower() == lastName.ToLower());
            if (!string.IsNullOrWhiteSpace(position))
                query = query.Where(p => p.position.ToLower() == position.ToLower());
            if (age > 0)
                query = query.Where(p => p.age == age);
            if (ageMin > 0)
                query = query.Where(p => p.age >= ageMin && p.age != -1);
            if (ageMax > 0)
                query = query.Where(p => p.age <= ageMax && p.age != -1);
            return await query.ToListAsync();
        }

        // GET: api/Players/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);

            if (player == null)
            {
                return NotFound();
            }

            return player;
        }

        //There would normally be some authentication/authorization here
        [HttpPost]
        public async Task<ActionResult<String>> LoadPlayers()
        {
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://api.cbssports.com/fantasy/players/list?version=3.0&SPORT=basketball&response_format=JSON");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var returnObj = JsonConvert.DeserializeObject<CBSSportsPlayersReturnObject>(result);

            return result;
        }
    }
}
