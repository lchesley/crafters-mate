using EL;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;

namespace Test
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                WebClient webClient = new WebClient();                
                                               
                List<Recipe> recipes = new List<Recipe>();

                for (int i = 1; i < 64; i++)
                {
                    HtmlDocument doc = new HtmlDocument();
                    string address = "http://na.finalfantasyxiv.com/lodestone/playguide/db/recipe/?page=" + i.ToString();
                    doc.Load(webClient.OpenRead(address));

                    HtmlNode blockContainer = doc.DocumentNode.SelectSingleNode("//div[@class=\"w480_footer mb10\"]");                    
                    HtmlNodeCollection nodes = blockContainer.SelectNodes("//tbody//tr");

                    foreach (HtmlNode node in nodes)
                    {
                        Recipe recipe = new Recipe();

                        HtmlNode disciple = node.SelectSingleNode(".//div[@class=\"ic_link_txt\"]//a");
                        recipe.DiscipleOfTheHand = disciple.InnerText;

                        HtmlNode itemInfo = node.SelectSingleNode(".//a[@class=\"db_popup highlight\"]");
                        HtmlAttribute att = itemInfo.Attributes["href"];
                        recipe.ToolkitLink = att.Value;
                        recipe.Name = HttpUtility.HtmlDecode(itemInfo.InnerText);                        

                        HtmlNode recipeLevel = node.SelectSingleNode(".//td[@class=\"col_center tc\"]");
                        recipe.RecipeLevel = Convert.ToInt32(Regex.Replace(recipeLevel.InnerText, @"\t|\n|\r", "").Trim());

                        HtmlNode itemLevel = node.SelectSingleNode(".//td[@class=\"col_right tc\"]");
                        recipe.ItemLevel = itemLevel.InnerText == "-" ? 0 : Convert.ToInt32(itemLevel.InnerText);

                        List<string> characteristics = new List<string>();

                        if (recipe.ItemLevel > 50)
                        {
                            if (recipe.ItemLevel == 55)
                            {
                                characteristics.Add("1 star");
                            }

                            if (recipe.ItemLevel == 70)
                            {
                                characteristics.Add("2 star");
                            }

                            if (recipe.ItemLevel == 90 || recipe.ItemLevel == 95)
                            {
                                characteristics.Add("3 star");
                            }

                            if (recipe.ItemLevel == 110)
                            {
                                characteristics.Add("4 star");
                            }
                        }

                        //Get recipe details.
                        WebClient recipeClient = new WebClient();
                        HtmlDocument recipeDoc = new HtmlDocument();
                        string recipeAddress = "http://na.finalfantasyxiv.com" + recipe.ToolkitLink;
                        recipeDoc.Load(recipeClient.OpenRead(address));

                        recipe.Crystals = new List<Entity>();

                        recipe.Materials = new List<Entity>();


                        recipes.Add(recipe);
                    }
                }
                                                
                var serializer = new XmlSerializer(typeof(List<Recipe>));
                using (var stream = File.OpenWrite("C:\\data.xml"))
                {
                    serializer.Serialize(stream, recipes);
                }
            }
            catch (Exception ee)
            {
                // Log error (omitted for brevity)
            }
        }
    }
}