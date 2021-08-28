﻿using System;
using System.IO;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace PocketObservatoryLibrary
{
    public class InterfaceFunctions
    {
        public static String test()
        {
            return Globals.planetCore.planets[0].name;
        }

        /// <summary>
        /// Please call me (:
        /// </summary>
        public static void Initialize(string name)
        {
            Functions.setupPlanets(name);
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
            private long orbitalPeriodSeconds;

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
                }
            }
            public double orbitalOffset; //Value used to offset the start from 0 radians at epoch.

            public double currentRadians; //Current angular difference from 0 to sun

            public string name; //Go figure.
            public bool ignore; //Whether or not to ignore this file.

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
        public static void setupPlanets(string name = "planets.json")
        {
            StreamReader r = new StreamReader(name);
            String temp = r.ReadToEnd();
            r.Close();
            Globals.planetCore = JsonConvert.DeserializeObject<Objects.PlanetCore>(temp);
        }
    }

    public static class Constants
    {

    }

    public static class Globals
    {
        public static Objects.PlanetCore planetCore;
    }
}