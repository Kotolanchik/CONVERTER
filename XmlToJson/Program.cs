using System;
using Newtonsoft.Json;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;

namespace XmlToJson
{
    internal class Program
    {     
    



        static void Main(string[] args)
        {
            XmlDocument XMLdoc = new XmlDocument(); //создать объект XmlDocument   представляет весь xml-документ
            XMLdoc.Load("WHO IS THE GAME.xml"); // и затем загрузить в него xml-файл   представляет отдельный элемент. Наследуется от класса XmlNode
            XmlElement rootElement = XMLdoc.DocumentElement; //получаем корневой элемент документа 

            allBrands ALLBRANDS = new allBrands(); // список брендов


            int i = 0;
            foreach (XmlElement xnode in rootElement) // в нашем случае смотрим на три ячейки нашего списка
            {
                // счетчик элементов 

                XmlNode attribute = xnode.Attributes.GetNamedItem("id");// получаем атрибут id
                Console.WriteLine($"id: {attribute.Value}");//xmlnode представляет узел xml. В качестве узла может использоваться весь документ, так и отдельный элемент

                game GAME = new game();

                foreach (XmlNode childNode in xnode.ChildNodes)               //Свойство ChildNodes возвращает коллекцию дочерних узлов для данного узла
                {                                                             // обходим, а не смотрим смотрим дочерние элементы тега игры 
                    if (childNode.Name == "title") // Свойство Name возвращает название узла. Например, <user> - значение свойства Name равно "user"                    
                        GAME.title = childNode.InnerText; //Console.WriteLine($"Title: {childNode.InnerText}");      // Свойство InnerText возвращает текстовое значение узла

                    //  if (childNode.Name == "brand")                    
                    //       BrandInfo.nameBrand = childNode.InnerText;   // Console.WriteLine($"Brand: {childNode.InnerText}");


                    if (childNode.Name == "genres")
                        if (childNode.ChildNodes[0] != null) // смотрим есть ли список
                        {
                            genres buff_genres = new genres();
                            int j = 0;// счетчик элементов
                            bool b = true;
                            while (b)
                            {
                                if (childNode.ChildNodes[j].Name == "genre")
                                {
                                    buff_genres.genre.Add(childNode.ChildNodes[j].InnerText);

                                    // var genresss = childNode.ChildNodes[j];// конкретный элемент genres 
                                    // Console.WriteLine($"жанр: {genresss.InnerText}");
                                }
                                ++j;
                                if (childNode.ChildNodes[j] == null) b = false;// проверяем вышли ли мы за границу
                            }
                            GAME.Genres = buff_genres;
                        }


                    if (childNode.Name == "release")
                    {
                        GAME.releaseYear = childNode.InnerText;
                        //Console.WriteLine($"Release: {childNode.InnerText}");
                    }


                    if (childNode.Name == "brand")
                    {
                        bool checkBrand = true;


                        for (int elem = 0; elem < ALLBRANDS.AllBrands.Count && ALLBRANDS.AllBrands.Count != 0; elem++) /// проверяем бренд
                            if (ALLBRANDS.AllBrands[elem].nameBrand == childNode.InnerText) // смотрим есть ли такой бренд в списке
                            {
                                checkBrand = false;
                                ALLBRANDS.AllBrands[elem].game.Add(GAME);// если есть т добавляем игру
                                break;

                            }






                        if (i == 0 || checkBrand == true)
                        {// создаем новый элемент списка конечного класса
                            brandInfo BRand = new brandInfo();
                            BRand.game.Add(GAME);
                            BRand.nameBrand = childNode.InnerText;
                            ALLBRANDS.AllBrands.Add(BRand);

                        }
                    }


                } // forech 2

                ++i;
            }//forech 1



            string json = System.Text.Json.JsonSerializer.Serialize(ALLBRANDS, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine(json);



        }//main
    } //programm

    public class game
    {
        public string title { get; set; }
        public genres Genres { get; set; }
        public string releaseYear { get; set; }
    }
    public class genres
    {
        public List<string> genre { get; set; } = new List<string>();
    }//???
    public class brandInfo
    {
        public string nameBrand { get; set; }
        public List<game> game { get; set; } = new List<game>();
    }
    public class allBrands
    {
        public List<brandInfo> AllBrands { get; set; } = new List<brandInfo>();
    }
}