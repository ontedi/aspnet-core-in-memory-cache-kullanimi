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
            return View("~/Views/Index.cshtml");
        }

        public IActionResult InMemoryCacheOlustur()
        {
            string strSonuc = string.Empty;
            List<Kisiler>? liste = null;
            bool blnKontrol = mcHafiza.TryGetValue(strKisiler_Cache_Anahtar, out liste);
            if (!blnKontrol)
            {
                liste = new List<Kisiler>();
                for (int i = 1; i < 200; i++)
                {
                    liste.Add(new Kisiler() { kisi_ID = i, kisi_adi = $"İsim {i}" });
                }

                liste[0].olusturulma_tarihi = DateTime.Now;

                MemoryCacheEntryOptions mceoAyarlar = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(10),
                    Priority = CacheItemPriority.Normal
                };
                mcHafiza.Set(strKisiler_Cache_Anahtar, liste, mceoAyarlar);

                strSonuc = "Cache içerisinde veri yoktu, cache oluşturuldu";
            }
            else
            {
                object? objListe_Kisiler = mcHafiza.Get(strKisiler_Cache_Anahtar);
                if (objListe_Kisiler != null)
                {
                    liste = (List<Kisiler>)objListe_Kisiler;
                    strSonuc = $"Cache içerisinde veri mevcut, veriler cache üzerinden okunarak alındı.";
                    strSonuc += $"<br />Bellekten okunan kişi sayısı: {liste.Count}";
                    strSonuc += $"<br />Bellek oluşturulma tarihi: {liste[0].olusturulma_tarihi}";
                    strSonuc += $"<br />Şuanki tarih: {DateTime.Now}";
                }
            }
            return View("~/Views/InMemoryCacheOlustur.cshtml", strSonuc);
        }

        public IActionResult InMemoryCacheListele()
        {
            List<Kisiler>? liste = new List<Kisiler>();
            object? objListe_Kisiler = mcHafiza.Get(strKisiler_Cache_Anahtar);
            if (objListe_Kisiler != null)
            {
                liste = (List<Kisiler>)objListe_Kisiler;
            }
            return View("~/Views/InMemoryCacheListele.cshtml", liste);
        }
    }
}
