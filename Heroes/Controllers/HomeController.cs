﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using Heroes.Game;


namespace Heroes.Controllers
{

    public class HomeController : Controller
    {
        static Dictionary<String, Player> Players = new Dictionary<String, Player>();
        [NonAction]
        public void StartFight(List<Game.Player> players, string type)
        {
            Game.Player P1 = players[0]; //P1 atakujacy
            Game.Player P2 = players[1];
            P1.Fight(P2, type);
        }

        public IActionResult Fight()
        {

            var players = PlayersList();

            return View(players);


        }
        //zmienic na 1 funkcje

        public IActionResult AttackArcher()
        {

            var players = PlayersList();
            StartFight(players, "A");
            ViewData["Message"] = "archer";
            return View(players);


        }


        public IActionResult AttackKnight()
        {


            var players = PlayersList();
            StartFight(players, "K");
            ViewData["Message"] = "knight";
            return View(players);


        }
        public IActionResult AttackGryphon()
        {

            var players = PlayersList();
            StartFight(players, "G");
            ViewData["Message"] = "gryph";
            return View(players);


        }


        public IActionResult AttackDragon()
        {


            var players = PlayersList();
            StartFight(players, "D");
            ViewData["Message"] = "dragon";
            return View(players);


        }


        [NonAction]
        public List<Game.Player> PlayersList()
        {
            if (Game.test.MyGlobalVariable1 == null)
            {
                Game.Player P1 = new Game.Player("1");
                Game.Player P2 = new Game.Player("2");  //stworzenie 2 graczy do testow  
                Game.test.MyGlobalVariable1 = P1;
                Game.test.MyGlobalVariable1.PlayerArmy.Archers.AddReinforcements(19);
                Game.test.MyGlobalVariable1.PlayerArmy.Knights.AddReinforcements(19);
                Game.test.MyGlobalVariable1.PlayerArmy.Dragons.AddReinforcements(3);
                Game.test.MyGlobalVariable1.PlayerArmy.Gryphons.AddReinforcements(7);
                Game.test.MyGlobalVariable2 = P2;
                Game.test.MyGlobalVariable2.PlayerArmy.Gryphons.AddReinforcements(7);
                Game.test.MyGlobalVariable2.PlayerArmy.Dragons.AddReinforcements(7);
                Game.test.MyGlobalVariable2.PlayerArmy.Knights.AddReinforcements(7);
                Game.test.MyGlobalVariable2.PlayerArmy.Archers.AddReinforcements(7);
                return new List<Game.Player>{
     P1,
    P2,
            };
            }
            else return new List<Game.Player>{
    Game.test.MyGlobalVariable1,
    Game.test.MyGlobalVariable2,
            };

        }



        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Town()
        {
            Player p = Players[TempData.Peek("Player").ToString()];
            var buildings = from building in p.City select building;
            ViewData["Ore"] = p.Goods.Ore;
            ViewData["Gold"] = p.Goods.Gold;
            ViewData["Clay"] = p.Goods.Clay;
            ViewData["Wood"] = p.Goods.Wood;

            return View(buildings.Select(k => k.Value).ToList());
        }

        public IActionResult Hero()
        {
            Player p = Players[TempData.Peek("Player").ToString()];
            var army = p.PlayerArmy.Container;
            return View(army.ToList<Creature>());
        }

        [HttpPost]
        public IActionResult Build(string submit)
        {
            try
            {
                Players[TempData.Peek("Player").ToString()].City[submit].Levelup();
            }
            catch (Exception e)
            {
                TempData.Add("Message", e.Message);
            }

            return RedirectToAction("Town");
        }

        public IActionResult Error()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddPlayer(string Name)
        {
            if (!Players.ContainsKey("Player"))
            {
                TempData.Add("Player", Name);
                Players.Add(Name, new Player(Name));
            }
            return View();
        }

        [HttpGet]
        public void Produce()
        {
            //something doesnt work here :/

            var time = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;

            if (TempData.ContainsKey("Player"))
            {
                var buildings = from b in Players[TempData.Peek("Player").ToString()].City.Select(k => k.Value).ToList()
                                where b.Level > 0
                                select b;
                foreach (var b in buildings)
                {
                    if (time - b.LastProduce > b.Interval)
                    {
                        b.Produce();
                        b.Getresources();
                        b.LastProduce = time;
                    }
                }
            }
        }
    }
}
