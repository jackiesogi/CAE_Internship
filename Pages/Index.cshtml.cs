//using FW_StorageM.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FW_StorageM.Pages
{
    //[Authorize]
    public class IndexModel : PageModel
    {
        //private readonly ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger;

        //public List<HistoryRecord> HistoryRecords { get; set; }

        //public IndexModel(ApplicationDbContext context, ILogger<IndexModel> logger)
        //{
        //    _context = context;
        //    _logger = logger;
        //}

        //public async Task OnGetAsync()
        //{
        //    HistoryRecords = await _context.HistoryRecords.ToListAsync();
        //    _logger.LogInformation("OnGetAsync method called.");
        //}
    }

}