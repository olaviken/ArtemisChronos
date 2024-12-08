using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Artemis_Chronos
{
    /*Variables: PascalCase
     Object and function: camelCase*/
    internal class createPrompt
    {
        private static List<string> artStyles = new List<string> 
        {
            "Realism", "Impressionism", "Cubism", "Surrealism", "Abstract Expressionism", "Pop Art",
        "Minimalism", "Futurism", "Dadaism", "Baroque", "Rococo", "Romanticism", "Neoclassicism",
        "Symbolism", "Art Nouveau", "Art Deco", "Constructivism", "De Stijl", "Expressionism",
        "Fauvism", "Neo-Impressionism", "Post-Impressionism", "Pointillism", "Naive Art", "Brutalism",
        "Hyperrealism", "Photorealism", "Conceptual Art", "Street Art", "Digital Art", "Bauhaus",
        "Op Art", "Outsider Art", "Kinetic Art", "Neo-Dada", "Psychedelic Art", "Suprematism",
        "Vorticism", "Tachisme", "Arte Povera", "Installation Art", "Folk Art", "Regionalism",
        "Ashcan School", "Luminism", "Orientalism", "Mannerism", "Tenebrism", "Magic Realism",
        "Gothic", "Renaissance", "Proto-Renaissance", "High Renaissance", "Mannerism", "Northern Renaissance",
        "Italian Renaissance", "Dutch Golden Age", "Caravaggisti", "Pre-Raphaelite", "Academicism",
        "Social Realism", "Synthetism", "Art Brut", "Neo-Expressionism", "Video Art", "Sound Art",
        "Graffiti", "Mosaic", "Collage", "Assemblage", "Ink Wash Painting", "Tempera", "Pastel Art",
        "Trompe-l'œil", "Fresco", "Engraving", "Woodcut", "Lithography", "Mezzotint", "Aquatint",
        "Etching", "Silkscreen Printing", "Contemporary Art", "Performance Art", "Land Art", "Environmental Art",
        "Outsider Art", "Hypermodernism", "Plein Air", "Automatism", "Synthetic Cubism", "Analytic Cubism",
        "Color Field Painting", "Action Painting", "Hard-Edge Painting", "Abstract Art", "Geometric Abstraction",
        "Figurative Art", "Narrative Art", "Typography Art", "Optical Art", "BioArt"
        };

        private static List<string> continent = new List<string>
        {
            "african continent", "asian continent", "european continent", "north-american continent", "south-american continent", "australian continent"
        };

        private static List<string> historicalPeriod = new List<string>
        {
            "after 2000", "between 1980 and 2000", "between 1950 and 1980", "between 1930 and 1950", "between 1900 and 1930", "between 1850 and 1900", "between 1800 and 1850", "between 1700 and 1800",
            "between 1500 and 1700", "between 1000 and 1500", "before 1000"
        };

        private static List<string> getRandomArtStyles()
        {
                Random randomStyle = new Random();
                List<string> styles = new List<string>();

                for (int i = 0; i < 3; i++)
                {
                    string Style = artStyles[randomStyle.Next(artStyles.Count)];
                    if (!styles.Contains(Style))
                    {
                        styles.Add(Style);
                    }
                }
                return styles;
        }

        private static string getRandomContinent()
        { 
                Random randomContinent = new Random();
                string Continent = continent[randomContinent.Next(continent.Count)];
                return Continent;
        }

        private static string getRandomHistoricalPeriod()
        {
            Random randomHistoricalPeriod = new Random();
            string RandomHistoricalPeriod = historicalPeriod[randomHistoricalPeriod.Next(historicalPeriod.Count)];
            return RandomHistoricalPeriod;
        }

        public static string createHistoricalPrompt()
        {
            DateTime today = DateTime.Now;
            string Today = today.ToString("dd/MM");
            string Continent = getRandomContinent();
            string HistoricalPeriod = getRandomHistoricalPeriod();
            string Prompt = $"Find a significant historical event that happened {Today} on the {Continent} {HistoricalPeriod}, and describe it briefly and in accordance with OpenAi guidelines (max 350 characters).";
            return Prompt;
        }

        public static string createArtworkConcept(string HistoricalEvent)
        {
            List<string> listArtstyles = getRandomArtStyles();
            string ArtworkConcept = $"Artemis Chronos is an AI that creates unique artworks inspired by historical events. Today’s artwork is based on {HistoricalEvent} using styles {listArtstyles[0]}, {listArtstyles[1]}, and {listArtstyles[2]}. Describe their interpretation and include hashtags like #AIArt, #NFT, and relevant dynamic tags. (max 700 characters)";
            return ArtworkConcept;
        }
    }
}
