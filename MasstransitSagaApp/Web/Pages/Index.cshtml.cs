using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IBus _bus;
        public IndexModel(IBus bus)
        {
            _bus = bus;
        }
        public void OnGet()
        {
            _bus.Publish<SendOrder>(new SendOrder() { Id = 1 });
        }
    }
}
