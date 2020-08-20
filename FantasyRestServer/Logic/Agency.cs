using FantasyRestServer.Models.Data;
using FantasyRestServer.Models.Mapping;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;


namespace FantasyRestServer.Logic
{
    public sealed class Agency
    {
        private static Random Rand = new Random();


        public static TeamIn BuildNewTeam(
            IOptions<FantasyConfig> fantasyConfigSection,
            IEnumerable<Position> positions)
        {
            uint totalplayers = (uint)positions.Sum(position => position.AmountInTeam);

            uint teamInitialBudget = fantasyConfigSection.Value.TeamInitialBudget;
            uint playerInitialValue = fantasyConfigSection.Value.PlayerInitialValue;

            return new TeamIn()
            {
                Name = TeamsPool[Rand.Next(TeamsPool.Length)],
                Country = CountriesPool[Rand.Next(CountriesPool.Length)],
                TotalValue = totalplayers * playerInitialValue,
                AvailableBudget = teamInitialBudget
            };
        }

        public static IEnumerable<PlayerIn> GeneratePlayers(
            IOptions<FantasyConfig> fantasyConfigSection,
            IEnumerable<Position> positions,
            int teamID)
        {
            uint playerInitialValue = fantasyConfigSection.Value.PlayerInitialValue;
            int playerMinAge = fantasyConfigSection.Value.PlayerMinAge;
            int playerMaxAge = fantasyConfigSection.Value.PlayerMaxAge;
            
            List<PlayerIn> players = new List<PlayerIn>();

            foreach (Position position in positions)
            {
                for (int i = 0; i < position.AmountInTeam; i++)
                {
                    players.Add(new PlayerIn()
                    {
                        FirstName = FirstNamesPool[Rand.Next(FirstNamesPool.Length)],
                        LastName = LastNamesPool[Rand.Next(LastNamesPool.Length)],
                        Country = CountriesPool[Rand.Next(CountriesPool.Length)],
                        Age = (uint)Rand.Next(playerMinAge, playerMaxAge + 1),
                        PositionRefID = position.ID,
                        TeamRefID = teamID,
                        MarketValue = playerInitialValue
                    });
                }
            }

            return players;
        }


        private static readonly string[] CountriesPool = {
            "Afghanistan", "Albania", "Algeria", "Andorra", "Angola",
            "Antigua and Barbuda", "Argentina", "Armenia", "Australia", "Austria",
            "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh", "Barbados",
            "Belarus", "Belgium", "Belize", "Benin", "Bhutan",
            "Bolivia", "Bosnia and Herzegovina", "Botswana", "Brazil", "Brunei",
            "Bulgaria", "Burkina Faso", "Burundi", "Côte d'Ivoire", "Cabo Verde",
            "Cambodia", "Cameroon", "Canada", "Chad", "Chile",
            "China", "Colombia", "Comoros", "Congo", "Costa Rica",
            "Croatia", "Cuba", "Cyprus", "Czechia", "Denmark",
            "Djibouti", "Dominica", "Dominican Republic", "Ecuador", "Egypt",
            "El Salvador", "Equatorial Guinea", "Eritrea", "Estonia", "Eswatini",
            "Ethiopia", "Fiji", "Finland", "France", "Gabon",
            "Gambia", "Georgia", "Germany", "Ghana", "Greece",
            "Grenada", "Guatemala", "Guinea", "Guinea-Bissau", "Guyana",
            "Haiti", "Holy See", "Honduras", "Hungary", "Iceland",
            "India", "Indonesia", "Iran", "Iraq", "Ireland",
            "Israel", "Italy", "Jamaica", "Japan", "Jordan",
            "Kazakhstan", "Kenya", "Kiribati", "Kuwait", "Kyrgyzstan",
            "Laos", "Latvia", "Lebanon", "Lesotho", "Liberia",
            "Libya", "Liechtenstein", "Lithuania", "Luxembourg", "Madagascar",
            "Malawi", "Malaysia", "Maldives", "Mali", "Malta",
            "Marshall Islands", "Mauritania", "Mauritius", "Mexico", "Micronesia",
            "Moldova", "Monaco", "Mongolia", "Montenegro", "Morocco",
            "Mozambique", "Myanmar", "Namibia", "Nauru", "Nepal",
            "Netherlands", "New Zealand", "Nicaragua", "Niger", "Nigeria",
            "North Korea", "North Macedonia", "Norway", "Oman", "Pakistan",
            "Palau", "Panama", "Papua New Guinea", "Paraguay", "Peru",
            "Philippines", "Poland", "Portugal", "Qatar", "Romania",
            "Russia", "Rwanda", "Saint Lucia", "Samoa", "San Marino",
            "Saudi Arabia", "Senegal", "Serbia", "Seychelles", "Sierra Leone",
            "Singapore", "Slovakia", "Slovenia", "Solomon Islands", "Somalia",
            "South Africa", "South Korea", "South Sudan", "Spain", "Sri Lanka",
            "Sudan", "Suriname", "Sweden", "Switzerland", "Syria",
            "Tajikistan", "Tanzania", "Thailand", "Togo", "Tonga",
            "Trinidad and Tobago", "Tunisia", "Turkey", "Turkmenistan", "Tuvalu",
            "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "United States of America",
            "Uruguay", "Uzbekistan", "Vanuatu", "Venezuela", "Vietnam",
            "Yemen", "Zambia", "Zimbabwe"
        };

