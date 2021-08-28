import React from 'react';
import { StyleSheet, Text, View} from 'react-native';
import {NavigationContainer } from '@react-navigation/native';
import { createMaterialBottomTabNavigator } from '@react-navigation/material-bottom-tabs'

import Camera from './src/views/Camera';
import VisiblePlanets from './src/views/VisiblePlanets';
import AllPlanets from './src/views/AllPlanets';


const Tab = createMaterialBottomTabNavigator();

const App = () => {

  return (
      <NavigationContainer>
        <Tab.Navigator 
        initialRouteName = "Camera">
          <Tab.Screen name = "VisiblePlanets"
          component = {VisiblePlanets}/>
        <Tab.Screen name = "Camera"
          component = {Camera}/>
        <Tab.Screen name = "AllPlanets"
          component = {AllPlanets}/>
        </Tab.Navigator>
      </NavigationContainer>
  );
  // <View>
  //   <Text>hi</Text>
  // </View>);

};

const styles = StyleSheet.create({
  main: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  }
});

export default App;

