import * as React from 'react';
import { Text, View, StyleSheet,TouchableOpacity,Image } from 'react-native';
import Constants from 'expo-constants';
import PlanetDetails from './PlanetDetails';


let order = ["", "first",  "second", "third", "fourth", "fifth", "sixth", "seventh", "eighth", "ninth"];

// const detailsNavigation = (id, name) => {
//    navigation.
// };

const PlanetCard = ( {planet} ) => {
    return (
      <TouchableOpacity style = {styles.button}>  {/* onPress = {() => detailsNavigation(planet.refID, planet.name)}> */}
        <View style={styles.itemContainer}>
            <Text style={styles.itemTitle}> {planet.name} </Text> 
            <Text style={styles.itemDescription}> {`The ${order[planet.order]} planet of the solar system.`} </Text>
        </View>
      </TouchableOpacity>
    );
};

const styles = StyleSheet.create({
  container: {
    backgroundColor: '#130D45',
    paddingTop: Constants.statusBarHeight,
    flex: 1,
    padding: 8,
  },
  button : {
    borderRadius: 20,
    padding: 10,
    marginBottom: 20,
    display: "block",
  },

  title: {
    fontWeight: 'bold',
    fontSize: 40,
    color: '#FFFFFF',
    margin:20,
  },
  itemContainer: {
    backgroundColor: "#2A2557",
    padding:10,
    borderRadius: 5,
  },
  itemTitle: {
    margin: 10,
    fontSize: 30,
    fontWeight: 'light',
    textAlign: 'left',
    color: 'white',
  },
  itemDescription: {
    margin: 10,
    fontSize: 20,
    fontWeight: 'light',
    textAlign: 'left',
    color: 'white',
  },
  
});

export default PlanetCard;