        private static readonly string[] TeamsPool = {
            "Bayern München", "Manchester City", "Liverpool", "Barcelona", "Paris Saint-Germain",
            "Real Madrid", "Atlético Madrid", "Atalanta", "Inter Milan", "RasenBallsport Leipzig",
            "Flamengo", "Manchester United", "Borussia Dortmund", "Sevilla", "Ajax Amsterdam",
            "Shakhtar Donetsk", "AC Milan", "Juventus", "FC Porto", "Bayer Leverkusen",
            "Boca Juniors", "Palmeiras", "Benfica", "Borussia Mönchengladbach", "Chelsea FC",
            "Roma", "Salzburg", "SSC Napoli", "Lazio", "Zenit St. Petersburg",
            "Tottenham Hotspur", "Villarreal", "Arsenal", "Wolverhampton Wanderers", "River Plate",
            "Grêmio", "Olympiakos", "Celtic", "Hoffenheim", "Santos FC",
            "Slavia Prague", "Flora Tallinn", "PSV Eindhoven", "Lyon", "Racing Club",
            "Atlético PR", "Viktoria Plzeň", "FC Basel", "Marseille", "Al Hilal (KSA)",
            "Real Sociedad", "FK Red Star Belgrade", "Olimpia", "Southampton", "Burnley",
            "Leicester City", "Feyenoord", "Al Ahly", "Getafe", "AZ Alkmaar",
            "Dinamo Zagreb", "Granada", "Levante", "PAOK Thessaloniki FC", "Athletic Bilbao",
            "FCI Levadia Tallinn", "PFC Ludogorets 1945 Razgrad", "Fiorentina", "Internacional", "Valencia",
            "Dynamo Kyiv", "Rangers", "Maccabi Tel Aviv FC", "Lille", "Sporting",
            "Jeonbuk FC", "Young Boys", "Club Brugge", "Dundalk FC", "Wolfsburg",
            "Rennes", "FC Sheriff", "Nomme JK Kalju", "The New Saints", "Sheffield United FC",
            "Eintracht Frankfurt", "Esperance de Tunis", "CSKA", "Vélez Sarsfield", "APOEL Nicosia",
            "Sassuolo", "İstanbul Başakşehir", "Defensa y Justicia", "Braga", "CFR Cluj",
            "São Paulo", "Osasuna", "Tigres UANL", "Freiburg (GER)", "Mainz 05"
        };

        private static readonly string[] FirstNamesPool = {
            "Lionel", "Cristiano", "Virgil", "Jan", "Kevin",
            "Robert", "Eden", "Mohamed", "Sadio", "Sergio",
            "Kylian", "Harry", "Antoine", "Toni", "Luka",
            "Luis", "Manuel", "Sergio", "Raheem", "Roberto",
            "Kalidou", "Thibaut", "Sergio", "Karim", "Samir",
            "Giorgio", "Bernardo", "Joshua", "Aymeric", "Paulo",
            "Heung", "Marco", "Paul", "Keylor", "Ciro",
            "Christian", "Marco", "Wojciech", "Ángel", "Diego",
            "Mats", "Dries", "David", "Hugo", "Thiago",
            "Jadon", "Milan", "Frenkie", "Leroy", "Clément",
            "Andrew", "Bruno", "Timo", "Jamie", "Hakim",
            "Riyad", "Raphaël", "Mauro", "Lorenzo", "Romelu",
            "Jordi", "Leonardo", "Toby", "Miralem", "Edinson",
            "Yann", "Alejandro", "Matthijs", "Trent", "Gianluigi",
            "André", "Sergej", "Nabil", "José", "Niklas",
            "Serge", "Samuel", "Luis", "Stefan", "Iago",
            "Bernd", "Thomas", "Roman", "Péter", "Jordan",
            "Georginio", "Axel", "Gareth", "Jan", "Ivan",
            "Gonzalo", "Zlatan", "Kai", "Marcus", "Lautaro",
            "Wilfred", "Lucas"
        };

        private static readonly string[] LastNamesPool = {
            "Messi", "Ronaldo", "Neymar", "VanDijk", "Oblak",
            "Lewandowski", "Hazard", "Alisson", "Salah", "Mané",
            "TerStegen", "Agüero", "Mbappé", "Kanté", "Kane",
            "Griezmann", "Kroos", "Modrić", "Suárez", "Neuer",
            "Ramos", "Ederson", "Sterling", "Firmino", "Koulibaly",
            "Casemiro", "Courtois", "Busquets", "Aubameyang", "Benzema",
            "Handanovič", "Piqué", "Chiellini", "Silva", "Kimmich",
            "Laporte", "Dybala", "Verratti", "Pogba", "Navas",
            "Immobile", "Eriksen", "Parejo", "Reus", "Szczęsny",
            "Di-María", "Godín", "Hummels", "Mertens", "Silva",
            "Lloris", "Silva", "Sancho", "Škriniar", "DeJong",
            "Sané", "Lenglet", "Robertson", "Fernandes", "Werner",
            "Fabinho", "Vardy", "Ziyech", "Marquinhos", "Mahrez",
            "Varane", "Icardi", "Insigne", "Koke", "Lukaku",
            "Thiago", "Alba", "Bonucci", "Alderweireld", "Pjanić",
            "Cavani", "Sommer", "Gómez", "DeLigt", "Rodri",
            "AlexanderArnold", "Arthur", "Donnarumma", "Onana", "MilinkovićSavić",
            "Fekir", "Giménez", "Süle", "Saúl", "Gnabry",
            "Umtiti", "Carvajal", "Alberto", "DeVrij", "Pizzi",
            "Isco", "Aspas", "Leno", "Müller", "Coutinho",
            "Bürki", "Paulinho", "Gulácsi", "Henderson", "Wijnaldum",
            "Witsel", "Bale", "Vertonghen", "Rakitić", "Higuaín",
            "Fernandinho", "Ibrahimović", "Havertz", "Rashford", "Martínez",
            "Ndidi", "Hernández"
        };
    }
}
