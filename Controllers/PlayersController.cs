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
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers(string last_name = "", string position = "", int age = 0, int age_min = 0, int age_max = 0)
        {
            var query = _context.Players.AsQueryable();
            if (!string.IsNullOrWhiteSpace(last_name))
                query = query.Where(p => p.last_name.ToLower() == last_name.ToLower());
            if (!string.IsNullOrWhiteSpace(position))
                query = query.Where(p => p.position.ToLower() == position.ToLower());
            if (age > 0)
                query = query.Where(p => p.age == age);
            if (age_min > 0)
                query = query.Where(p => p.age >= age_min && p.age != -1);
            if (age_max > 0)
                query = query.Where(p => p.age <= age_max && p.age != -1);
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

            var uniquePositions = returnObj?.body.players.Select(p => p.position).Distinct().ToList();
            List<SportsPosition> positions = new List<SportsPosition>();
            foreach (var position in uniquePositions)
            {
                var sportsPosition = new SportsPosition()
                {
                    name = position,
                    averageAge = (int)returnObj?.body.players.Where(y => y.position == position && y.age != null).Select(y => y.age).Average()
                };
                positions.Add(sportsPosition);
            }
                        
            foreach (var cbsPlayer in returnObj.body.players)
            {
                Player player = new Player()
                {
                    id = cbsPlayer.id,
                    first_name = cbsPlayer.firstname,
                    last_name = cbsPlayer.lastname,
                    position = cbsPlayer.position,
                    age = cbsPlayer.age,
                    age_diff = (cbsPlayer.age != null) ? cbsPlayer.age - positions.First(x => x.name == cbsPlayer.position).averageAge : null
                };
                if (_context.Players.Any(x => x.id == player.id))
                    _context.Players.Update(player);
                else
                    _context.Players.Add(player);
            }

            _context.SaveChanges();

            return "Players Updated";
        }
    }
}
