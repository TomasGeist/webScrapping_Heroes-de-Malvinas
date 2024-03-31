using ConsoleAppScrapper.Models;
using Microsoft.Extensions.Options;
using HtmlAgilityPack;
using System;
using System.Net.Http;
using System.Linq;
using ScrapySharp.Extensions;


 namespace ConsoleAppScrapper
{
    class Program
    {
        static async Task Main(String[] args)
        {
            string url = "https://www.veteranos.mindef.gov.ar/index.php";
            var httpCliente = new HttpClient();
            var html = httpCliente.GetStringAsync(url).Result;
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var heroesElement = htmlDocument.DocumentNode.CssSelect("td").Select(x=> x.InnerText);
            
            var lst = new List<String>();

            heroesElement.ToList().ForEach(e=>
            {
                lst.Add(e.ToString());
            });

            List<List<string>> partes = new List<List<string>>();
            for (int i = 0; i < lst.Count; i += 6) 
            {
                partes.Add(lst.GetRange(i, Math.Min(6, lst.Count - i)));
            }

             foreach (var parte in partes)
            {
                 Heroe oHeroe = new Heroe
                {

                    NombreCompleto = parte[0],
                    Documento = parte[1],
                    Fuerza = parte[2],
                    Grado = parte[3],
                    Vive = parte[4],
                    Condicion = parte[5]

                };

                 using (DbHeroesdemalvinasContext db = new DbHeroesdemalvinasContext())
                {

                    await db.Heroes.AddAsync(oHeroe);
                    await db.SaveChangesAsync();
                }



            }

            

        }

    }
}
