using IceSync.Models;
using IceSync.Models.Context;
using IceSync.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IceSync.Controllers
{
    public class WorkflowsController : Controller, IJob
    {
        const string Url = "https://api-test.universal-loader.com/";
        private readonly MyDbContext _context;
        private UnitOfWorks _unitOfWork;
       private string message = string.Empty;
        public WorkflowsController(MyDbContext context)
        {
            _context = context;
           _unitOfWork = new UnitOfWorks(_context);
        }

        public System.Threading.Tasks.Task Execute(IJobExecutionContext context)
        {
            var task = System.Threading.Tasks.Task.Run(() => GetWorkflows());
            return task;
        }
        public async Task<IActionResult> LoginUser(UniversalInfo universal)
        {

            string token = string.Empty;

            using (var httpClient = new HttpClient())
            {
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(universal), System.Text.Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync($"{Url}authenticate", stringContent))
                {
                    token = await response.Content.ReadAsStringAsync();

                    if (token == "ivalid credential")
                    {
                        message = "Incorect User And Password";
                        return BadRequest();
                    }
                    else
                    {
                        message = "Login Succes";
                        HttpContext.Session.SetString("JWToken", token);
                    }
                }
            }

            return Redirect("~/Workflows/Index");
        }
        public async Task<IActionResult> GetWorkflows()
        {
            _unitOfWork.GetRepository<Workflow>().DeleteAll();
            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = $"{Url}Workflows";
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            string jsonStr = await httpClient.GetStringAsync(url);
            List<Workflow> result= JsonConvert.DeserializeObject<List<Workflow>>(jsonStr).ToList();
            foreach (var i in result)
            {
                _unitOfWork.GetRepository<Workflow>().Add(new Workflow {Id=i.Id,Name=i.Name,IsActive=i.IsActive,IsRunning=i.IsRunning,MultiExecBehavior=i.MultiExecBehavior });
                _unitOfWork.SaveChanges();
            }
            return Redirect("~/Workflows/Index");
        } 
        public async Task<IActionResult> Index()
        {
            ViewBag.Message=message;
            return View(_unitOfWork.GetRepository<Workflow>().GetAll().ToList());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workFlowRun = await _context.Workflows
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workFlowRun == null)
            {
                return NotFound();
            }

            return View(workFlowRun);
        }
        public async System.Threading.Tasks.Task RunWorkflows(int Id)
        {
             message= "";
            _unitOfWork.GetRepository<Workflow>().DeleteAll();
            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = $"{Url}Workflows/{Id}/Run";
            HttpClient httpClient = new HttpClient();
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(Id), System.Text.Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            var result=  await httpClient.PostAsync(url, stringContent);
            if (result.IsSuccessStatusCode)
            {
                  ViewData["ToplamPersonal"] = "Hello";
            }
            else
            {
                Details(Id);
            }
            //await GetWorkflows();
          //return Redirect($"~/Workflows/Details/"+Id);
        }
    
        private bool WorkflowExists(int id)
        {
            return _context.Workflows.Any(e => e.Id == id);
        }
      
       
    }
}
