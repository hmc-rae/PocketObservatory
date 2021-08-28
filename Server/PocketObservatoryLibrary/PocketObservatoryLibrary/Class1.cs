using System;
using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;
using System.Threading.Tasks;

namespace PocketObservatoryLibrary
{
    public class InterfaceFunctions
    {
        //So, funny story. I've never touched servers before.
        //This is the best thing I can come up with at 3am in the morning.
        //I need to touch grass.
        public static string BackendInterp(string query)
        {
            if (DateTimeOffset.Now.ToUnixTimeSeconds() - Globals.lastUpdate > 300)
            {
                UpdatePlanets();
                Globals.lastUpdate = DateTimeOffset.Now.ToUnixTimeSeconds();
            }

            string[] words = Functions.disect(query);
            double[] parmy = new double[8];
            for (int n = 0; n < 8; n++)
            {
                if (n+1 >= words.Length)
                {
                    break;
                }
                Double.TryParse(words[n + 1], out parmy[n]);
            }

            int firstParam = 0;
            if (words.Length >= 2)
            {
                int.TryParse(words[1], out firstParam);
            }

            string output = "";
            switch (words[0].ToLower())
            {
                case "updateplanets":
                    UpdatePlanets();
                    break;
                case "getall":
                    output = GetAll(parmy[0], parmy[1]);
                    break;
                case "getplanet":
                    output = GetPlanet(firstParam);
                    break;
                case "trackplanet":
                    int i = 1;
                    output = TrackPlanet(firstParam, parmy[i++], parmy[i++], parmy[i++], parmy[i++], parmy[i++]);
                    break;
            }
            return output;
        }
        public static String test()
        {
            return $"{Globals.planetCore.planets[0].currentRadians}";
        }

        /// <summary>
        /// Please call me during initial loading.
        /// </summary>
        public static void Initialize()
        {
            Globals.lastUpdate = DateTimeOffset.Now.ToUnixTimeSeconds();
            Functions.setupPlanets();
            for (int n = 0; n < Globals.planetCore.planets.Count; n++)
            {
                Globals.planetCore.planets[n].id = n;
                if (Globals.planetCore.planets[n].name == "Earth")
                {
                    Globals.earthReference = Globals.planetCore.planets[n];
                }
            }
            UpdatePlanets();
        }

        /// <summary>
        /// Updates the positions of each planet.
        /// </summary>
        public static void UpdatePlanets()
        {
            Functions.computePlanets();
            for (int n = 0; n < Globals.planetCore.planets.Count; n++)
            {
                Functions.getPlanetPosition(Globals.planetCore.planets[n]);
            }
        }

        /// <summary>
        /// Returns a JSON string with general information about all planets.
        /// </summary>
        /// <returns></returns>
        public static string GetAll(double lat, double lon)
        {
            bool[] visible = Functions.FindVisible(lat, lon);
            string jsonString = "{\"allPlanets\":[";
            for (int n = 0; n < Globals.planetCore.planets.Count; n++)
            {
                if (n != 0)
                {
                    jsonString += ",";
                }
                jsonString += "{\"name\":\"" + Globals.planetCore.planets[n].name;
                jsonString += "\",\"refID\":" + Globals.planetCore.planets[n].id;
                jsonString += ",\"order\":" + Globals.planetCore.planets[n].order;
                jsonString += ",\"color\":\"" + Globals.planetCore.planets[n].additionalData.colorScheme;
                jsonString += "\",\"visible\":";
                jsonString += visible[n] ? "true" : "false";
                jsonString += "}";
            }
            jsonString += "]}";

            return jsonString;
        }

        /// <summary>
        /// Returns a JSON string with detailed information about one of the planets as per ID.
        /// </summary>
        /// <param name="planetID"></param>
        /// <returns></returns>
        public static string GetPlanet(int planetID)
        {
            return JsonConvert.SerializeObject(Globals.planetCore.planets[planetID % Globals.planetCore.planets.Count].additionalData);
        }

        /// <summary>
        /// Returns rotational instructions to find the nearest visible planet.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <returns></returns>
        public static double[] TrackPlanet(double x, double y, double z, double lat, double lon)
        {
            List<Objects.PlanetData> visible = Functions.BuildVisible(lat, lon);
            return null;
        }

