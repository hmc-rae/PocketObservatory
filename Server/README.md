# All the code for the backend server goes here
The backend is mainly a big calculator. The planets.json file has data for each of the planets in the program, including orbital period, mean orbital difference, and an 'orbital offset' for the local epoch, which we've set to 1st January 2018 at Midnight.
The 'orbital offset' is calculated by hand by knowing the Hohmann transfer window for that planet - if we know the angle of transfer window start as well as when a transfer window started, we can calculate almost exactly what % of orbit that planet had made by the start of a given year.

At init, or every 5 minutes, the realtime position of each planet is recalculated.
