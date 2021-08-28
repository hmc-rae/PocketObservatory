using System;
using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace PocketObservatoryLibrary
{
    public class InterfaceFunctions
    {
        public static String test()
        {
            return $"{Globals.planetCore.planets[1].currentRadians}";
        }

        /// <summary>
        /// Please call me during initial loading.
        /// </summary>
        public static void Initialize()
        {
            Functions.setupPlanets();
            for (int n = 0; n < Globals.planetCore.planets.Count; n++)
            {
                Globals.planetCore.planets[n].id = n;
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

        public static List<Objects.PlanetData> BuildVisible(double lat, double lon)
        {
            return null;
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
            List<Objects.PlanetData> visible = BuildVisible(lat, lon);
            return null;
        }

        /// <summary>
        /// Returns rotational instructions to find a specific visible planet.
        /// </summary>
        /// <param name="planetID"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <returns></returns>
        public static double[] TrackPlanet(int planetID, double x, double y, double z, double lat, double lon)
        {
            return null;
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
            public double radius;
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

            public PlanetData()
            {
                additionalData = new PlanetExtension();
            }
        }
        /// <summary>
        /// An extension pack that stores information about the Planet.
        /// </summary>
        public class PlanetExtension
        {
            public string colorScheme; //For potential UI elements.

            public PlanetExtension()
            {
                colorScheme = "#000000";
            }
        }
    }
    
    public static class Functions
    {
        public static double[] get3DPosition(double lat, double lon, Objects.PlanetData planet)
        {
            double[] pos = get3DVector(lat, lon, planet);
            for (int n = 0; n < 3; n++)
            {
                pos[n] *= planet.radius;
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
                lat = -lat;
            }

            //substitute longitude - timezones are more relevant (if we assume that at midday you're facing the sun)
            //used for x/y
            lon = (Math.PI / 180) * (((DateTimeOffset.Now.Hour - 12) * 15) + ((180 / Math.PI) * normalize((Math.PI / 2) - planet.currentRadians)));

            //used for elev
            lat = (Math.PI / 180) * lat; //convert to radians too

            pos[0] += (Math.Sin(lon) * 1) * Math.Cos(lat);
            pos[1] += (Math.Cos(lon) * 1) * Math.Cos(lat);
            pos[2] += Math.Sin(lat) * 1;

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
            while (value < -Math.PI) { value = (2 * Math.PI) + value; }
            while (value > Math.PI) { value = -(2 * Math.PI) + value; }
            if (value < 0)
            {
                value = Math.PI + value;
            }
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
                Globals.planetCore.planets[n].currentRadians = normalize(Globals.planetCore.planets[n].orbitalOffset + ((Globals.planetCore.planets[n].OrbitalPeriod * yearsSince) * Math.PI));
            }
        }

        /// <summary>
        /// Gets angle the user is facing in the universal 0.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <returns></returns>
        public static double[] getFixedAngles(double x, double y, double z, double lat, double lon)
        {
            double[] data = { 0, 0, 0 };
            //We're assuming x&y is compass direction, with Y being north/south, x being east/west. z is elevation so we worry about it later. 
            double[] forwardVector = { x, y, z };
            data[0] = (x * x) + (y * y) + (z * z);
            

            //1 is how far left/right cam is facing
            return data;
        }
    }

    public static class Constants
    {
        public const long LocalEpoch = 1514725200;
        public const long YearSeconds = 31557600;
    }

    public static class Globals
    {
        public static Objects.PlanetCore planetCore;
    }
}