        /// <summary>
        /// Should return rotational instructions to find a specific visible planet. #0 is compass, #1 is elev
        /// </summary>
        /// <param name="planetID"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <returns></returns>
        public static string TrackPlanet(int planetID, double x, double y, double z, double lat, double lon)
        {
            Functions.getPlanetPosition(Globals.planetCore.planets[planetID]);
            double[] planetPos = Globals.earthReference.position;
            double[] targetPos = Globals.planetCore.planets[planetID].position;
            double[] personPos = Functions.get3DPosition(lat, lon, Globals.earthReference);

            double[] anglePerson = Functions.getAngles(planetPos, personPos);
            double[] angleTarget = Functions.getAngles(planetPos, targetPos);

            double[] oAngles = { anglePerson[0] - angleTarget[0], anglePerson[1] - angleTarget[1] };
            double distanceTarget = Functions.getDistance(planetPos, targetPos);

            double[] result = { 0, 0 };
            result[0] = -Math.Atan2(oAngles[1], oAngles[0]);
            result[1] = Math.Sqrt(Math.Pow(oAngles[0], 2) + Math.Pow(oAngles[1], 2)) - (Math.PI / 2);

            string jsonString = JsonConvert.SerializeObject(result);
            return jsonString;
        }
    }

    public static class Objects
    {
        /// <summary>
        /// Contains all planets.
        /// </summary>
        public class PlanetCore
        {
            [JsonProperty("planets")]
            public List<Objects.PlanetData> planets { get; set; }
        }
        /// <summary>
        /// Core object that stores the main values that we need to calculate where it is.
        /// </summary>
        public class PlanetData
        {
            public double meanDistance;  //Average distance from the sun. Once we get set up, I'll expand it to include perihelion and aphelion.
            private double orbitalPeriod; //Time it takes to orbit the sun once, in earth years (365.25 days)
            private double orbitalPeriodSeconds; //Time in seconds to orbit the sun once.

            public double OrbitalPeriod
            {
                get
                {
                    return orbitalPeriod;
                }
                set
                {
                    orbitalPeriod = value;
                    //calc seconds
                    orbitalPeriodSeconds = orbitalPeriod * Constants.YearSeconds;

                }
            }
            public double OrbitalPeriodSeconds
            {
                get
                {
                    return orbitalPeriodSeconds;
                }
            }
            public double orbitalOffset; //Value used to offset the start from 0 radians at the LOCAL epoch (1st jan @ midnight, 2018)

            public double currentRadians; //Current angular difference from 0 to sun

            public int id; //Used for tracking.
            public string name; //Go figure.
            public int order; //The order from the sun it is.
            public bool ignore; //Whether or not to ignore this file.

            public double[] position;

            public PlanetExtension additionalData; //Contains all extra data.
        }
        /// <summary>
        /// An extension pack that stores information about the Planet.
        /// </summary>
        public class PlanetExtension
        {
            public string colorScheme; //For potential UI elements.

            public double perihelion;
            public double aphelion;

            public double eccentricity;
            public double inclination;

            public double radius;

            public string description;
        }
    }
    
    public static class Functions
    {
        public static bool[] FindVisible(double lat, double lon)
        {
            bool[] visible = new bool[Globals.planetCore.planets.Count];
            double[] position = get3DPosition(lat, lon, Globals.earthReference);

            for (int n = 0; n < Globals.planetCore.planets.Count; n++)
            {
                if (Globals.planetCore.planets[n].name == "Earth")
                {
                    continue;
                }
                double earthDistance = getDistance(Globals.planetCore.planets[n].position, Globals.earthReference.position);
                double meDistance = getDistance(Globals.planetCore.planets[n].position, position);

                visible[n] = false;
                if (meDistance > earthDistance)
                {
                    visible[n] = true;
                }
            }

            return visible;
        }

        public static List<Objects.PlanetData> BuildVisible(double lat, double lon)
        {
            bool[] visible = FindVisible(lat, lon);
            List<Objects.PlanetData> visiblePlanets = new List<Objects.PlanetData>();
            for (int n = 0; n < visible.Length; n++)
            {
                if (visible[n])
                {
                    visiblePlanets.Add(Globals.planetCore.planets[n]);
                }
            }

            return visiblePlanets;
        }

