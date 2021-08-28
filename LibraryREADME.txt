All functions which you will need are located in PocketObservatoryLibrary.dll. They are in:
PocketObservatoryLibrary.InterfaceFunctions

FUNCTIONS:
		Initialize()
			returns void
		
	Builds the planet data. Updates the planet positions.
	
		UpdatePlanets()
			returns void
	
	Forces an update of the planet positions.
	
		TrackPlanet(double x, double y, double z, double lat, double lon)
			returns [ double x, double y, double z, double val ]
	
	Given GPS latitude & longitude as well as X, Y and Z gyroscope values, returns X, Y and Z difference to find the nearest visible planet.
	4th return value 'val' will be 0 if no planets are visible.
	
		TrackPlanet(int planetID, double x, double y, double z, double lat, double lon)
			returns [ double x, double y, double z, double val ]
	
	Given the unique ID of one of the tracked planets, as well as GPS and gyroscope values, finds the X, Y and Z difference to the planet.
	4th return value 'val' will be 0 if the planet is not visible.
	
		GetAll(double lat, double lon)
			returns string
	
	Returns a JSON string containing name of planet, order of planet, planetID, and whether it's visible or not. Earth is always tagged as not visible.
	Must take the latitude & longitude of the phone when querying.
	
		GetPlanet(int planetID)
			returns string
	
	Returns a JSON string containing in-depth planet data.