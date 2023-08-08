using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ProjeInMemory.Classes;

namespace ProjeInMemory.Controllers
{
    public class IndexController : Controller
    {
        string strKisiler_Cache_Anahtar = "kisiler_key";


        private readonly IMemoryCache mcHafiza;
        public IndexController(IMemoryCache _mcHafiza)
        {
            mcHafiza = _mcHafiza;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult InMemoryCacheOlustur()
        {
            List<Kisiler>? liste = null;
            bool blnKontrol = mcHafiza.TryGetValue(strKisiler_Cache_Anahtar, out liste);
            if (!blnKontrol)
            {
                liste = new List<Kisiler>();
                for (int i = 0; i < 200; i++)
                {
                    liste.Add(new Kisiler() { kisi_ID = i, kisi_adi = $"İsim {i}" });
                }

                MemoryCacheEntryOptions mceoAyarlar = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(10),
                    Priority = CacheItemPriority.Normal
                };
                mcHafiza.Set(strKisiler_Cache_Anahtar, liste, mceoAyarlar);
            }
            else
            {
                object? objListe_Kisiler = mcHafiza.Get(strKisiler_Cache_Anahtar);
                if (objListe_Kisiler != null)
                {
                    liste = (List<Kisiler>)objListe_Kisiler;
                }
            }
            return View("~/Views/InMemoryCacheOlustur.cshtml", liste);
        }
    }
}
