import * as React from 'react';
import { Text, View, StyleSheet, FlatList } from 'react-native';
import Constants from 'expo-constants';
import PlanetCard from '../components/PlanetCard';


export default function VisiblePlanets( {planets} ) {
  console.log(planets);
  return (
    <View style={styles.container}>
      <Text style={styles.title}>Visible Planets</Text>
      <FlatList
      data = {planets}
      renderItem = {({item: planet}) => (<PlanetCard planet = {planet}/>)}/>
    </View>
  );
}

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
    backgroundColor: "#E5E5E5",
    padding:10,
    borderRadius: 5,
  },
  itemTitle: {
    margin: 10,
    fontSize: 30,
    fontWeight: 'light',
    textAlign: 'left',
    color: 'black',
  },
  itemDescription: {
    margin: 10,
    fontSize: 20,
    fontWeight: 'light',
    textAlign: 'left',
    color: 'black',
  },
  
});
