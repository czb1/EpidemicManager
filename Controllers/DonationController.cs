﻿using Microsoft.AspNetCore.Mvc;
using EpidemicManager.Models;
using System.Collections.Generic;
using System.Data;
using System;

namespace EpidemicManager.Controllers
{
    public class DonationController : Controller
    {
      
        
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult DonateMaterial()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DonateMaterial(DonateMaterialModel Material)
        {
                DonateMaterialModel material = new DonateMaterialModel();
                material.people_id = Request.Form["people_id"];
                material.type = Request.Form["type"];
                material.amount = Convert.ToInt32(Request.Form["amount"]);
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                string time = DateTime.Now.ToString("T");
                Sql.Execute("INSERT INTO donate_info(date,time,people_id)  VALUES(@0,@1,@2)",date,time,material.people_id);
                var ids = Sql.Read("SELECT max(donate_id) FROM donate_info ");
                int donate_id = 0;
                int is_distributed = 0;
                foreach (DataRow id in ids)
                {
                    donate_id = Convert.ToInt32(id[0]);
                }
                Sql.Execute("INSERT INTO donate_material VALUES(@0,@1,@2,@3,@4,@5)",donate_id, date, time, material.type, material.amount, is_distributed);
                return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult DonateMoney(DonateMoneyModel Money)
        {

            Models.DonateMoneyModel money = new DonateMoneyModel();
            money.number = Convert.ToInt32(Request.Form["number"]);
            money.people_id = Request.Form["people_id"];
           
            string date = DateTime.Now.ToString("yyyy-MM-dd");
                string time = DateTime.Now.ToString("T");
                Sql.Execute("INSERT INTO donate_info(date,time,people_id) VALUES(@0, @1, @2)",date,time,money.people_id);
                var ids = Sql.Read("SELECT max(donate_id) FROM donate_info "); 
                int donate_id = 0;
                int is_destributed = 0;
                foreach (DataRow id in ids)
                {
                    donate_id = Convert.ToInt32(id[0]);
                }
                Sql.Execute("INSERT INTO donate_money VALUES(@0,@1,@2,@3,@4)",donate_id, date, time, money.number, is_destributed);
                return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult DonateMoney()
        {
            return View();
        }
        
        public IActionResult Distribute()
        {
            var documentation = Sql.Read("SELECT * FROM distribute");
            var LDIds= new List<string>();
            var LNames = new List<string>();
            var LDates = new List<string>();
            var LTimes = new List<string>();
            var LMIds = new List<string>();
            int Inum = 0;
            foreach (DataRow record in documentation)
            {
                LDIds.Add(record[0].ToString());
                LNames.Add(record[1].ToString());
                LDates.Add(Convert.ToDateTime(record[2]).ToString("yyyy-MM-dd"));
                LTimes.Add(record[3].ToString());
                LMIds.Add(record[4].ToString());
                Inum++;
            }
            var model = new DistributeModel
            {
                DIds = LDIds,
                Names=LNames,
                Dates=LDates,
                Times=LTimes,
                MIds=LMIds,   
                num=Inum,
            };
            return View(model);
        }
    }
}