        public static double[] get3DPosition(double lat, double lon, Objects.PlanetData planet)
        {
            double[] pos = get3DVector(lat, lon, planet);
            for (int n = 0; n < 3; n++)
            {
                pos[n] *= planet.additionalData.radius;
                pos[n] += planet.position[n];
            }

            return pos;
        }

        public static double[] get3DVector(double lat, double lon, Objects.PlanetData planet)
        {
            double[] pos = { 0, 0, 0 };

            Boolean latNeg = lat < 0;
            if (latNeg)
            {
                lat = lat;
            }

            double angleToSun = (Math.PI / 2) - planet.currentRadians;

            //substitute longitude - timezones are more relevant (if we assume that at midday you're facing the sun)
            //used for x/y
            //lon = (Math.PI / 180) * (((DateTimeOffset.Now.Hour - 12) * 15) + ((180 / Math.PI) * normalize((Math.PI / 2) - planet.currentRadians)));
            double n = DateTime.UtcNow.Hour;
            n -= 2;
            n *= 0.261799;
            n = normalizeNoFlip(angleToSun + n);
            lon = n + (lon * (Math.PI / 180));

            //used for elev
            lat = lat * (Math.PI / 180); //convert to radians too

            pos[0] = (Math.Sin(lon) * 1) * Math.Cos(lat);
            pos[1] = (Math.Cos(lon) * 1) * Math.Cos(lat);
            pos[2] = Math.Sin(lat) * 1;

            return pos;
        }

        public static void getPlanetPosition(Objects.PlanetData planet)
        {
            planet.position = new double[3];
            planet.position[2] = 0;
            planet.position[0] = Math.Sin(planet.currentRadians) * planet.meanDistance;
            planet.position[1] = Math.Cos(planet.currentRadians) * planet.meanDistance;
        }

        public static double normalize(double value)
        {
            value = normalizeNoFlip(value);
            if (value < 0)
            {
                value = Math.PI + value;
            }
            return value;
        }
        public static double normalizeNoFlip(double value)
        {
            while (value < -Math.PI) { value = (2 * Math.PI) + value; }
            while (value > Math.PI) { value = -(2 * Math.PI) + value; }
            return value;
        }
        public static void setupPlanets()
        {
            StreamReader r = new StreamReader("planets.json");
            String temp = r.ReadToEnd();
            r.Close();
            Globals.planetCore = JsonConvert.DeserializeObject<Objects.PlanetCore>(temp);
        }
        public static void computePlanets()
        {
            long timeStamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            timeStamp -= Constants.LocalEpoch;
            double yearsSince = timeStamp / (double)Constants.YearSeconds;

            for (int n = 0; n < Globals.planetCore.planets.Count; n++)
            {
                Globals.planetCore.planets[n].currentRadians = normalize(Globals.planetCore.planets[n].orbitalOffset + ((yearsSince / Globals.planetCore.planets[n].OrbitalPeriod) * Math.PI));
                Globals.planetCore.planets[n].currentRadians += Constants.CorrectionalConst;
            }
        }

        /// <summary>
        /// Gets angle the user is facing in the universal 0. Should work.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <returns></returns>
        public static double[] getFixedAngles(double x, double y, double z, double lat, double lon)
        {
            double[] normalizedVector = { 0, 0, 0 };
            double[] abnormalVector = { x, y, z };

            return normalizedVector;
        }

        public static double[] getAngles(double[] a, double[] b)
        {
            double[] result = { 0, 0 };
            double r = getDistance(a, b);
            result[0] = Math.Atan2(b[1] - a[1], b[0] - a[0]); //pan
            result[1] = Math.Atan2(r, b[2] - a[2]);           //yaw
            return result;
        }

        public static double getDistance(double[] a, double[] b)
        {
            return Math.Sqrt(Math.Pow(a[0] - b[0], 2) + Math.Pow(a[1] - b[1], 2) + Math.Pow(a[0] - b[0], 2));
        }

        public static string[] disect(string input)
        {
            string[] result = input.Split(',');
            return result;
        }
    }

    public static class Constants
    {
        public const long LocalEpoch = 1514725200;
        public const long YearSecondsShort = 31536000;
        public const double CorrectionalConst = 0.0872665;
        public const long YearSeconds = 31557600;
    }

    public static class Globals
    {
        public static long lastUpdate = -1;
        public static Objects.PlanetCore planetCore;
        public static Objects.PlanetData earthReference;
    }
}
