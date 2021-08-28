import React, { useEffect, useState } from 'react';
import { StyleSheet, Text, View} from 'react-native';
import {NavigationContainer } from '@react-navigation/native';
import { createMaterialBottomTabNavigator } from '@react-navigation/material-bottom-tabs'
import { Octicons } from '@expo/vector-icons';

import Camera from './src/views/Observatory';
import VisiblePlanets from './src/views/VisiblePlanets';
import AllPlanets from './src/views/AllPlanets';

const Tab = createMaterialBottomTabNavigator();


const App = () => {

  const [planets, setPlanets] = useState(
  {"allPlanets":
      [{"name":"Mercury","refID":0,"order":1,"color":"","visible":false},
      {"name":"Venus","refID":1,"order":2,"color":"","visible":false},
      {"name":"Earth","refID":2,"order":3,"color":"","visible":false},
      {"name":"Mars","refID":3,"order":4,"color":"","visible":false},
      {"name":"Jupiter","refID":4,"order":5,"color":"","visible":true}]});

// useEffect(() => {
//   const getPlanets = async () => {
//     const planetsFromServer = await fetchPlanets()
//     setPlanets(planetsFromServer)
//   }
//   getPlanets();
// }, [])

// const fetchPlanets = async () => {
//   const response = await fetch ('')
//   const data = await response.json();
//   return data;
// }

  return (
      <NavigationContainer>
        <Tab.Navigator 
        initialRouteName = "Camera"
        activeColor="#8A2BE2"
        barStyle = {{backgroundColor: '#130D45'}}
        >
       
       <Tab.Screen name = "Viewable Planets"
          component = {() => <VisiblePlanets planets = {planets.allPlanets.filter((planet) => planet.visible)}/>}
        options={{
          tabBarIcon: ({ color }) => (
            <Octicons name="telescope" color={color} size={26} />
          ),
        }}
          />
      
        <Tab.Screen name = "Camera"
          component = {Camera}
        options={{
          tabBarIcon: ({ color }) => (
            <Octicons name="device-camera" color={color} size={26} />
          ),
        }}
          />
        <Tab.Screen name = "All Planets"
          component = {() => <AllPlanets planets = {planets.allPlanets}/>}
          options={{
          tabBarIcon: ({ color }) => (
            <Octicons name="three-bars" color={color} size={26} 
            />
          ),
        }}
          />
        </Tab.Navigator>
      </NavigationContainer>
  );
};

const styles = StyleSheet.create({
  main: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  }
});

export default App;
